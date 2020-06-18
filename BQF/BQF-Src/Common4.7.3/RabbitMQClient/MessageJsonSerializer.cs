using SCM.RabbitMQClient.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCM.RabbitMQClient
{
    /// <summary>
    /// 用Newtonsoft的消息序列化。
    /// </summary>
    public class MessageJsonSerializer : IMessageSerializer
    {
        #region 属性
        private Encoding _Encoder;

        public Encoding Encoder { get { return this._Encoder; } set { this._Encoder = value; } }

        #endregion

        #region 构造函数

        public MessageJsonSerializer(string EncodingName = "UTF-8")
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
            
            return JsonXmlObjectParser.SerializeToBytes(message,this._Encoder);
        }
        
        /// <summary>
        /// 反序列化消息。
        /// </summary>
        /// <typeparam name="T">消息的类型。</typeparam>
        /// <param name="bytes">bytes。</param>
        /// <returns></returns>
        public T Deserialize<T>(byte[] bytes) where T : class, new()
        {
            return JsonXmlObjectParser.DeserializeBytes<T>(bytes,this._Encoder);
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
        /// <summary>
        /// 序列化消息为Json字符串。
        /// </summary>
        /// <typeparam name="T">消息类型</typeparam>
        /// <param name="message">消息实例</param>
        /// <returns></returns>
        public string SerializerString<T>(T message) where T : class, new()
        {
            return Common.JsonXmlObjectParser.SerializeObject(message);
        }
        #endregion
    }
}
