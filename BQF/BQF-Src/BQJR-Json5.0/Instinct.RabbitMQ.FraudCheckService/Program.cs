using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;


namespace Instinct.RabbitMQ.FraudCheckService
{
    /// <summary>
    /// 主程序入口类
    /// </summary>
    class Program
    {
        /// <summary>
        /// 主程序入口
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Console.WriteLine("START TO DO FRAUD CHECKING...");
            try
            {
                SetConfigValues(); 
                BLL.InstinctBusiness AWorker = new BLL.InstinctBusiness(GetRabbitMQSender, GetRabbitMQReceiver, Util.GlobalVariable.MqSendExchange, Util.GlobalVariable.MqSendQueueName, Util.GlobalVariable.MqReceiveProcessCount, Util.GlobalVariable.Config);
                AWorker.Work();                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Util.LogHelper.ErrorLog("Main", ex);
            }
            Console.ReadLine();
        }
        /// <summary>
        /// 获取配置信息
        /// </summary>
        private static void SetConfigValues()
        {
            if (Util.GlobalVariable.Config == null)
                Util.GlobalVariable.Config = new ClsOnlineServiceCall.SetINIValue();
            Util.GlobalVariable.Config.AppActionOutputFileFlag = System.Configuration.ConfigurationManager.AppSettings["AppActionOutputFileFlag"];
            Util.GlobalVariable.Config.AppInputFormat = System.Configuration.ConfigurationManager.AppSettings["AppInputFormat"];
            Util.GlobalVariable.Config.AppLocalTimeDifference = System.Configuration.ConfigurationManager.AppSettings["AppLocalTimeDifference"];
            Util.GlobalVariable.Config.AppOrganisation = System.Configuration.ConfigurationManager.AppSettings["AppOrganisation"];
            Util.GlobalVariable.Config.AppOutputFormat = System.Configuration.ConfigurationManager.AppSettings["AppOutputFormat"];
            Util.GlobalVariable.Config.AppOutputDirectory = System.Configuration.ConfigurationManager.AppSettings["AppOutputDirectory"];
            Util.GlobalVariable.Config.UseWindowsAuthentication = System.Configuration.ConfigurationManager.AppSettings["UseWindowsAuthentication"];
            Util.GlobalVariable.Config.UseDefinedEncryptionKey = System.Configuration.ConfigurationManager.AppSettings["UseDefinedEncryptionKey"];
            Util.GlobalVariable.Config.Key1Path = System.Configuration.ConfigurationManager.AppSettings["Key1Path"];
            Util.GlobalVariable.Config.Key2Path = System.Configuration.ConfigurationManager.AppSettings["Key2Path"];
            Util.GlobalVariable.Config.DatabaseUserId = System.Configuration.ConfigurationManager.AppSettings["DatabaseUserId"];
            Util.GlobalVariable.Config.DatabasePassword = System.Configuration.ConfigurationManager.AppSettings["DatabasePassword"];
            Util.GlobalVariable.Config.DataSource = System.Configuration.ConfigurationManager.AppSettings["DataSource"];
            Util.GlobalVariable.Config.DefaultCountry = System.Configuration.ConfigurationManager.AppSettings["DefaultCountry"];
            Util.GlobalVariable.Config.DelimiterCharacters = System.Configuration.ConfigurationManager.AppSettings["DelimiterCharacters"];
            Util.GlobalVariable.Config.InitialCatalog = System.Configuration.ConfigurationManager.AppSettings["InitialCatalog"];
            Util.GlobalVariable.Config.RulesInOutputFile = System.Configuration.ConfigurationManager.AppSettings["RulesInOutputFile"];
            Util.GlobalVariable.Config.RulesDescriptionInOutputFile = System.Configuration.ConfigurationManager.AppSettings["RulesDescriptionInOutputFile"];
            Util.GlobalVariable.Config.ActionCountNumberInOutputFile = System.Configuration.ConfigurationManager.AppSettings["ActionCountNbrInOutputFile"];
            Util.GlobalVariable.Config.NatureOfFraudInOutputFile = System.Configuration.ConfigurationManager.AppSettings["NatureOfFraudInOutputFile"];
            Util.GlobalVariable.Config.DiaryInOutputFile = System.Configuration.ConfigurationManager.AppSettings["DiaryInOutputFile"];
            Util.GlobalVariable.Config.SiteWithSpecialFunctions = System.Configuration.ConfigurationManager.AppSettings["SiteWithSpecialFunctions"];
            Util.GlobalVariable.Config.SecondaryServicePrefix = System.Configuration.ConfigurationManager.AppSettings["SecondServiceSuffix"];
            Util.GlobalVariable.Config.UserIdInOutputFile = System.Configuration.ConfigurationManager.AppSettings["UserIdInOutputFile"];
            Util.GlobalVariable.Config.WriteLogFile = System.Configuration.ConfigurationManager.AppSettings["WriteLogFile"];
            Util.GlobalVariable.Config.DecisionReasonInOutputFile = System.Configuration.ConfigurationManager.AppSettings["DecisionReasonInOutputFile"];
            Util.GlobalVariable.Config.UserDefinedAlertInOutputFile = System.Configuration.ConfigurationManager.AppSettings["UserDefinedAlertInOutputFile"];
            Util.GlobalVariable.Config.GroupMemberCode = System.Configuration.ConfigurationManager.AppSettings["GroupMemberCode"];
            Util.GlobalVariable.Config.LowFraudScore = System.Configuration.ConfigurationManager.AppSettings["LowFraudScore"];
            Util.GlobalVariable.Config.NewApplicationsAge = System.Configuration.ConfigurationManager.AppSettings["NewApplicationsAge"];
            Util.GlobalVariable.Config.FraudAlertUserId = System.Configuration.ConfigurationManager.AppSettings["FraudAlertUserIdInOutputFile"];
            //Util.GlobalVariable.Config.MaxPoolSize = System.Configuration.ConfigurationManager.AppSettings["MaxPoolSize"];
            //Util.GlobalVariable.Config.ApplicationName = System.Configuration.ConfigurationManager.AppSettings["ApplicationName"];
            //Set MQ Config Value
            Util.GlobalVariable.MqSendHostName = System.Configuration.ConfigurationManager.AppSettings["MqSendHostName"];
            Util.GlobalVariable.MqSendVHostName = System.Configuration.ConfigurationManager.AppSettings["MqSendVHostName"];
            Util.GlobalVariable.MqSendUserName = System.Configuration.ConfigurationManager.AppSettings["MqSendUserName"];
            Util.GlobalVariable.MqSendPassword = System.Configuration.ConfigurationManager.AppSettings["MqSendPassword"];
            Util.GlobalVariable.MqSendQueueName = System.Configuration.ConfigurationManager.AppSettings["MqSendQueueName"];
            Util.GlobalVariable.MqSendExchange = System.Configuration.ConfigurationManager.AppSettings["MqSendExchange"];
            Util.GlobalVariable.MqSendPort = Util.Tool.IsNumberic(System.Configuration.ConfigurationManager.AppSettings["MqSendPort"]) ? Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MqSendPort"]) : 0;
            Util.GlobalVariable.MqReceiveHostName = System.Configuration.ConfigurationManager.AppSettings["MqReceiveHostName"];
            Util.GlobalVariable.MqReceiveVHostName = System.Configuration.ConfigurationManager.AppSettings["MqReceiveVHostName"];
            Util.GlobalVariable.MqReceiveUserName = System.Configuration.ConfigurationManager.AppSettings["MqReceiveUserName"];
            Util.GlobalVariable.MqReceivePassword = System.Configuration.ConfigurationManager.AppSettings["MqReceivePassword"];
            Util.GlobalVariable.MqListenQueueName = System.Configuration.ConfigurationManager.AppSettings["MqListenQueueName"];
            Util.GlobalVariable.MqReceivePort = Util.Tool.IsNumberic(System.Configuration.ConfigurationManager.AppSettings["MqReceivePort"]) ? Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MqReceivePort"]) : 0;
            Util.GlobalVariable.MqReceiveProcessCount = Util.Tool.IsNumberic(System.Configuration.ConfigurationManager.AppSettings["MqReceiveProcessCount"]) ? Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MqReceiveProcessCount"]) : 0;
            string EncodeName = System.Configuration.ConfigurationManager.AppSettings["MqEncode"];
            Util.GlobalVariable.MqEncoding  = EncodeName;
            Util.GlobalVariable.MqNeedDeclareQueue = System.Configuration.ConfigurationManager.AppSettings["MqNeedDeclareQueue"] != null && System.Configuration.ConfigurationManager.AppSettings["MqNeedDeclareQueue"].ToString().ToUpper()=="TRUE"?true :false ;
            Util.GlobalVariable.MqSynchronization = System.Configuration.ConfigurationManager.AppSettings["MqSynchronization"] != null && System.Configuration.ConfigurationManager.AppSettings["MqSynchronization"].ToString().ToUpper() == "TRUE" ? true : false;
            Util.GlobalVariable.BReferenceSynonyms = System.Configuration.ConfigurationManager.AppSettings["ReferenceSynonyms"] != null && System.Configuration.ConfigurationManager.AppSettings["ReferenceSynonyms"].ToString().ToUpper() == "TRUE" ? true : false;

            string sReferenceTables= System.Configuration.ConfigurationManager.AppSettings["ReferenceTables"]==null?"": System.Configuration.ConfigurationManager.AppSettings["ReferenceTables"];
            if (sReferenceTables != "")
            {
                string[] arrayTables = sReferenceTables.Split(Util.GlobalVariable.Config.DelimiterCharacters.Trim().ToCharArray()[0]);
                Util.GlobalVariable.ReferenceTables = new System.Collections.ArrayList();
                Util.GlobalVariable.ReferenceTables.AddRange(arrayTables);
            }
            Util.GlobalVariable.BParticiple= System.Configuration.ConfigurationManager.AppSettings["Participle"] != null && Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["Participle"])=="Y"?true:false;

