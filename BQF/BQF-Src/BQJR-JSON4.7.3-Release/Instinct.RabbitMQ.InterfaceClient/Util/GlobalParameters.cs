using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instinct.RabbitMQ.InterfaceClient.Util
{
    public class GlobalParameters
    {
        public static string MqHostName { get; set; }
        public static string MqvHostName { get; set; }
        public static string MqUserName { get; set; }
        public static string MqPassword { get; set; }
        public static string MqListenQueueName { get; set; }
        public static string MqSendQueueName { get; set; }
        public static string MqSendExchange { get; set; }
        public static int MqPort { get; set; }
        public static string MethodNames { get; set; }
        public static string Encoding { get; set; }
        public static bool MqSynchronization { get; set; }
     
    }
}
