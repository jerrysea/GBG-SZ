using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Instinct.RabbitMQ.InterfaceClient
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            SetConfigValues();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Frm.MainFrm());
        }

        private static void SetConfigValues()
        {
            //Set MQ Config Value
            Util.GlobalParameters.MqHostName = System.Configuration.ConfigurationManager.AppSettings["MqHostName"];
            Util.GlobalParameters.MqvHostName = System.Configuration.ConfigurationManager.AppSettings["MqvHostName"];
            Util.GlobalParameters.MqUserName = System.Configuration.ConfigurationManager.AppSettings["MqUserName"];
            Util.GlobalParameters.MqPassword = System.Configuration.ConfigurationManager.AppSettings["MqPassword"];
            Util.GlobalParameters.MqListenQueueName = System.Configuration.ConfigurationManager.AppSettings["MqListenQueueName"];
            Util.GlobalParameters.MqSendQueueName = System.Configuration.ConfigurationManager.AppSettings["MqSendQueueName"];
            Util.GlobalParameters.MqSendExchange = System.Configuration.ConfigurationManager.AppSettings["MqSendExchange"];
            Util.GlobalParameters.MqPort = Util.Tool.IsNumberic(System.Configuration.ConfigurationManager.AppSettings["MqPort"]) ? Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MqPort"]) : 0;
            string EncodeName = System.Configuration.ConfigurationManager.AppSettings["Encode"];
            Util.GlobalParameters.Encoding = EncodeName;            
            Util.GlobalParameters.MethodNames = System.Configuration.ConfigurationManager.AppSettings["MethodNames"];
            Util.GlobalParameters.MqSynchronization = System.Configuration.ConfigurationManager.AppSettings["MqSynchronization"] != null && System.Configuration.ConfigurationManager.AppSettings["MqSynchronization"].ToString().ToUpper() == "TRUE" ? true : false;

            
        }
    }
}
