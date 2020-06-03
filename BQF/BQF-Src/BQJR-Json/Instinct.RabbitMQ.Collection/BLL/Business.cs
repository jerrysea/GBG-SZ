using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using SCM.RabbitMQClient;

namespace Instinct.RabbitMQ.Collection.BLL
{
    public class Business
    {
        #region 成员
        private Instinct.Collection.Lib.DAL.InstinctDAL dal;

        private List<SCM.RabbitMQClient.IRabbitMqClient> receiverObjList;

        public int ProcessCount { get; set; }

        public delegate SCM.RabbitMQClient.IRabbitMqClient BuildClient();
        public BuildClient ReceiverClient { get; set; }
        #endregion

        #region 构造
        public Business(BuildClient receiver,ArrayList subtables, int processCount)
        {
            dal = new Instinct.Collection.Lib.DAL.InstinctDAL(Instinct.Collection.Lib.Util.GlobalVariables.CnnString,subtables ,Util.LogHelper.ErrorLog,Util.LogHelper.InfoLog);

            this.ReceiverClient = receiver;
            this.ProcessCount = processCount;
            this.receiverObjList = new List<IRabbitMqClient>();

            if (this.ProcessCount > 0)
            {
                for (int i = 0; i < this.ProcessCount; i++)
                {
                    SCM.RabbitMQClient.IRabbitMqClient rclient = ReceiverClient();
                    this.receiverObjList.Add(rclient);
                }
            }

            SCM.RabbitMQClient.Common.LogLocation.Log = new Util.LogHelper();
            
        }
        #endregion
        #region 公有函数
        /// <summary>
        /// 侦听MQ，更新最终结果
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
        /// <summary>
        /// 侦听事件启动
        /// </summary>
        /// <param name="receiver"></param>
        private void Listening(SCM.RabbitMQClient.IRabbitMqClient receiver)
        {
            receiver.ActionEventMessage += mqClient_ActionEventMessage;
            receiver.OnListening();
        }
        /// <summary>
        /// 侦听响应函数
        /// </summary>
        /// <param name="result"></param>
        private void mqClient_ActionEventMessage(EventMessageResult result)
        {
            Util.LogHelper.InfoLog("START TO UPDATE THE AUDIT RESULT...");

            string sInput = string.Empty;
            string sMarkCode = string.Empty;

            Encoding encoder = Instinct.Collection.Lib.Util.Tool.GetEncoding(Util.GlobalVariables.MqEncoding);

            try
            {
                sInput =MessageSerializerFactory.CreateMessageJsonSerializerInstance(Util.GlobalVariables.MqEncoding)
                               .Deserialize(result.MessageBytes);
                Util.LogHelper.InfoLog(String.Format("INPUT={0}", sInput));

                sMarkCode = result.EventMessageBytes.EventMessageMarkcode;
                Util.LogHelper.InfoLog(String.Format("EventMessageMarkcode={0}", sMarkCode));

                switch (result.EventMessageBytes.ResponseMethod.Trim().ToUpper())
                {
                    case MessageTypeConst.InstinctAuditXMLResult:
                        Util.LogHelper.InfoLog("CALLING METHOD[" + MessageTypeConst.InstinctAuditXMLResult + "]...");

                        if (dal.ProcessInputXml(sInput))
                        {
                            Util.LogHelper.InfoLog("SUCCEED TO PROCESS.");
                        }
                        else
                        {
                            Util.LogHelper.InfoLog("FAIL TO PROCESS.");
                        }
                        break;
                    case MessageTypeConst.InstinctAuditJSONResult:
                        Util.LogHelper.InfoLog("CALLING METHOD[" + MessageTypeConst.InstinctAuditJSONResult + "]...");
                        sInput = SCM.RabbitMQClient.Common.JsonXmlObjectParser.JsonToXml(sInput);
                        
                        if (dal.ProcessInputXml(sInput))
                        {
                            Util.LogHelper.InfoLog("SUCCEED TO PROCESS.");
                        }
                        else
                        {
                            Util.LogHelper.InfoLog("FAIL TO PROCESS.");
                        }                       
                        break;
                    default:
                        Util.LogHelper.InfoLog(String.Format("THERE IS NO MATCHING FUNCTION.", sInput));
                        break;
                }

            }
            catch (Exception ex)
            {
                Util.LogHelper.ErrorLog("FAIL TO UPDATE AUDIT RESULT", ex);                
            }
            finally
            {
                result.IsOperationOk = true; //告诉消息队列接收成功。 
            }
            Util.LogHelper.InfoLog("END TO UPDATE THE AUDIT RESULT...");

        }
        #endregion
    }
}
