using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace SCM.RabbitMQClient
{
    /// <summary>
    /// 面向XML的消息序列化。
    /// </summary>
    public class MessageXmlSerializer : IMessageSerializer
    {
        #region 属性
        private Encoding _Encoder;

        public Encoding Encoder { get { return this._Encoder; } set { this._Encoder = value; } }
        #endregion

        #region 构造函数      
        public MessageXmlSerializer(string EncodingName="UTF-8")
        {
            this._Encoder = Encoding.GetEncoding(EncodingName);
        }
        #endregion

        #region 公有函数
        /// <summary>
        /// 序列化成bytes。
        /// </summary>
        /// <typeparam name="T">消息的类型。</typeparam>
        /// <param name="message">消息的实例。</param>
        /// <returns></returns>
        public byte[] SerializerBytes<T>(T message) where T : class, new()
        {
            var xmlSerializer = new XmlSerializer(typeof (T));
            using (var msStream = new MemoryStream())
            {
                xmlSerializer.Serialize(msStream, message);
                return msStream.ToArray();
            }
        }

        /// <summary>
        /// 序列化消息为XML字符串。
        /// </summary>
        /// <param name="message">消息类型</param>
        /// <typeparam name="T">消息实例</typeparam>
        /// <returns></returns>
        public string SerializerString<T>(T message) where T : class, new()
        {
            var xmlSerializer = new XmlSerializer(typeof(T));
            using (var msStream = new StringWriter())
            {
                xmlSerializer.Serialize(msStream, message);
                return msStream.ToString();
            }
        }
        
        /// <summary>
        /// 反序列化消息。
        /// </summary>
        /// <typeparam name="T">消息的类型。</typeparam>
        /// <param name="bytes">bytes。</param>
        /// <returns></returns>
        public T Deserialize<T>(byte[] bytes) where T : class, new()
        {
            var xmlSerializer = new XmlSerializer(typeof (T));
            using (var msStream = new MemoryStream(bytes))
            {
                return xmlSerializer.Deserialize(msStream) as T;
            }
        }
        /// <summary>
        /// 反序列化消息。
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="encode">编码类型</param>
        /// <returns></returns>
        public string Deserialize(byte[] bytes)
        {            
            return this._Encoder.GetString(bytes);
        }
        #endregion
    }
}