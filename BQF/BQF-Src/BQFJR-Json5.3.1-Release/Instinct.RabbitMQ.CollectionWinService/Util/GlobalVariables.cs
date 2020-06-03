using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instinct.RabbitMQ.CollectionWinService.Util
{
    public class GlobalVariables
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
        public static string MqEncoding { get; set; }
        public static bool MqNeedDeclareQueue { get; set; }
        public static ArrayList SubTables { get; set; }
    }
}
