using System;
using System.Collections.Generic;
using System.Collections;
using System.Data;
using System.Linq;
using System.Xml;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using SCM.RabbitMQClient;

namespace Instinct.RabbitMQ.FraudCheckWinService.BLL
{
    public class InstinctBusiness
    {
        #region 成员
        private DAL.InstinctFraudCheckThread checker;

        private SCM.RabbitMQClient.IRabbitMqClient senderObj;
        private List<SCM.RabbitMQClient.IRabbitMqClient> receiverObjList;

        public string SendExchange { get; set; }
        public string SendQueueName { get; set; }

        public int ProcessCount { get; set; }

        public ClsOnlineServiceCall.SetINIValue ConfigObj { get; set; }

        public delegate SCM.RabbitMQClient.IRabbitMqClient BuildClient();

        public BuildClient SenderClient { get; set; }
        public BuildClient ReceiverClient { get; set; }

        private static readonly object SynObject = new object();
        public Dictionary<int, DAL.ReferenceTable> ReferenceHistory { get; set; }

        public const string APPKEY = "AppKey";

        public ParticipleParse parse;
        #endregion

        #region 构造函数
        public InstinctBusiness(BuildClient sender, BuildClient receiver, string sendexchange, string sendqueue, int processCount, ClsOnlineServiceCall.SetINIValue oConfig)
        {
            this.SenderClient = sender;
            this.ReceiverClient = receiver;
            this.SendExchange = sendexchange;
            this.SendQueueName = sendqueue;
            this.ProcessCount = processCount;
            this.senderObj = this.SenderClient();
            this.receiverObjList = new List<IRabbitMqClient>();

            if (this.ProcessCount > 0)
            {
                for (int i = 0; i < this.ProcessCount; i++)
                {
                    SCM.RabbitMQClient.IRabbitMqClient rclient = ReceiverClient();
                    this.receiverObjList.Add(rclient);
                }
            }

            this.ConfigObj = oConfig;

            SCM.RabbitMQClient.Common.LogLocation.Log = new Util.LogHelper();
            checker = new DAL.InstinctFraudCheckThread(this.ConfigObj, Util.LogHelper.ErrorLog);

            ReferenceHistory = new Dictionary<int, DAL.ReferenceTable>();
            //加分词
            if (Util.GlobalVariable.BParticiple)
            {
                parse = new ParticipleParse();
                parse.Init();
            }
        }

        #endregion

        #region 公有函数
        /// <summary>
        /// 调用欺诈检查
        /// </summary>
        public void Work()
        {
            try
            {
                Util.LogHelper.InfoLog("START TO LISTENING...");
                if (this.ProcessCount > 0)
                {
                    for (int i = 0; i < this.ProcessCount; i++)
                    {
                        SCM.RabbitMQClient.IRabbitMqClient receiver = this.receiverObjList[i];
                        Listening(receiver);
                    }
                }
            }
            catch (Exception ex)
            {
                Util.LogHelper.ErrorLog("LISTENING ERROR", ex);
            }
        }

        #endregion

        #region 私有函数