            Util.GlobalVariable.WaitingSeconds = System.Configuration.ConfigurationManager.AppSettings["WaitingSeconds"] != null && Util.Tool.IsNumberic(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["WaitingSeconds"])) ? Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["WaitingSeconds"]) : 2;

            Util.Tool.ConnectDatabase();

            DataTable  DTCompanySuffix = DAL.ReferenceTableDAL.GetCompanySuffix();
            if (Util.GlobalVariable.CompanySuffix == null)
            {
                Util.GlobalVariable.CompanySuffix = new System.Collections.ArrayList();
            }
            foreach (DataRow drCompanySuffix in DTCompanySuffix.Rows)
            {
                if (!Convert.IsDBNull(drCompanySuffix["Company Suffix"]))
                {
                    Util.GlobalVariable.CompanySuffix.Add(drCompanySuffix["Company Suffix"]);
                }
            }
        }
        /// <summary>
        /// 初始化发送MQ者
        /// </summary>
        /// <returns></returns>
        private static SCM.RabbitMQClient.RabbitMqClientJSONSimple GetRabbitMQSender()
        {
            SCM.RabbitMQClient.RabbitMqClientJSONSimple client = new SCM.RabbitMQClient.RabbitMqClientJSONSimple(Util.GlobalVariable.MqSendHostName, Util.GlobalVariable.MqSendVHostName, Util.GlobalVariable.MqSendQueueName, Util.GlobalVariable.MqSendPort, Util.GlobalVariable.MqSendUserName, Util.GlobalVariable.MqSendPassword, Util.GlobalVariable.MqEncoding,Util.GlobalVariable.MqNeedDeclareQueue,Util.GlobalVariable.MqSynchronization  );
            client.ThreadExceptionEvents += new SCM.RabbitMQClient.RabbitMqClientFactory.ThreadExceptionEventHandler(OnThreadException);
            return client;
        }
        /// <summary>
        /// 初始化接收MQ者
        /// </summary>
        /// <returns></returns>
        private static SCM.RabbitMQClient.RabbitMqClientJSONSimple GetRabbitMQReceiver()
        {
            SCM.RabbitMQClient.RabbitMqClientJSONSimple client = new SCM.RabbitMQClient.RabbitMqClientJSONSimple(Util.GlobalVariable.MqReceiveHostName, Util.GlobalVariable.MqReceiveVHostName, Util.GlobalVariable.MqListenQueueName, Util.GlobalVariable.MqReceivePort, Util.GlobalVariable.MqReceiveUserName, Util.GlobalVariable.MqReceivePassword, Util.GlobalVariable.MqEncoding, Util.GlobalVariable.MqNeedDeclareQueue, Util.GlobalVariable.MqSynchronization);
            client.ThreadExceptionEvents += new SCM.RabbitMQClient.RabbitMqClientFactory.ThreadExceptionEventHandler(OnThreadException);
            return client;
        }
        /// <summary>
        /// 子线程抛出异常，处理函数
        /// </summary>
        /// <param name="ex"></param>
        private static void OnThreadException(Exception ex)
        {
            Util.LogHelper.ErrorLog("Abnormal program termination,message info:" + ex.Message);
            Util.Tool.ReturnMainCode(3);
        }
    }
}
