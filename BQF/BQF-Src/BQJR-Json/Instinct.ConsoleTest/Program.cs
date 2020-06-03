using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Instinct.ConsoleTest
{
    class Program
    {
        //private static ClsOnlineServiceCall.SetINIValue oConfig;

        public static string MqHostName;
        public static string MqvHostName;
        public static string MqUserName;
        public static string MqPassword;
        public static string MqListenQueueName;
        public static string MqSendQueueName;
        public static string MqSendExchange;
        public static int MqPort;
        public static int MqProcessCount;
        static void Main(string[] args)
        {
            try
            {
                //oConfig = new ClsOnlineServiceCall.SetINIValue();
                SetConfigValues();
                string filename = "inputsample1.xml";
                XmlDocument doc=new XmlDocument();
                doc.Load(filename);
                //string xmlvalue = System.IO.File.ReadAllText(filename, Encoding.ASCII); 
                
                //Instinct.RabbitMQ.FraudCheckService.DAL.InstinctFraudCheckThread t = new RabbitMQ.FraudCheckService.DAL.InstinctFraudCheckThread(oConfig, Instinct.RabbitMQ.FraudCheckService.Util.LogHelper.DebugLog);
                //string output = t.InstinctFraudCheck_XMLString(xmlvalue);
                //Console.WriteLine(output);
                //Console.ReadLine();
                //DateTime d = DateTime.Now;
                //Console.WriteLine(d.ToString("HHmmss"));
                SCM.RabbitMQClient.Common.LogLocation.Log = new LogHelper();
                SCM.RabbitMQClient.IRabbitMqClient client = new SCM.RabbitMQClient.RabbitMqClientJSONSimple(MqHostName, MqvHostName, MqSendQueueName, MqPort, MqUserName, MqPassword, "UTF-8", true, true);
                
                for (int i = 0; i < 10000; i++)
                {
                    doc.SelectSingleNode("ApplicationSchema").SelectSingleNode("Application").SelectSingleNode("Application_Number").InnerText = "ceshi_" + i.ToString("0000");
                    string sInput=SCM.RabbitMQClient.Common.JsonXmlObjectParser.ConvertXmlToString(doc);
                    var sendMessage =
                            SCM.RabbitMQClient.EventMessageFactory.CreateEventMessageInstance(sInput, i.ToString(), Encoding.GetEncoding("UTF-8"), "INSTINCTFRAUDCHECK_XMLSTRING");

                    client.TriggerEventMessage(sendMessage, MqSendExchange, MqSendQueueName);
                }

            }
            catch (Exception ex)
            {
                
                throw;
            }
            
        }

        private static void SetConfigValues()
        {
            //oConfig.AppActionOutputFileFlag = System.Configuration.ConfigurationManager.AppSettings["AppActionOutputFileFlag"];
            //oConfig.AppInputFormat = System.Configuration.ConfigurationManager.AppSettings["AppInputFormat"];
            //oConfig.AppLocalTimeDifference = System.Configuration.ConfigurationManager.AppSettings["AppLocalTimeDifference"];
            //oConfig.AppOrganisation = System.Configuration.ConfigurationManager.AppSettings["AppOrganisation"];
            //oConfig.AppOutputFormat = System.Configuration.ConfigurationManager.AppSettings["AppOutputFormat"];
            //oConfig.AppOutputDirectory = System.Configuration.ConfigurationManager.AppSettings["AppOutputDirectory"];
            //oConfig.UseWindowsAuthentication = System.Configuration.ConfigurationManager.AppSettings["UseWindowsAuthentication"];
            //oConfig.UseDefinedEncryptionKey = System.Configuration.ConfigurationManager.AppSettings["UseDefinedEncryptionKey"];
            //oConfig.Key1Path = System.Configuration.ConfigurationManager.AppSettings["Key1Path"];
            //oConfig.Key2Path = System.Configuration.ConfigurationManager.AppSettings["Key2Path"];
            //oConfig.DatabaseUserId = System.Configuration.ConfigurationManager.AppSettings["DatabaseUserId"];
            //oConfig.DatabasePassword = System.Configuration.ConfigurationManager.AppSettings["DatabasePassword"];
            //oConfig.DataSource = System.Configuration.ConfigurationManager.AppSettings["DataSource"];
            //oConfig.DefaultCountry = System.Configuration.ConfigurationManager.AppSettings["DefaultCountry"];
            //oConfig.DelimiterCharacters = System.Configuration.ConfigurationManager.AppSettings["DelimiterCharacters"];
            //oConfig.InitialCatalog = System.Configuration.ConfigurationManager.AppSettings["InitialCatalog"];
            //oConfig.RulesInOutputFile = System.Configuration.ConfigurationManager.AppSettings["RulesInOutputFile"];
            //oConfig.RulesDescriptionInOutputFile = System.Configuration.ConfigurationManager.AppSettings["RulesDescriptionInOutputFile"];
            //oConfig.ActionCountNumberInOutputFile = System.Configuration.ConfigurationManager.AppSettings["ActionCountNbrInOutputFile"];
            //oConfig.NatureOfFraudInOutputFile = System.Configuration.ConfigurationManager.AppSettings["NatureOfFraudInOutputFile"];
            //oConfig.DiaryInOutputFile = System.Configuration.ConfigurationManager.AppSettings["DiaryInOutputFile"];
            //oConfig.SiteWithSpecialFunctions = System.Configuration.ConfigurationManager.AppSettings["SiteWithSpecialFunctions"];
            //oConfig.SecondaryServicePrefix = System.Configuration.ConfigurationManager.AppSettings["SecondServiceSuffix"];
            //oConfig.UserIdInOutputFile = System.Configuration.ConfigurationManager.AppSettings["UserIdInOutputFile"];
            //oConfig.WriteLogFile = System.Configuration.ConfigurationManager.AppSettings["WriteLogFile"];
            //oConfig.DecisionReasonInOutputFile = System.Configuration.ConfigurationManager.AppSettings["DecisionReasonInOutputFile"];
            //oConfig.UserDefinedAlertInOutputFile = System.Configuration.ConfigurationManager.AppSettings["UserDefinedAlertInOutputFile"];
            //oConfig.GroupMemberCode = System.Configuration.ConfigurationManager.AppSettings["GroupMemberCode"];
            //oConfig.LowFraudScore = System.Configuration.ConfigurationManager.AppSettings["LowFraudScore"];
            //oConfig.NewApplicationsAge = System.Configuration.ConfigurationManager.AppSettings["NewApplicationsAge"];
            //oConfig.FraudAlertUserId = System.Configuration.ConfigurationManager.AppSettings["FraudAlertUserIdInOutputFile"];
            //oConfig.MaxPoolSize = System.Configuration.ConfigurationManager.AppSettings["MaxPoolSize"];
            //oConfig.ApplicationName = System.Configuration.ConfigurationManager.AppSettings["ApplicationName"];
            //Set MQ Config Value
            MqHostName = System.Configuration.ConfigurationManager.AppSettings["MqHostName"];
            MqvHostName = System.Configuration.ConfigurationManager.AppSettings["MqvHostName"];
            MqUserName = System.Configuration.ConfigurationManager.AppSettings["MqUserName"];
            MqPassword = System.Configuration.ConfigurationManager.AppSettings["MqPassword"];
            MqListenQueueName = System.Configuration.ConfigurationManager.AppSettings["MqListenQueueName"];
            MqSendQueueName = System.Configuration.ConfigurationManager.AppSettings["MqSendQueueName"];
            MqSendExchange = System.Configuration.ConfigurationManager.AppSettings["MqSendExchange"];
            MqPort = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MqPort"]);
            MqProcessCount = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MqProcessCount"]);

        }
    }
}