        private void Listening(SCM.RabbitMQClient.IRabbitMqClient receiver)
        {
            receiver.ActionEventMessage += mqClient_ActionEventMessage;
            receiver.OnListening();
        }
        /// <summary>
        /// 从消息队列中得到数据，开启欺诈检查
        /// 文本"|"格式，不处理Reference Tables
        /// </summary>
        /// <param name="result"></param>
        private void mqClient_ActionEventMessage(EventMessageResult result)
        {
            Util.LogHelper.InfoLog("START TO FRAUD CHECK...");

            //BasicProperties
            string sCorrelationId = result.CorrelationId == null ? "" : result.CorrelationId;
            string sReplyTo = result.ReplyTo == null ? "" : result.ReplyTo;
            string sContentType = result.ContentType == null ? "" : result.ContentType;

            string sAppkey = string.Empty;
            //分词
            ParticipleParameter pParticiple = null;

            bool IsOperationSuccess = true;

            string sInput = string.Empty;
            string sOutput = string.Empty;
            string sMarkCode = string.Empty;

            //引用表相关
            string sInputWithOutRef = string.Empty;
            XmlNodeList ReferenceXMLList = null;

            EventMessage sendMessage = null;
            Encoding encoder = Util.Tool.GetEncoding(Util.GlobalVariable.MqEncoding);
            bool bReply = true;
            StringBuilder errMsg = new StringBuilder();
            DAL.Error err = new DAL.Error();

            try
            {
                sInput = MessageSerializerFactory.CreateMessageJsonSerializerInstance(Util.GlobalVariable.MqEncoding)
                                .Deserialize(result.MessageBytes);
                Util.LogHelper.InfoLog(String.Format("INPUT={0}", sInput));

                sMarkCode = result.EventMessageBytes.EventMessageMarkcode;
                Util.LogHelper.InfoLog(String.Format("EventMessageMarkcode={0}", sMarkCode));

                switch (result.EventMessageBytes.ResponseMethod.ToUpper())
                {
                    // 带引用表数据的XML格式欺诈检查
                    case Util.MessageTypeConst.InstinctFraudCheck_XMLString:
                        //欺诈检查处理
                        Util.LogHelper.InfoLog("CALLING METHOD[" + Util.MessageTypeConst.InstinctFraudCheck_XMLString + "]...");
                        if (Util.GlobalVariable.BParticiple)
                        {
                            sAppkey = GetAppKeyAndParticipleByInXml(sInput, out pParticiple, err);
                        }
                        else
                        {
                            sAppkey = GetAppkeyByInXml(sInput, err);
                        }
                        //判断当前正在处理的件是否有与当前要处理的件AppKey 相同，
                        //相同，等待5分钟

                        if (!ApplicationsTrace.Instance.AddElement(sAppkey))
                        {
                            Util.LogHelper.InfoLog(string.Format("THE FRAUD CHECKING APPLICATIONS CONTAIN THE CURREN APPLICAITON [APPKEY={0}].", sAppkey));
                            Util.LogHelper.InfoLog(string.Format("WAIT FOR {0} SECONDS.", Util.GlobalVariable.WaitingSeconds));
                            Task.Delay(Util.GlobalVariable.WaitingSeconds * 1000);
                            IsOperationSuccess = false;
                            return;
                        }

                        //xml 格式数据                        
                        //分离业务表数据与引用表数据  & 处理分词                      
                        ReferenceXMLList = GetReferenceTableNode(sAppkey, sInput, out sInputWithOutRef);
                        if (ProcessXMLReferenceTableList(sAppkey, ReferenceXMLList, ref errMsg, err) && ProcessParticiple(sAppkey, pParticiple, ref errMsg, err))
                        {
                            Util.LogHelper.InfoLog(String.Format("PROCESS TO FRAUD CHECKING APPKEY={0}--BEGIN", sAppkey));
                            try
                            {
                                sOutput = this.checker.InstinctFraudCheck_XMLString(sInputWithOutRef);
                            }
                            catch (Exception fraudex)
                            {
                                err.Error_Code = "99";
                                err.Remark = fraudex.Message;
                                sOutput = err.ToOutputSchemaString();
                            }
                            finally
                            {
                                Util.LogHelper.InfoLog(String.Format("PROCESS TO FRAUD CHECKING APPKEY={0}--END", sAppkey));
                            }
                        }
                        else
                        {
                            Util.LogHelper.ErrorLog(string.Format("LOAD REFERENCE & PARTICIPLE[APPKEY={0}]", sAppkey), new Exception(errMsg.ToString()));
                            sOutput = err.ToOutputSchemaString();
                        }


                        Util.LogHelper.InfoLog(string.Format("OUTPUT[APPKEY={0}]={1}", sAppkey, sOutput));
                        sendMessage =
                            EventMessageFactory.CreateEventMessageInstance(sOutput, sMarkCode, encoder, Util.MessageTypeConst.InstinctFraudCheck_XMLString);

                        break;
                    // 带引用表数据的XML格式欺诈检查
                    case Util.MessageTypeConst.InstinctFraudCheck_JSONString:
                        //欺诈检查处理
                        Util.LogHelper.InfoLog("CALLING METHOD[" + Util.MessageTypeConst.InstinctFraudCheck_JSONString + "]...");
                        //Json to xml                        
                        sInput = SCM.RabbitMQClient.Common.JsonXmlObjectParser.JsonToXml(sInput);
                        if (Util.GlobalVariable.BParticiple)
                        {
                            sAppkey = GetAppKeyAndParticipleByInXml(sInput, out pParticiple, err);
                        }
                        else
                        {
                            sAppkey = GetAppkeyByInXml(sInput, err);
                        }
                        //判断当前正在处理的件是否有与当前要处理的件AppKey 相同，
                        //相同，等待5分钟
                        if (!ApplicationsTrace.Instance.AddElement(sAppkey))
                        {
                            Util.LogHelper.InfoLog(string.Format("THE FRAUD CHECKING APPLICATIONS CONTAIN THE CURREN APPLICAITON [APPKEY={0}].", sAppkey));
                            Util.LogHelper.InfoLog(string.Format("WAIT FOR {0} SECONDS.", Util.GlobalVariable.WaitingSeconds));
                            Task.Delay(Util.GlobalVariable.WaitingSeconds * 1000);
                            IsOperationSuccess = false;
                            return;
                        }

                        //xml 格式数据
                        //分离业务表数据与引用表数据  & 处理分词                           
                        ReferenceXMLList = GetReferenceTableNode(sAppkey, sInput, out sInputWithOutRef);
                        if (ProcessXMLReferenceTableList(sAppkey, ReferenceXMLList, ref errMsg, err) && ProcessParticiple(sAppkey, pParticiple, ref errMsg, err))
                        {
                            Util.LogHelper.InfoLog(String.Format("PROCESS TO FRAUD CHECKING APPKEY={0}--BEGIN", sAppkey));
                            try
                            {
                                sOutput = this.checker.InstinctFraudCheck_XMLString(sInputWithOutRef);
                            }
                            catch (Exception fraudex)
                            {
                                err.Error_Code = "99";
                                err.Remark = fraudex.Message;
                                sOutput = err.ToOutputSchemaString();
                            }
                            finally
                            {
                                Util.LogHelper.InfoLog(String.Format("PROCESS TO FRAUD CHECKING APPKEY={0}--END", sAppkey));
                            }
                        }
                        else
                        {
                            Util.LogHelper.ErrorLog(string.Format("LOAD REFERENCE & PARTICIPLE[APPKEY={0}]", sAppkey), new Exception(errMsg.ToString()));
                            sOutput = err.ToOutputSchemaString();
                        }


                        //xml to json                            
                        sOutput = SCM.RabbitMQClient.Common.JsonXmlObjectParser.XmlToJson(sOutput);
                        Util.LogHelper.InfoLog(string.Format("OUTPUT[APPKEY={0}]={1}", sAppkey, sOutput));

                        sendMessage =
                            EventMessageFactory.CreateEventMessageInstance(sOutput, sMarkCode, encoder, Util.MessageTypeConst.InstinctFraudCheck_JSONString);

                        break;

                    //case Util.MessageTypeConst.InstinctFraudCheck_String:
                    //    //欺诈检查处理
                    //    Util.LogHelper.InfoLog("CALLING METHOD[" + Util.MessageTypeConst.InstinctFraudCheck_String + "]...");
                    //    sAppkey = getAppkeyByTXT(sInput);
                    //    if (ApplicationsTrace.Instance.ContainsKey(sAppkey))
                    //    {
                    //        ApplicationsTrace.Instance[sAppkey]++;
                    //        if (ApplicationsTrace.Instance[sAppkey] > 150)
                    //        {
                    //            ApplicationsTrace.Instance.Remove(sAppkey);
                    //        }
                    //        else
                    //        {
                    //            Util.LogHelper.InfoLog(string.Format("THE FRAUD CHECKING APPLICATIONS CONTAIN THE CURREN APPLICAITON [APPKEY={0}].", sAppkey));
                    //            Util.LogHelper.InfoLog("WAIT FOR 2 SECONDS.");
                    //            Task.Delay(2000);
                    //            IsOperationSuccess = false;
                    //            return;
                    //        }
                    //    }

                    //    //TXT 格式数据
                    //    try
                    //    {
                    //        if (!ApplicationsTrace.Instance.ContainsKey(sAppkey))
                    //        {
                    //            ApplicationsTrace.Instance.Add(sAppkey, 1);
                    //        }
                    //        try
                    //        {
                    //            sOutput = this.checker.InstinctFraudCheck_String(sInput);
                    //        }
                    //        catch (Exception fraudex)
                    //        {
                    //            err.Error_Code = "99";
                    //            err.Remark = fraudex.Message;
                    //            sOutput = err.ToTXTString();
                    //        }
                    //    }
                    //    finally
                    //    {
                    //        if (ApplicationsTrace.Instance.ContainsKey(sAppkey))
                    //            ApplicationsTrace.Instance.Remove(sAppkey);
                    //    }

                    //    Util.LogHelper.InfoLog(string.Format("OUTPUT[APPKEY={0}]={1}", sAppkey, sOutput));
                    //    sendMessage =
                    //        EventMessageFactory.CreateEventMessageInstance(sOutput, sMarkCode, encoder, Util.MessageTypeConst.InstinctFraudCheck_String);
                    //    break;

                    default:
                        Util.LogHelper.InfoLog(String.Format("THERE IS NO MATCHING FUNCTION,INPUT={0}", sInput));
                        //不匹配ResponseMethod，无处理函数
                        err.Error_Code = "100";
                        sOutput = err.ToOutputSchemaString();
                        err.Remark = String.Format("INPUT={0}", sInput);

                        sendMessage =
                            EventMessageFactory.CreateEventMessageInstance(sOutput, sMarkCode, encoder, "DEFAULT");
                        break;
                }
            }
            catch (Exception ex)
            {
                bReply = false;
                Util.LogHelper.ErrorLog("FRAUD CHECK ERROR", ex);
            }
            finally
            {
                ApplicationsTrace.Instance.RemoveElement(sAppkey);
                if (IsOperationSuccess)
                    result.IsOperationOk = true; //告诉消息队列接收成功。 
            }

            //发送返回值到队列中
            if (bReply)
            {
                try
                {
                    if (Util.GlobalVariable.MqSynchronization)
                    {
                        sReplyTo = sReplyTo != null && sReplyTo != "" ? sReplyTo : this.SendQueueName;
                        Util.LogHelper.InfoLog(string.Format("BEGIN TO SEND FOR QUEUE INFO[APPKEY={0},SYNCHRONIZATION=TRUE]:REPLYTO={1},CORRELATIONID={2},CONTENTTYPE={3}", sAppkey, sReplyTo, sCorrelationId, sContentType));
                        this.senderObj.TriggerEventMessage(sendMessage, this.SendExchange, sReplyTo, sReplyTo, sCorrelationId, sContentType);
                        Util.LogHelper.InfoLog(string.Format("END TO SEND FOR QUEUE[APPKEY={0},SYNCHRONIZATION=TRUE]:REPLYTO={1},CORRELATIONID={2},CONTENTTYPE={3}", sAppkey, sReplyTo, sCorrelationId, sContentType));
                    }
                    else
                    {
                        Util.LogHelper.InfoLog(string.Format("BEGIN TO SEND FOR QUEUE INFO[APPKEY={0},SYNCHRONIZATION=FALSE]:REPLYTO={1}", sAppkey, this.SendQueueName));
                        this.senderObj.TriggerEventMessage(sendMessage, this.SendExchange, this.SendQueueName);
                        Util.LogHelper.InfoLog(string.Format("END TO SEND FOR QUEUE INFO[APPKEY={0},SYNCHRONIZATION=FALSE]:REPLYTO={1}", sAppkey, this.SendQueueName));
                    }
                }
                catch (Exception oex)
                {
                    Util.LogHelper.ErrorLog("FAIL TO SEND TO REPLY MESSAGE", oex);
                }

            }

            Util.LogHelper.InfoLog("END TO FRAUD CHECK.");
        }

