using SCM.RabbitMQClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instinct.RabbitMQ.InterfaceClient.DAL
{
    class MQClient
    {
        #region 属性
        public string encodingName { get; set; }
        public string sendexChange { get; set; }
        public string sendqueuename { get; set; }
        public string replyqueuename { get; set; }

        private ScintillaNET.Scintilla richtext;

        public delegate void _AppendRichText(ScintillaNET.Scintilla text, string msg);

        public _AppendRichText AppendText;

        #endregion

        #region 构造
        public MQClient(ScintillaNET.Scintilla richtext,_AppendRichText append)
        {
            Init();
            SCM.RabbitMQClient.Common.LogLocation.Log = new Util.LogHelper();
            this.richtext = richtext;
            this.AppendText = append;
        }
        #endregion

        #region 公有
        /// <summary>
        /// 侦听
        /// </summary>
        public void Listening()
        {
            RabbitMqClientJSONSingle.Instance.EncodingName = this.encodingName;
            if (!Util.GlobalParameters.MqSynchronization)
            {                
                if (RabbitMqClientFactory.MqListenQueueName != null && RabbitMqClientFactory.MqListenQueueName != "")
                {
                    RabbitMqClientJSONSingle.Instance.ActionEventMessage += mqClient_ActionJSONEventMessage;
                    RabbitMqClientJSONSingle.Instance.OnListening();  
                }                
            }                      
        }
        /// <summary>
        /// 压入队列
        /// </summary>
        /// <param name="message"></param>
        /// <param name="methodname"></param>
        public void PressInQueue(string message, string methodname )
        {
            string sOutput = string.Empty;
            Encoding encoder = Encoding.GetEncoding(Util.GlobalParameters.Encoding);
            string sMarkCode = Guid.NewGuid().ToString();
            var sendMessage =
                        EventMessageFactory.CreateEventMessageInstance(message,sMarkCode, encoder, methodname.ToUpper());

            if (Util.GlobalParameters.MqSynchronization)
            {
                RabbitMqClientJSONSingle.Instance.SynchronizationFlag = true;
                sOutput = RabbitMqClientJSONSingle.Instance.RpcClient(sendMessage, this.sendexChange, this.sendqueuename, replyqueuename);
                this.AppendText(this.richtext, sOutput);
            }
            else
            {
                RabbitMqClientJSONSingle.Instance.SynchronizationFlag = false;
                RabbitMqClientJSONSingle.Instance.TriggerEventMessage(sendMessage, this.sendexChange, this.sendqueuename);
            }                        
        }
        /// <summary>
        /// 注销
        /// </summary>
        public void Exit()
        {
            RabbitMqClientJSONSingle.Instance.Dispose();            
        }

        #endregion

        #region 私有
        private void Init()
        {
            RabbitMqClientFactory.MqHostName =Util.GlobalParameters.MqHostName;
            RabbitMqClientFactory.MqvHostName =Util.GlobalParameters.MqvHostName;
            RabbitMqClientFactory.MqUserName = Util.GlobalParameters.MqUserName;
            RabbitMqClientFactory.MqPassword =Util.GlobalParameters.MqPassword;
            RabbitMqClientFactory.MqListenQueueName =Util.GlobalParameters.MqListenQueueName;
            RabbitMqClientFactory.MqPort = Util.GlobalParameters.MqPort;

            this.sendexChange = Util.GlobalParameters.MqSendExchange;
            this.sendqueuename = Util.GlobalParameters.MqSendQueueName;
            this.replyqueuename = Util.GlobalParameters.MqListenQueueName;
            this.encodingName = Util.GlobalParameters.Encoding;
            
        }
        /// <summary>
        /// xml格式 侦听时间
        /// </summary>
        /// <param name="result"></param>
        private void mqClient_ActionJSONEventMessage(EventMessageResult result)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                Util.LogHelper.InfoLog(result.EventMessageBytes.EventMessageMarkcode);
                sb.Append(result.EventMessageBytes.EventMessageMarkcode);
                Util.LogHelper.InfoLog(":\n");
                sb.Append(":\n");

                string message =
                        MessageSerializerFactory.CreateMessageJsonSerializerInstance(Util.GlobalParameters.Encoding)
                            .Deserialize(result.MessageBytes);
                Util.LogHelper.InfoLog(string.Format("RESPONSE:{0}", message));
                sb.Append(string.Format("RESPONSE:{0}", message));
                sb.Append("\n");
                this.AppendText(this.richtext, sb.ToString());
            }
            catch (Exception ex)
            {
                Util.LogHelper.ErrorLog("Listening Action Message",ex);
            }
            finally
            {
                result.IsOperationOk = true; //处理成功
            }            
        }       
        #endregion
    }
}
