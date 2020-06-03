using System;
using System.Text;

namespace SCM.RabbitMQClient
{
    /// <summary>
    /// 创建EventMessage实例。
    /// </summary>
    public class EventMessageFactory
    {
        /// <summary>
        /// 创建EventMessage传输对象。
        /// </summary>
        /// <param name="originObject">原始强类型对象实例。</param>
        /// <param name="eventMessageMarkcode">消息的标记码。</param>
        /// <typeparam name="T">原始对象类型。</typeparam>
        /// <returns>EventMessage.</returns>
        public static EventMessage CreateEventMessageXMLInstance<T>(T originObject, string eventMessageMarkcode, string responseMethod = "")
            where T : class, new()
        {
            var result = new EventMessage()
            {
                CreateDateTime = DateTime.Now,
                EventMessageMarkcode = eventMessageMarkcode,
                ResponseMethod = responseMethod 
            };

            var bytes = MessageSerializerFactory.CreateMessageXMLSerializerInstance().SerializerBytes<T>(originObject);

            result.EventMessageBytes = bytes;

            return result;
        }
        /// <summary>
        /// 创建EventMessage传输对象。
        /// </summary>
        /// <param name="originObject">原始强类型对象实例。</param>
        /// <param name="eventMessageMarkcode">消息的标记码。</param>
        /// <typeparam name="T">原始对象类型。</typeparam>
        /// <returns>EventMessage.</returns>
        public static EventMessage CreateEventMessageJSONInstance<T>(T originObject, string eventMessageMarkcode, string responseMethod = "")
            where T : class, new()
        {
            var result = new EventMessage()
            {
                CreateDateTime = DateTime.Now,
                EventMessageMarkcode = eventMessageMarkcode,
                ResponseMethod =responseMethod 
            };

            var bytes = MessageSerializerFactory.CreateMessageJsonSerializerInstance().SerializerBytes<T>(originObject);

            result.EventMessageBytes = bytes;

            return result;
        }
        /// <summary>
        /// 创建EventMessage传输对象
        /// </summary>
        /// <param name="message">字符串类型消息</param>
        /// <param name="eventMessageMarkcode">消息标记码</param>
        /// <param name="encode"></param>
        /// <returns>字符串消息编码信息</returns>
        public static EventMessage CreateEventMessageInstance(String message, string eventMessageMarkcode, Encoding encode = null, string responseMethod = "")
        {
            var result = new EventMessage()
            {
                CreateDateTime = DateTime.Now,
                EventMessageMarkcode = eventMessageMarkcode,
                ResponseMethod =responseMethod 
            };

            if (encode == null)
                encode = Encoding.Default;
            
            var bytes = encode.GetBytes(message);

            result.EventMessageBytes = bytes;

            return result;
        }
    }
}