        ///// <summary>
        ///// 解析欺诈检查接口input订单APpkey
        ///// </summary>
        ///// <param name="application"></param>
        ///// <returns></returns>
        //private string getAppkeyByOutXml(string output, DAL.Error err = null)
        //{
        //    string appkey = string.Empty;
        //    XmlDocument doc = new XmlDocument();
        //    doc.LoadXml(output);
        //    XmlElement root = doc.DocumentElement;
        //    string sOrganisation = root.SelectSingleNode("/OutputSchema/Output/Organisation").InnerText;
        //    string sCN = root.SelectSingleNode("/OutputSchema/Output/Country_Code").InnerText;
        //    string sApplicationNumber = root.SelectSingleNode("/OutputSchema/Output/Application_Number").InnerText;
        //    string sType = root.SelectSingleNode("/OutputSchema/Output/Application_Type").InnerText;
        //    appkey = sOrganisation.Trim() + sCN.Trim() + sApplicationNumber.Trim() + sType.Trim();
        //    if (err != null)
        //    {
        //        err.Organisation = sOrganisation.Trim();
        //        err.Country_Code = sCN.Trim();
        //        err.Application_Number = sApplicationNumber.Trim();
        //        err.Application_Type = sType.Trim();
        //    }
        //    return appkey;
        //}
        /// <summary>
        /// 解析欺诈检查接口output订单APpkey
        /// </summary>
        /// <param name="output"></param>
        /// <param name="appkey"></param>
        /// <returns></returns>
        private string GetAppkeyByInXml(string input, DAL.Error err = null)
        {
            string appkey = string.Empty;
            XmlDocument doc = new XmlDocument();
            string sOrganisation = "";
            string sCN = "";
            string sApplicationNumber = "";
            string sType = "";
            //解析Appkey
            try
            {
                doc.LoadXml(input);
                XmlElement root = doc.DocumentElement;
                sOrganisation = root.SelectSingleNode("/ApplicationSchema/Application/Organisation").InnerText;
                sCN = root.SelectSingleNode("/ApplicationSchema/Application/Country_Code").InnerText;
                sApplicationNumber = root.SelectSingleNode("/ApplicationSchema/Application/Application_Number").InnerText;
                sType = root.SelectSingleNode("/ApplicationSchema/Application/Application_Type").InnerText;
                appkey = sOrganisation.Trim() + sCN.Trim() + sApplicationNumber.Trim() + sType.Trim();
            }
            catch (Exception ex)
            {
                throw new Exception("ERROR WHILE PARSING THE APPKEY VALUE,ERROR INFO:" + ex.ToString());
            }

            if (err != null)
            {
                err.Organisation = sOrganisation.Trim();
                err.Country_Code = sCN.Trim();
                err.Application_Number = sApplicationNumber.Trim();
                err.Application_Type = sType.Trim();
            }
            return appkey;
        }
        /// <summary>
        /// 加分词逻辑获取分词对象
        /// </summary>
        /// <param name="input"></param>
        /// <param name="p"></param>
        /// <param name="err"></param>
        /// <returns></returns>
        private string GetAppKeyAndParticipleByInXml(string input, out ParticipleParameter p, DAL.Error err = null)
        {
            string appkey = string.Empty;
            XmlDocument doc = new XmlDocument();
            string sOrganisation = "";
            string sCN = "";
            string sApplicationNumber = "";
            string sType = "";
            //解析Appkey
            try
            {
                doc.LoadXml(input);
                XmlElement root = doc.DocumentElement;
                sOrganisation = root.SelectSingleNode("/ApplicationSchema/Application/Organisation").InnerText;
                sCN = root.SelectSingleNode("/ApplicationSchema/Application/Country_Code").InnerText;
                sApplicationNumber = root.SelectSingleNode("/ApplicationSchema/Application/Application_Number").InnerText;
                sType = root.SelectSingleNode("/ApplicationSchema/Application/Application_Type").InnerText;
                appkey = sOrganisation.Trim() + sCN.Trim() + sApplicationNumber.Trim() + sType.Trim();
            }
            catch (Exception ax)
            {
                throw new Exception("ERROR WHILE PARSING THE APPKEY VALUE,ERROR INFO:" + ax.ToString());
            }
            //解析分词值
            Util.LogHelper.InfoLog(String.Format("PROCESS TO PARTICIPLE APPKEY={0}--BEGIN", appkey));
            try
            {
                p = this.parse.GetValueFromInput(doc, appkey);
            }
            catch (Exception ex)
            {
                throw new Exception("ERROR WHILE PARSING THE PARTICIPLE VALUE,ERROR INFO:" + ex.ToString());
            }
            finally
            {
                Util.LogHelper.InfoLog(String.Format("PROCESS TO PARTICIPLE APPKEY={0}--END", appkey));
            }

            if (err != null)
            {
                err.Organisation = sOrganisation.Trim();
                err.Country_Code = sCN.Trim();
                err.Application_Number = sApplicationNumber.Trim();
                err.Application_Type = sType.Trim();
            }
            return appkey;
        }
        ///// <summary>
        ///// 文本格式，获取APPKey
        ///// </summary>
        ///// <param name="output"></param>
        ///// <returns></returns>
        //private string getAppkeyByTXT(string input)
        //{
        //    string appkey = string.Empty;
        //    string[] items = input.Split('|');
        //    string sOrganisation = items[0];
        //    string sCN = items[1];
        //    string sApplicationNumber = items[3];
        //    string sType = items[6];
        //    appkey = sOrganisation.Trim() + sCN.Trim() + sApplicationNumber.Trim() + sType.Trim();

