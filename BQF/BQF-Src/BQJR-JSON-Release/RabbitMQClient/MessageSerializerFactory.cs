using System.Text;
namespace SCM.RabbitMQClient
{
    /// <summary>
    /// IMessageSerializer 创建工厂。
    /// </summary>
    public class MessageSerializerFactory
    {       

        /// <summary>
        /// 创建一个XML消息序列化组件。
        /// </summary>
        /// <returns></returns>
        public static IMessageSerializer CreateMessageXMLSerializerInstance(string EncodingName="UTF-8")
        {
            return new MessageXmlSerializer(EncodingName);
        }        

        /// <summary>
        /// 创建一个JSON消息序列化组件。
        /// </summary>
        /// <returns></returns>
        public static IMessageSerializer CreateMessageJsonSerializerInstance(string EncodingName = "UTF-8")
        {
            return new MessageJsonSerializer(EncodingName);
        }
    }
}