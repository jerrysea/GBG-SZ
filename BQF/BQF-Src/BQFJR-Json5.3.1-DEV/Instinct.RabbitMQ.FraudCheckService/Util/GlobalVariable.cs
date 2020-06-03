using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instinct.RabbitMQ.FraudCheckService.Util
{
    public class GlobalVariable
    {
        public static string MqSendHostName { get; set; }
        public static string MqSendVHostName { get; set; }
        public static string MqSendUserName { get; set; }
        public static string MqSendPassword { get; set; }
        public static string MqSendQueueName { get; set; }
        public static string MqSendExchange { get; set; }
        public static int MqSendPort { get; set; }
        public static string MqReceiveHostName { get; set; }
        public static string MqReceiveVHostName { get; set; }
        public static string MqReceiveUserName { get; set; }
        public static string MqReceivePassword { get; set; }
        public static string MqListenQueueName { get; set; }
        public static int MqReceivePort { get; set; }
        public static int MqReceiveProcessCount { get; set; }
        public static ClsOnlineServiceCall.SetINIValue Config { get; set; }
        public static string MqEncoding { get; set; }
        public static  ArrayList CompanySuffix { get; set; }
        public static string CnnString { get; set; }
        public static bool MqNeedDeclareQueue { get; set; }
        public static bool MqSynchronization { get; set; }
        public static bool BReferenceSynonyms { get; set; }
        public static ArrayList ReferenceTables { get; set; }
        public static bool BParticiple { get; set; }
        public static int WaitingSeconds { get; set; }
        public static bool MqAutoAck { get; set; }
        public static ushort MqHeartBeat { get; set; }
    }
}