        //    return appkey;
        //}

        #region Reference Table 相关
        /// <summary>
        /// 获取引用表表相关信息
        /// </summary>
        /// <param name="tableid"></param>
        /// <returns></returns>
        private DAL.ReferenceTable GetReferenceSetting(int tableid)
        {
            DAL.ReferenceTable refTable = null;
            DataTable dt = DAL.ReferenceTableDAL.GetReferenceTableFieldDetails(tableid);

            if (dt != null && dt.Rows.Count > 0)
            {
                refTable = new DAL.ReferenceTable(tableid);
                foreach (DataRow row in dt.Rows)
                {
                    int fieldid = Convert.ToInt32(row["Field_Id"]);
                    string fieldname = row["Field_Name"].ToString();
                    string description = row["Description"].ToString();
                    string valuetype = row["Value_Type"].ToString();
                    int length = Util.Tool.IsNumberic(row["Length"]) ? Convert.ToInt32(row["Length"]) : 0;
                    string format = Convert.IsDBNull(row["Format"]) ? null : row["Format"].ToString();
                    bool active = Convert.IsDBNull(row["Active"]) ? false : Convert.ToBoolean(row["Active"]);
                    string cal = Convert.IsDBNull(row["Calculations"]) ? null : Convert.ToString(row["Calculations"]);

                    DAL.ReferenceColumn col = new DAL.ReferenceColumn(fieldid, fieldname, description, valuetype, length, format, active, cal);
                    refTable.Columns.Add(fieldname, col);
                }
            }

            return refTable;
        }
        /// <summary>
        /// 切割引用表节点，返回通用Input xml 格式
        /// </summary>
        /// <param name="inputxml">输入参数</param>
        /// <param name="inputwithoutref">不带引用表数据的通用Input xml 格式</param>
        /// <returns>引用表节点</returns>
        private XmlNodeList GetReferenceTableNode(string sAppKey, string inputxml, out string inputwithoutref)
        {
            Util.LogHelper.InfoLog(String.Format("PROCESS TO PARSE THE REFERENCE NODE LIST APPKEY={0}--BEGIN", sAppKey));
            inputwithoutref = string.Empty;

            XmlDocument doc = Util.XMLProcess.XMLLoad(inputxml);
            string path = "/ApplicationSchema/Application/ReferenceTable";
            XmlNodeList nodeList = Util.XMLProcess.ReadAll(path, doc);
            if (nodeList != null && nodeList.Count > 0)
            {
                for (int i = 0; i < nodeList.Count; i++)
                {
                    Util.XMLProcess.Delete(path, doc);
                }
            }
            inputwithoutref = SCM.RabbitMQClient.Common.JsonXmlObjectParser.ConvertXmlToString(doc);
            Util.LogHelper.InfoLog(String.Format("PROCESS TO PARSE THE REFERENCE NODE LIST APPKEY={0}--END", sAppKey));
            return nodeList;
        }
        /// <summary>
        /// 验证引用表各输入字段数据&格式化
        /// </summary>
        /// <param name="referenceNode"></param>
        /// <param name="tableid"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        private bool ValidateAndFormatReferenceTableNode(ref XmlNode referenceNode, DAL.ReferenceTable refTbAtt, ref StringBuilder errMsg)
        {
            bool bRet = true;

            XmlNodeList fieldNodes = referenceNode.ChildNodes;
            if (fieldNodes.Count + 1 == refTbAtt.Columns.Count)
            {
                foreach (XmlNode fieldNode in fieldNodes)
                {
                    string fieldname = fieldNode.Name;
                    string fieldvalue = fieldNode.InnerText.Trim();
                    DAL.ReferenceColumn col = refTbAtt.Columns[fieldname];
                    if (col.Value_Type.ToUpper().Trim() == "DATETIME")
                    {
                        if (Util.Tool.IsDate(fieldvalue))
                        {
                            fieldNode.InnerText = Convert.ToDateTime(fieldvalue).ToString("yyyy-MM-dd HH:mm:ss");
                        }
                        else
                        {
                            bRet = false;
                            errMsg.Append(string.Format("THE FIELD[{0}] OF TABLE ID[{1}] 'S VALUE IS NOT DATETIME.\n", fieldname, refTbAtt.Table_Id));

                        }
                    }

                    switch (col.Format.ToUpper().Trim())
                    {
                        case "PHONE NUMBER":
                            fieldNode.InnerText = Util.Tool.Leave0to9(fieldvalue);
                            break;
                        case "ADDRESS":
                            fieldNode.InnerText = Util.Tool.RemoveSingleQuoteCommaDot(fieldvalue);
                            break;
                        case "ID NUMBER":
                            fieldNode.InnerText = Util.Tool.LeaveAtoZand0to9(fieldvalue);
                            break;
                        case "COMPANY SUFFIX":
                            fieldvalue = fieldvalue.Replace('\'', '\0');
                            fieldvalue = " " + fieldvalue + " ";
                            for (int i = 0; i < Util.GlobalVariable.CompanySuffix.Count; i++)
                            {
                                string tempCompanySuffix = Util.GlobalVariable.CompanySuffix[i].ToString();
                                fieldvalue = fieldvalue.Replace(tempCompanySuffix, "");
                            }
                            fieldNode.InnerText = fieldvalue.Trim();
                            break;
                    }
                }
                if (errMsg.Length > 0)
                {
                    bRet = false;
                }
            }
            else
            {
                bRet = false;
                errMsg.Append(string.Format("TABLE ID[{0}] IS NOT MATCHED WITH THE INPUT.\n", refTbAtt.Table_Id));
            }

            return bRet;
        }
        /// <summary>
        /// 获取引用表配置
        /// </summary>
        /// <param name="RID"></param>
        /// <returns></returns>
        private DAL.ReferenceTable GetReferenceSettingObj(int RID)
        {
            // Syn operation.
            lock (SynObject)
            {
                if (ReferenceHistory.ContainsKey(RID))
                {
                    return ReferenceHistory[RID];
                }
                else
                {
                    DAL.ReferenceTable att = GetReferenceSetting(RID);
                    if (att != null)
                        ReferenceHistory.Add(RID, att);
                    return att;
                }
            }
        }
        /// <summary>
        /// 处理引用表节点总入口
        /// </summary>
        /// <param name="inputxml"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        private bool ProcessXMLReferenceTableList(string Appkey, XmlNodeList referenceNodes, ref StringBuilder errMsg, DAL.Error err = null)
        {
            Util.LogHelper.InfoLog(String.Format("PROCESS TO SAVE THE REFERENCE NODE LIST APPKEY={0}--BEGIN", Appkey));
            bool bRet = true;

            //清理Appkey相关引用表
            try
            {
                clearReferenceData(Appkey);
            }
            catch (Exception ex)
            {
                bRet = false;
                //错误代码106 加载数据出错
                if (err != null)
                {
                    err.Error_Code = "106";
                }
                errMsg.Append(string.Format("FAIL TO CLEAR REFERENCE DATA[APPKEY={0}]，ERROR MESSAGE：{1}", Appkey, ex.Message));

            }

            DataSet referenceDSList = null;
            //验证Reference Nodes
            bRet = ValidateReferenceTable(Appkey, referenceNodes, ref errMsg, out referenceDSList, ref err);

            //更新数据库
            if (bRet && referenceDSList != null && referenceDSList.Tables.Count > 0)
            {
                try
                {
                    //BulkSaveReferenceData(Appkey, referenceDSList);
                    BulkInsertReferenceData(Appkey, referenceDSList);
                }
                catch (Exception ex)
                {
                    bRet = false;
                    //错误代码106 加载数据出错
                    if (err != null)
                    {
                        err.Error_Code = "106";
                    }
                    errMsg.Append(string.Format("FAIL TO LOAD REFERENCE DATA[APPKEY={0}]，ERROR MESSAGE：{1}", Appkey, ex.Message));
                }
            }            

            Util.LogHelper.InfoLog(String.Format("PROCESS TO SAVE THE REFERENCE NODE LIST APPKEY={0}--END", Appkey));
            return bRet;
        }
        /// <summary>
        /// 获取带有APPKEY 列的Datatable
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="RID"></param>
        /// <param name="tabatt"></param>
        /// <returns></returns>
        private DataTable GetDTByRID(ref DataSet ds, int RID, DAL.ReferenceTable tabatt)
        {
            DataTable dt = null;
            string tabname = "R" + RID.ToString("00");
            if (ds.Tables.Contains(tabname))
            {
                dt = ds.Tables[tabname];
            }
            else
            {
                dt = new DataTable(tabname);
                foreach (string key in tabatt.Columns.Keys)
                {
                    dt.Columns.Add(key);
                }
                ds.Tables.Add(dt);
            }
            return dt;
        }

