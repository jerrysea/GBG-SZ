using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using Newtonsoft.Json;

namespace SCM.RabbitMQClient.Common
{
    /// <summary>
    /// Json 转换
    /// </summary>
    public class JsonXmlObjectParser
    {
        /// <summary>
        /// Xml 转换Json
        /// </summary>
        /// <param name="xmlStr"></param>
        /// <returns></returns>
        public static string XmlToJson(string xmlStr)
        {
            var doc = new XmlDocument();
            doc.LoadXml(xmlStr);
            var json = JsonConvert.SerializeXmlNode(doc);
            return json;
        }
        /// <summary>
        /// Json 转换XMl
        /// </summary>
        /// <param name="jsonStr"></param>
        /// <returns></returns>
        public static string JsonToXml(string jsonStr)
        {
            var doc = (XmlDocument)JsonConvert.DeserializeXmlNode(jsonStr);
            return ConvertXmlToString(doc);
        }
        /// <summary>
        /// 以rootElementName为根的xml
        /// </summary>
        /// <param name="jsonStr"></param>
        /// <param name="rootElementName"></param>
        /// <returns></returns>
        public static string JsonToXml(string jsonStr, string rootElementName)
        {
            var doc = (XmlDocument)JsonConvert.DeserializeXmlNode(jsonStr, rootElementName);
            return ConvertXmlToString(doc);
        }
        /// <summary>
        /// 将XML 对象转换字符串
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public static string ConvertXmlToString(XmlDocument doc)
        {
            var stream = new MemoryStream();
            var writer = new XmlTextWriter(stream, null);
            writer.Formatting = System.Xml.Formatting.Indented;

            doc.Save(writer);
            var sr = new StreamReader(stream, System.Text.Encoding.UTF8);
            stream.Position = 0;
            string xmlString = sr.ReadToEnd();
            sr.Close();
            stream.Close();
            return xmlString;
        }
        /// <summary>
        /// 将json 字符串转换成T对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonStr"></param>
        /// <returns></returns>
        public static T DeserializeObject<T>(string jsonStr)
        {
            if (string.IsNullOrEmpty(jsonStr))
            {
                return default(T);
            }
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            var jsonObject = JsonConvert.DeserializeObject<T>(jsonStr, jsonSerializerSettings);
            return jsonObject;
        }
        /// <summary>
        /// 某对象序列化
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string SerializeObject(object value)
        {
            if (value == null)
            {
                return "";
            }
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            var objectJson = JsonConvert.SerializeObject(value, jsonSerializerSettings);
            return objectJson;
        }
        /// <summary>
        /// 某对象序列化为字节码
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] SerializeToBytes(object value)
        {
            if (value == null)
            {
                return new byte[0];
            }
            var message = SerializeObject(value);
            var body = Encoding.UTF8.GetBytes(message);
            return body;
        }
        /// <summary>
        /// 某对象序列化为字节码
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] SerializeToBytes(object value, Encoding encoder)
        {
            if (value == null)
            {
                return new byte[0];
            }
            var message = SerializeObject(value);
            var body = encoder.GetBytes(message);
            return body;
        } 
        /// <summary>
        /// 将某字节码反序列化对象T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T DeserializeBytes<T>(byte[] value)
        {
            if (value == null)
            {
                return default(T);
            }
            var result = Encoding.UTF8.GetString(value);
            var tObject = DeserializeObject<T>(result);
            return tObject;
        }
        /// <summary>
        /// 将某字节码反序列化对象T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T DeserializeBytes<T>(byte[] value,Encoding encoder)
        {
            if (value == null)
            {
                return default(T);
            }
            var result = encoder.GetString(value);
            var tObject = DeserializeObject<T>(result);
            return tObject;
        }
    }
}
