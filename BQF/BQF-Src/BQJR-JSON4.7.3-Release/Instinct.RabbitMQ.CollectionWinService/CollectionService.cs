using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;


namespace Instinct.RabbitMQ.CollectionWinService
{
    public partial class CollectionService : ServiceBase
    {
        public CollectionService()
        {
            InitializeComponent();
            SetConfigValues();           
        }

        protected override void OnStart(string[] args)
        {
            Util.LogHelper.InfoLog("BEGIN TO DO Event\"OnStart\"");
            Util.LogHelper.InfoLog("START TO PUSH THE AUDIT RESULTS...");
            try
            {                
				BLL.Business AWorker = new BLL.Business(GetRabbitMQReceiver,Util.GlobalVariables.SubTables ,  Util.GlobalVariables.MqReceiveProcessCount);
                AWorker.Work();
            }
            catch (Exception ex)
            {                
                Util.LogHelper.ErrorLog("Main", ex);
            }
            Console.ReadLine();
            Util.LogHelper.InfoLog("END TO DO Event\"OnStart\"");
        }

        protected override void OnStop()
        {
            Util.LogHelper.InfoLog("BEGIN TO DO Event\"OnStop\"");
            Util.LogHelper.InfoLog("......");
            Util.LogHelper.InfoLog("END TO DO Event\"OnStop\"");
        }

        /// <summary>
        /// 获取配置信息
        /// </summary>
        private static void SetConfigValues()
        {

            Instinct.Collection.Lib.Util.GlobalVariables.UseWindowsAuthentication = System.Configuration.ConfigurationManager.AppSettings["UseWindowsAuthentication"];
            Instinct.Collection.Lib.Util.GlobalVariables.DatabaseUserId = System.Configuration.ConfigurationManager.AppSettings["DatabaseUserId"];
            Instinct.Collection.Lib.Util.GlobalVariables.DatabasePassword = System.Configuration.ConfigurationManager.AppSettings["DatabasePassword"];
            Instinct.Collection.Lib.Util.GlobalVariables.DataSource = System.Configuration.ConfigurationManager.AppSettings["DataSource"];
            Instinct.Collection.Lib.Util.GlobalVariables.InitialCatalog = System.Configuration.ConfigurationManager.AppSettings["InitialCatalog"];
            Instinct.Collection.Lib.Util.GlobalVariables.MaxPoolSize = System.Configuration.ConfigurationManager.AppSettings["MaxPoolSize"];
            Instinct.Collection.Lib.Util.GlobalVariables.ApplicationName = System.Configuration.ConfigurationManager.AppSettings["ApplicationName"];
            //Set MQ Config Value
            Util.GlobalVariables.MqSendHostName = System.Configuration.ConfigurationManager.AppSettings["MqSendHostName"];
            Util.GlobalVariables.MqSendVHostName = System.Configuration.ConfigurationManager.AppSettings["MqSendVHostName"];
            Util.GlobalVariables.MqSendUserName = System.Configuration.ConfigurationManager.AppSettings["MqSendUserName"];
            Util.GlobalVariables.MqSendPassword = System.Configuration.ConfigurationManager.AppSettings["MqSendPassword"];
            Util.GlobalVariables.MqSendQueueName = System.Configuration.ConfigurationManager.AppSettings["MqSendQueueName"];
            Util.GlobalVariables.MqSendExchange = System.Configuration.ConfigurationManager.AppSettings["MqSendExchange"];
            Util.GlobalVariables.MqSendPort = Instinct.Collection.Lib.Util.Tool.IsNumberic(System.Configuration.ConfigurationManager.AppSettings["MqSendPort"]) ? Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MqSendPort"]) : 0;
            Util.GlobalVariables.MqReceiveHostName = System.Configuration.ConfigurationManager.AppSettings["MqReceiveHostName"];
            Util.GlobalVariables.MqReceiveVHostName = System.Configuration.ConfigurationManager.AppSettings["MqReceiveVHostName"];
            Util.GlobalVariables.MqReceiveUserName = System.Configuration.ConfigurationManager.AppSettings["MqReceiveUserName"];
            Util.GlobalVariables.MqReceivePassword = System.Configuration.ConfigurationManager.AppSettings["MqReceivePassword"];
            Util.GlobalVariables.MqListenQueueName = System.Configuration.ConfigurationManager.AppSettings["MqListenQueueName"];
            Util.GlobalVariables.MqReceivePort = Instinct.Collection.Lib.Util.Tool.IsNumberic(System.Configuration.ConfigurationManager.AppSettings["MqReceivePort"]) ? Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MqReceivePort"]) : 0;
            Util.GlobalVariables.MqReceiveProcessCount = Instinct.Collection.Lib.Util.Tool.IsNumberic(System.Configuration.ConfigurationManager.AppSettings["MqReceiveProcessCount"]) ? Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MqReceiveProcessCount"]) : 0;
            string EncodeName = System.Configuration.ConfigurationManager.AppSettings["MqEncode"];
            Util.GlobalVariables.MqEncoding = EncodeName;

            Instinct.Collection.Lib.Util.Tool.ConnectDatabase();

            Util.GlobalVariables.MqNeedDeclareQueue = System.Configuration.ConfigurationManager.AppSettings["MqNeedDeclareQueue"] != null && System.Configuration.ConfigurationManager.AppSettings["MqNeedDeclareQueue"].ToString().ToUpper() == "TRUE" ? true : false;
			
			string subtablestr = System.Configuration.ConfigurationManager.AppSettings["SubTables"];
            Util.GlobalVariables.SubTables = new ArrayList();
            if (subtablestr != null && subtablestr != "")
            {
                Util.GlobalVariables.SubTables.AddRange(subtablestr.Split('|'));
            }
			
			Util.GlobalVariables.MqAutoAck = System.Configuration.ConfigurationManager.AppSettings["MqAutoAck"] != null && Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["MqAutoAck"]).ToUpper() == "TRUE" ? true : false;
            Util.GlobalVariables.MqHeartBeat = (ushort)(Instinct.Collection.Lib.Util.Tool.IsNumberic(System.Configuration.ConfigurationManager.AppSettings["MqHeartBeat"]) ? Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MqHeartBeat"]) : 60);

        }
        /// <summary>
        /// 初始话MQ 客户端
        /// </summary>
        /// <returns></returns>
        private static SCM.RabbitMQClient.RabbitMqClientJSONSimple GetRabbitMQReceiver()
        {
            SCM.RabbitMQClient.RabbitMqClientJSONSimple client = new SCM.RabbitMQClient.RabbitMqClientJSONSimple(Util.GlobalVariables.MqReceiveHostName, Util.GlobalVariables.MqReceiveVHostName, Util.GlobalVariables.MqListenQueueName, Util.GlobalVariables.MqReceivePort, Util.GlobalVariables.MqReceiveUserName, Util.GlobalVariables.MqReceivePassword, Util.GlobalVariables.MqEncoding, Util.GlobalVariables.MqNeedDeclareQueue,false,Util.GlobalVariables.MqAutoAck,Util.GlobalVariables.MqHeartBeat);
            return client;
        }
    }
}