        /// <summary>
        /// 组合数据
        /// </summary>
        /// <param name="Appkey"></param>
        /// <param name="referenceNode"></param>
        /// <param name="dt"></param>
        private void AddDataToTable(string Appkey, XmlNode referenceNode, ref DataTable dt)
        {
            DataRow row = dt.NewRow();
            row[APPKEY] = Appkey;
            XmlNodeList fieldNodes = referenceNode.ChildNodes;
            foreach (XmlNode fieldNode in fieldNodes)
            {
                string fieldname = fieldNode.Name;
                string value = fieldNode.InnerText.Trim();
                row[fieldname] = value;
            }
            dt.Rows.Add(row);
        }

        /// <summary>
        /// 批量导入数据库
        /// </summary>
        /// <param name="AppKey"></param>
        /// <param name="ds"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        private ArrayList BulkSaveReferenceData(string AppKey, DataSet ds)
        {
            ArrayList tableids = new ArrayList();
            try
            {
                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataTable dt in ds.Tables)
                    {
                        string tablename = dt.TableName;
                        int tableID = Convert.ToInt32(tablename.Substring(1));
                        string stagingname = GetTableStagingName(tableID);
                        string dbtablename = GetDbTableName(tableID);

                        if (Util.GlobalVariable.BReferenceSynonyms)
                        {
                            SetSynonymsNames(tableID, ref stagingname, ref dbtablename);
                        }

                        Util.SqlHelper.BulkInsert(Util.GlobalVariable.CnnString, dt, stagingname);

                        DAL.ReferenceTable attdt = this.ReferenceHistory[tableID];

                        string fields = GetFieldsFromTableAttributes(attdt);

                        DAL.ReferenceTableDAL.CopyDataFromStagingTable(stagingname, dbtablename, fields, AppKey);
                        //由于数据读写频繁，下面操作会死锁表
                        //DAL.ReferenceTableDAL.ApplyCalculations(tableID, transaction);

                        //获取引用表id
                        if (!tableids.Contains(tableID.ToString()))
                        {
                            tableids.Add(tableID.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return tableids;
        }

        /// <summary>
        /// 批量导入数据库
        /// </summary>
        /// <param name="AppKey"></param>
        /// <param name="ds"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        private void BulkInsertReferenceData(string AppKey, DataSet ds)
        {            
            try
            {
                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataTable dt in ds.Tables)
                    {
                        string tablename = dt.TableName;
                        int tableID = Convert.ToInt32(tablename.Substring(1));
                        string stagingname = GetTableStagingName(tableID);
                        string dbtablename = GetDbTableName(tableID);

                        if (Util.GlobalVariable.BReferenceSynonyms)
                        {
                            SetSynonymsNames(tableID, ref stagingname, ref dbtablename);
                        }

                        DAL.ReferenceTable attdt = this.ReferenceHistory[tableID];
                       
                        ArrayList field_list = GetFieldsArrayFromTableAttributes(attdt);

                        DAL.ReferenceTableDAL.InsertReferenceData(dt, dbtablename, field_list, AppKey);
                        
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }           
        }

        private bool ValidateReferenceTable(string Appkey ,XmlNodeList referenceNodes, ref StringBuilder errMsg,out DataSet referenceDSList,ref DAL.Error err)
        {
            bool bRet = true;

            referenceDSList = new DataSet();           

            //如果引用表节点为空，不处理，直接返回成功。
            if (referenceNodes != null && referenceNodes.Count > 0)
            {
                //收集数据，放入Dataset [referenceDSList]中
                for (int i = 0; i < referenceNodes.Count; i++)
                {
                    XmlNode refItem = referenceNodes[i];

                    int tableid = -1;
                    if (refItem.Attributes["TableID"] == null)
                    {
                        //错误代码101
                        if (err != null)
                        {
                            err.Error_Code = "101";
                        }
                        errMsg.Append("A REFERENCE XML NODE HAS NO TABLE ID. \n");
                        bRet = false;
                        break;
                    }
                    if (refItem.Attributes["TableID"].Value == "" || !Util.Tool.IsNumberic(refItem.Attributes["TableID"].Value))
                    {
                        //错误代码102
                        if (err != null)
                        {
                            err.Error_Code = "102";
                        }
                        errMsg.Append("A REFERENCE XML NODE 's TABLE ID IS NOT NUMERIC. \n");
                        bRet = false;
                        break;
                    }

                    tableid = Convert.ToInt32(refItem.Attributes["TableID"].Value);

                    DAL.ReferenceTable refAtt = GetReferenceSettingObj(tableid);

                    if (refAtt == null)
                    {
                        //错误代码103
                        if (err != null)
                        {
                            err.Error_Code = "103";
                        }
                        errMsg.Append(string.Format("THE REFERENCE TABLE[{0}] ATTRIBUTES IS NOT FOUND.\n", tableid));
                        bRet = false;
                        break;
                    }
                    if (!refAtt.Columns.ContainsKey(APPKEY))
                    {
                        //错误代码104
                        if (err != null)
                        {
                            err.Error_Code = "104";
                        }
                        errMsg.Append(string.Format("THE REFERENCE TABLE[{0}] ATTRIBUTES HAS NOT APPKEY COLUMN.\n", tableid));
                        bRet = false;
                        break;
                    }

                    bRet = ValidateAndFormatReferenceTableNode(ref refItem, refAtt, ref errMsg);

                    if (!bRet)
                    {
                        //错误代码105 验证数据出错
                        if (err != null)
                        {
                            err.Error_Code = "105";
                        }
                        break;
                    }

                    DataTable refTable = GetDTByRID(ref referenceDSList, tableid, refAtt);
                    AddDataToTable(Appkey, refItem, ref refTable);
                }                
            }
            return bRet;
        }
        /// <summary>
        /// 获取临时引用表名
        /// </summary>
        /// <param name="RID"></param>
        /// <returns></returns>
        private string GetTableStagingName(int RID)
        {
            return "R_" + RID.ToString("00") + "_B";
        }
        /// <summary>
        /// 获取引用表名
        /// </summary>
        /// <param name="RID"></param>
        /// <returns></returns>
        private string GetDbTableName(int RID)
        {
            return "R_" + RID.ToString("00") + "_A";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="RID"></param>
        /// <param name="stagingTab"></param>
        /// <param name="dbTab"></param>
        private void SetSynonymsNames(int RID, ref string stagingTab, ref string dbTab)
        {
            string tabname = "R_" + RID.ToString("00");

            DataTable dt = DAL.ReferenceTableDAL.GetSynonymsNames(tabname);
            if (dt != null && dt.Rows.Count == 2)
            {
                foreach (DataRow row in dt.Rows)
                {
                    string sname = row["NAME"].ToString();
                    string sbase_object_name = row["BASE_OBJECT_NAME"].ToString();
                    if (sname.Trim().ToUpper().EndsWith("STAGING"))
                    {
                        stagingTab = sbase_object_name;
                    }
                    else
                    {
                        dbTab = sbase_object_name;
                    }
                }
            }
        }
        /// <summary>
        /// 获取插入字段
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private string GetFieldsFromTableAttributes(DAL.ReferenceTable tab)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string key in tab.Columns.Keys)
            {
                sb.Append("[");
                sb.Append(key);
                sb.Append("]");
                sb.Append(",");
            }
            return sb.ToString().TrimEnd(',');
        }
        /// <summary>
        /// 获取插入字段
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private ArrayList GetFieldsArrayFromTableAttributes(DAL.ReferenceTable tab)
        {
            ArrayList al = new ArrayList();
            foreach (string key in tab.Columns.Keys)
            {
                al.Add(key);                
            }
            return al;
        }
        /// <summary>
        /// 获取无更新引用表IDs
        /// </summary>
        /// <param name="refTabIds"></param>
        /// <returns></returns>
        private ArrayList getReferenceIDsForClear(ArrayList refTabIds)
        {
            ArrayList referIdsCopy = null;
            if (Util.GlobalVariable.ReferenceTables != null && Util.GlobalVariable.ReferenceTables.Count > 0)
            {
                referIdsCopy = (ArrayList)Util.GlobalVariable.ReferenceTables.Clone();
                if (refTabIds != null && refTabIds.Count > 0)
                {
                    foreach (string id in refTabIds)
                    {
                        if (referIdsCopy.Contains(id))
                        {
                            referIdsCopy.Remove(id);
                        }
                    }
                }
            }
            return referIdsCopy;
        }
        /// <summary>
        /// 清理Appkey 相关引用表数据
        /// </summary>
        /// <param name="referIdsCopy"></param>
        /// <param name="appkey"></param>
        private void clearReferenceData(ArrayList referIdsCopy, string appkey)
        {
            ArrayList referTableList = new ArrayList();
            if (referIdsCopy != null && referIdsCopy.Count > 0)
            {
                foreach (string id in referIdsCopy)
                {
                    int tableID = Convert.ToInt32(id);
                    string stagingname = GetTableStagingName(tableID);
                    string dbtablename = GetDbTableName(tableID);

                    if (Util.GlobalVariable.BReferenceSynonyms)
                    {
                        SetSynonymsNames(tableID, ref stagingname, ref dbtablename);
                    }
                    referTableList.Add(dbtablename);
                }

                if (referTableList != null && referTableList.Count > 0)
                {
                    DAL.ReferenceTableDAL.DeleteReferenceData(referTableList, appkey);
                }
            }
        }
        /// <summary>
        /// 清理Appkey 相关引用表数据
        /// </summary>
        /// <param name="referIdsCopy"></param>
        /// <param name="appkey"></param>
        private void clearReferenceData(string appkey)
        {
            ArrayList referTableList = new ArrayList();
            if (Util.GlobalVariable.ReferenceTables != null && Util.GlobalVariable.ReferenceTables.Count > 0)
            {
                foreach (string id in Util.GlobalVariable.ReferenceTables)
                {
                    int tableID = Convert.ToInt32(id);
                    string stagingname = GetTableStagingName(tableID);
                    string dbtablename = GetDbTableName(tableID);

                    if (Util.GlobalVariable.BReferenceSynonyms)
                    {
                        SetSynonymsNames(tableID, ref stagingname, ref dbtablename);
                    }
                    referTableList.Add(dbtablename);
                }

                if (referTableList != null && referTableList.Count > 0)
                {
                    DAL.ReferenceTableDAL.DeleteReferenceData(referTableList, appkey);
                }
            }
        }
        #endregion

        #region Participle 相关
        /// <summary>
        /// 处理分词
        /// </summary>
        /// <param name="Appkey"></param>
        /// <param name="pParticiple"></param>
        /// <param name="errMsg"></param>
        /// <param name="err"></param>
        /// <returns></returns>
        private bool ProcessParticiple(string Appkey, ParticipleParameter pParticiple, ref StringBuilder errMsg, DAL.Error err = null)
        {
            bool bRet = true;
            if (Util.GlobalVariable.BParticiple)
            {
                Util.LogHelper.InfoLog(String.Format("PROCESS TO SAVE THE PARTICIPIAL DATA APPKEY={0}--BEGIN", Appkey));
                try
                {
                    string sxml = pParticiple.ToXML();
                    DAL.ParticipleDAl.ExecuteParticiple(Appkey, sxml, "A");
                }
                catch (Exception ex)
                {
                    bRet = false;
                    //错误代码200 处理分词逻辑出错
                    if (err != null)
                    {
                        err.Error_Code = "200";
                        err.Remark = "THE SEGMENTATION  DATA OF THE APPLICATION\"{0}\" FAIL TO SAVE.";
                    }
                    errMsg.Append(string.Format("FAIL TO PROCESS PARTICIPLE ENTITY DATA[APPKEY={0}],ERROR MESSAGE：{1}", Appkey, ex.Message));
                }
                finally
                {
                    Util.LogHelper.InfoLog(String.Format("PROCESS TO SAVE THE PARTICIPIAL DATA APPKEY={0}--END", Appkey));
                }
            }
            return bRet;
        }
        #endregion

        #endregion
    }
}
