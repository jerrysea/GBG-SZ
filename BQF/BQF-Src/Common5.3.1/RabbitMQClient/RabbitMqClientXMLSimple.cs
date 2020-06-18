using System;
using RabbitMQ.Client.Events;
using SCM.RabbitMQClient.Common;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics;

namespace SCM.RabbitMQClient
{
    /// <summary>
    /// MQ 简单类
    /// </summary>
    public class RabbitMqClientXMLSimple : IRabbitMqClient
    {
        #region 构造
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="host"></param>
        /// <param name="vhost"></param>
        /// <param name="queue"></param>
        /// <param name="port"></param>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <param name="encode"></param>
        /// <param name="needdeclare"></param>
        /// <param name="synchronizationFlag"></param>
        /// <param name="autoAck"></param>
        /// <param name="heartbeat"></param>
        public RabbitMqClientXMLSimple(string host, string vhost, string queue, int port, string user, string password, string encode, bool needdeclare = true, bool synchronizationFlag = false,bool autoAck=false, ushort heartbeat =60)
        {
            MQHost = host;
            MQVHost = vhost;
            MQPort = port;
            MQUser = user;
            MQPassword = password;
            EncodingName = encode;
            AutoAck = autoAck;
            HeartBeat = heartbeat;

            var rabbitMqClientContext = new RabbitMqClientContext
            {
                ListenQueueName = queue,//MqConfigDomFactory.CreateConfigDomInstance().MqListenQueueName,
                InstanceCode = Guid.NewGuid().ToString()
            };

            this.Context = rabbitMqClientContext;
            this.NeedDeclare = needdeclare;
            this.SynchronizationFlag = synchronizationFlag;
        }

        #endregion

        #region 属性 fields
        public String EncodingName { get; set; }
        public string MQHost { get; set; }
        public string MQVHost { get; set; }
        public int MQPort { get; set; }
        public string MQUser { get; set; }
        public string MQPassword { get; set; }
        public bool NeedDeclare { get; set; }
        public bool SynchronizationFlag { get; set; }

        public bool AutoAck { get; set; }

        public ushort HeartBeat { get; set; }

        /// <summary>
        /// RabbitMqClient 数据上下文。
        /// </summary>
        public RabbitMqClientContext Context { get; set; }

        /// <summary>
        /// 事件激活委托实例。
        /// </summary>
        private ActionEvent _actionMessage;

        /// <summary>
        /// 当侦听的队列中有消息到达时触发的执行事件。
        /// </summary>
        public event ActionEvent ActionEventMessage
        {
            add
            {
                if (_actionMessage.IsNull())
                    _actionMessage += value;
            }
            remove
            {
                if (_actionMessage.IsNotNull())
                    _actionMessage -= value;
            }
        }

        public event RabbitMqClientFactory.ThreadExceptionEventHandler ThreadExceptionEvents;
        #endregion

        #region send method

        /// <summary>
        /// 触发一个事件且将事件打包成消息发送到远程队列中。
        /// </summary>
        /// <param name="eventMessage">发送的消息实例。</param>
        /// <param name="exChange">RabbitMq的Exchange名称。</param>
        /// <param name="queue">队列名称。</param>
        /// <param name="replyto">应答队列</param>
        /// <param name="corelationid">关联ID</param>
        /// <param name="contenttype">内容类型。</param>
        public void TriggerEventMessage(EventMessage eventMessage, string exChange, string queue, string replyto = "", string corelationid = "", string contenttype = "")
        {
            RabbitMQ.Client.IConnection SendConnection = null;
            try
            {
                SendConnection = RabbitMqClientFactory.CreateConnection(this.MQHost, this.MQVHost, this.MQUser, this.MQPassword, this.MQPort,this.HeartBeat); //获取连接
                using (SendConnection)
                {
                    RabbitMQ.Client.IModel SendChannel = RabbitMqClientFactory.CreateModel(SendConnection); //获取通道

                    if (NeedDeclare)
                    {
                        SendChannel.QueueDeclare(queue: queue,
                                                            durable: true,
                                                            exclusive: false,
                                                            autoDelete: false,
                                                            arguments: null);
                    }

                    const byte deliveryMode = 2;
                    using (SendChannel)
                    {
                        var messageSerializer = MessageSerializerFactory.CreateMessageXMLSerializerInstance(this.EncodingName); //反序列化消息

                        var properties = SendChannel.CreateBasicProperties();
                        properties.DeliveryMode = deliveryMode; //表示持久化消息                    

                        if (this.SynchronizationFlag)
                        {
                            if (corelationid != null && corelationid != "")
                                properties.CorrelationId = corelationid;
                            if (contenttype != null && contenttype != "")
                                properties.ContentType = contenttype;
                            if (replyto != null && replyto != "")
                                properties.ReplyTo = replyto;
                        }

                        //推送消息
                        SendChannel.BasicPublish(
                            exChange, queue, properties, messageSerializer.SerializerBytes(eventMessage));
                    }
                }
            }
            catch (Exception ex1)
            {
                try
                {
                    SendConnection = RabbitMqClientFactory.CreateConnection(this.MQHost, this.MQVHost, this.MQUser, this.MQPassword, this.MQPort,this.HeartBeat); //获取连接
                    using (SendConnection)
                    {
                        RabbitMQ.Client.IModel SendChannel = RabbitMqClientFactory.CreateModel(SendConnection); //获取通道

                        if (NeedDeclare)
                        {
                            SendChannel.QueueDeclare(queue: queue,
                                                                durable: true,
                                                                exclusive: false,
                                                                autoDelete: false,
                                                                arguments: null);
                        }

                        const byte deliveryMode = 2;
                        using (SendChannel)
                        {
                            var messageSerializer = MessageSerializerFactory.CreateMessageXMLSerializerInstance(this.EncodingName); //反序列化消息

                            var properties = SendChannel.CreateBasicProperties();
                            properties.DeliveryMode = deliveryMode; //表示持久化消息                    

                            if (this.SynchronizationFlag)
                            {
                                if (corelationid != null && corelationid != "")
                                    properties.CorrelationId = corelationid;
                                if (contenttype != null && contenttype != "")
                                    properties.ContentType = contenttype;
                                if (replyto != null && replyto != "")
                                    properties.ReplyTo = replyto;
                            }

                            //推送消息
                            SendChannel.BasicPublish(
                                exChange, queue, properties, messageSerializer.SerializerBytes(eventMessage));
                        }
                    }
                }
                catch (Exception ex2)
                {
                    try
                    {
                        SendConnection = RabbitMqClientFactory.CreateConnection(this.MQHost, this.MQVHost, this.MQUser, this.MQPassword, this.MQPort,this.HeartBeat); //获取连接
                        using (SendConnection)
                        {
                            RabbitMQ.Client.IModel SendChannel = RabbitMqClientFactory.CreateModel(SendConnection); //获取通道

                            if (NeedDeclare)
                            {
                                SendChannel.QueueDeclare(queue: queue,
                                                                    durable: true,
                                                                    exclusive: false,
                                                                    autoDelete: false,
                                                                    arguments: null);
                            }

                            const byte deliveryMode = 2;
                            using (SendChannel)
                            {
                                var messageSerializer = MessageSerializerFactory.CreateMessageXMLSerializerInstance(this.EncodingName); //反序列化消息

                                var properties = SendChannel.CreateBasicProperties();
                                properties.DeliveryMode = deliveryMode; //表示持久化消息                    

                                if (this.SynchronizationFlag)
                                {
                                    if (corelationid != null && corelationid != "")
                                        properties.CorrelationId = corelationid;
                                    if (contenttype != null && contenttype != "")
                                        properties.ContentType = contenttype;
                                    if (replyto != null && replyto != "")
                                        properties.ReplyTo = replyto;
                                }

                                //推送消息
                                SendChannel.BasicPublish(
                                    exChange, queue, properties, messageSerializer.SerializerBytes(eventMessage));
                            }
                        }
                    }
                    catch (Exception ex3)
                    {
                        if (LogLocation.Log.IsNotNull())
                            LogLocation.Log.WriteException("SCM.RabbitMQClient", ex3);
                    }
                }
            }
            finally
            {
                if (!SendConnection.IsNull())
                {
                    if (SendConnection.IsOpen)
                        SendConnection.Close();

                    SendConnection.Dispose();
                }
            }
        }

        #endregion

        #region receive method

        /// <summary>
        /// 开始侦听默认的队列。
        /// </summary>
        public void OnListening()
        {
            try
            {
                Task.Factory.StartNew(ListenInit, TaskCreationOptions.AttachedToParent);
            }
            catch (Exception ex)
            {
                if (LogLocation.Log.IsNotNull())
                    LogLocation.Log.WriteException("SCM.RabbitMQClient", ex);

            }
        }

        /// <summary>
        /// 侦听初始化。
        /// </summary>
        private void ListenInit()
        {
            try
            {
                if (LogLocation.Log.IsNotNull())
                    LogLocation.Log.WriteInfo("START LISTENNING ", "TASK ID:[" + Task.CurrentId + "]...");

                Context.ListenConnection = RabbitMqClientFactory.CreateConnection(this.MQHost, this.MQVHost, this.MQUser, this.MQPassword, this.MQPort,this.HeartBeat); //获取连接

                Context.ListenConnection.ConnectionShutdown += (o, e) =>
                {
                    if (LogLocation.Log.IsNotNull())
                        LogLocation.Log.WriteInfo("SCM.RabbitMQClient", "connection shutdown:" + e.ReplyText);
                    ListenInit();
                };               

                Context.ListenChannel = RabbitMqClientFactory.CreateModel(Context.ListenConnection); //获取通道

                if (NeedDeclare)
                {
                    Context.ListenChannel.QueueDeclare(queue: Context.ListenQueueName,
                                         durable: true,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);
                }

                var consumer = new EventingBasicConsumer(Context.ListenChannel); //创建事件驱动的消费者类型
                consumer.Received += consumer_Received;

                consumer.ConsumerCancelled += (o, e) =>
                 {
                     if (LogLocation.Log.IsNotNull())
                         LogLocation.Log.WriteInfo("SCM.RabbitMQClient", "Consumer cancelled:" + e.ToString());
                     ListenInit();
                 };

                Context.ListenChannel.BasicQos(0, 1, false); //一次只获取一个消息进行消费
                Context.ListenChannel.BasicConsume(Context.ListenQueueName, AutoAck, consumer);
            }
            catch (Exception ex1)
            {
                if (LogLocation.Log.IsNotNull())
                    LogLocation.Log.WriteInfo("LISTENNING ERROR", ex1.Message);
                try
                {
                    Context.ListenConnection = RabbitMqClientFactory.CreateConnection(this.MQHost, this.MQVHost, this.MQUser, this.MQPassword, this.MQPort,this.HeartBeat); //获取连接

                    Context.ListenConnection.ConnectionShutdown += (o, e) =>
                    {
                        if (LogLocation.Log.IsNotNull())
                            LogLocation.Log.WriteInfo("SCM.RabbitMQClient", "connection shutdown:" + e.ReplyText);
                        ListenInit();
                    };

                    Context.ListenChannel = RabbitMqClientFactory.CreateModel(Context.ListenConnection); //获取通道

                    if (NeedDeclare)
                    {
                        Context.ListenChannel.QueueDeclare(queue: Context.ListenQueueName,
                                             durable: true,
                                             exclusive: false,
                                             autoDelete: false,
                                             arguments: null);
                    }

                    var consumer = new EventingBasicConsumer(Context.ListenChannel); //创建事件驱动的消费者类型
                    consumer.Received += consumer_Received;

                    consumer.ConsumerCancelled += (o, e) =>
                    {
                        if (LogLocation.Log.IsNotNull())
                            LogLocation.Log.WriteInfo("SCM.RabbitMQClient", "Consumer cancelled:" + e.ToString());
                        ListenInit();
                    };

                    Context.ListenChannel.BasicQos(0, 1, false); //一次只获取一个消息进行消费
                    Context.ListenChannel.BasicConsume(Context.ListenQueueName, AutoAck, consumer);
                }
                catch (Exception ex2)
                {
                    if (LogLocation.Log.IsNotNull())
                        LogLocation.Log.WriteInfo("LISTENNING ERROR", ex2.Message);
                    try
                    {
                        Context.ListenConnection = RabbitMqClientFactory.CreateConnection(this.MQHost, this.MQVHost, this.MQUser, this.MQPassword, this.MQPort,this.HeartBeat); //获取连接

                        Context.ListenConnection.ConnectionShutdown += (o, e) =>
                        {
                            if (LogLocation.Log.IsNotNull())
                                LogLocation.Log.WriteInfo("SCM.RabbitMQClient", "connection shutdown:" + e.ReplyText);
                            ListenInit();
                        };

                        Context.ListenChannel = RabbitMqClientFactory.CreateModel(Context.ListenConnection); //获取通道

                        if (NeedDeclare)
                        {
                            Context.ListenChannel.QueueDeclare(queue: Context.ListenQueueName,
                                                 durable: true,
                                                 exclusive: false,
                                                 autoDelete: false,
                                                 arguments: null);
                        }

                        var consumer = new EventingBasicConsumer(Context.ListenChannel); //创建事件驱动的消费者类型
                        consumer.Received += consumer_Received;

                        consumer.ConsumerCancelled += (o, e) =>
                        {
                            if (LogLocation.Log.IsNotNull())
                                LogLocation.Log.WriteInfo("SCM.RabbitMQClient", "Consumer cancelled:" + e.ToString());
                            ListenInit();
                        };

                        Context.ListenChannel.BasicQos(0, 1, false); //一次只获取一个消息进行消费
                        Context.ListenChannel.BasicConsume(Context.ListenQueueName, AutoAck, consumer);
                    }
                    catch (Exception ex3)
                    {
                        if (LogLocation.Log.IsNotNull())
                            LogLocation.Log.WriteInfo("LISTENNING ERROR", ex3.Message);
                        if (ThreadExceptionEvents.IsNotNull())
                            ThreadExceptionEvents(ex3);
                    }
                }
            }
        }

        /// <summary>
        /// 接受到消息。
        /// </summary>
        private void consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            EventMessageResult result = null;
            try
            {
                result = EventMessage.BuildEventMessageXMLResult(e.Body, this.EncodingName, e.BasicProperties.CorrelationId, e.BasicProperties.ReplyTo, e.BasicProperties.ContentType); //获取消息返回对象

                if (_actionMessage.IsNotNull())
                {
                    _actionMessage(result); //触发外部侦听事件 
                }
            }
            catch (Exception exception)
            {
                if (LogLocation.Log.IsNotNull())
                    LogLocation.Log.WriteException("SCM.RabbitMQClient", exception);
            }
            finally
            {
                if (result != null && !this.AutoAck)//非自动ACK
                {
                    if (result.IsOperationOk.IsFalse())
                    {
                        //未能消费此消息，重新放入队列头
                        Context.ListenChannel.BasicReject(e.DeliveryTag, true);
                    }
                    else if (Context.ListenChannel.IsClosed.IsFalse())
                    {
                        Context.ListenChannel.BasicAck(e.DeliveryTag, false);
                    }
                }
            }
        }

        #endregion

        #region RPC
        /// <summary>
        /// RPC Client 
        /// 该模式下，发送队列与接收队列用同一套用户/密码/Host
        /// </summary>
        /// <param name="eventMessage"></param>
        /// <param name="exChange"></param>
        /// <param name="queue"></param>
        /// <param name="replyqueue"></param>
        /// <returns></returns>
        public string RpcClient(EventMessage eventMessage, string exChange, string queue, string replyqueue)
        {
            if (!this.SynchronizationFlag)
            {
                throw new Exception("The Current MQ is not synchronous mode.");
            }

            RabbitMQ.Client.IConnection SendConnection = null;
            RabbitMQ.Client.IModel SendChannel = null;

            try
            {
                SendConnection = RabbitMqClientFactory.CreateConnection(this.MQHost, this.MQVHost, this.MQUser, this.MQPassword, this.MQPort,this.HeartBeat); //获取连接
                SendChannel = RabbitMqClientFactory.CreateModel(SendConnection); //获取通道

                if (NeedDeclare)
                {
                    SendChannel.QueueDeclare(queue: queue,
                                                       durable: true,
                                                       exclusive: false,
                                                       autoDelete: false,
                                                       arguments: null);
                    SendChannel.QueueDeclare(queue: replyqueue,
                                                       durable: true,
                                                       exclusive: false,
                                                       autoDelete: false,
                                                       arguments: null);
                }

                var correlationId = eventMessage.EventMessageMarkcode;
                var basicProperties = SendChannel.CreateBasicProperties();
                basicProperties.ReplyTo = replyqueue;
                basicProperties.CorrelationId = correlationId;

                var messageSerializer = MessageSerializerFactory.CreateMessageXMLSerializerInstance(this.EncodingName); //反序列化消息

                SendChannel.BasicPublish(exChange, queue, basicProperties, messageSerializer.SerializerBytes(eventMessage));

                var sw = Stopwatch.StartNew();
                while (true)
                {
                    RabbitMQ.Client.BasicGetResult res = SendChannel.BasicGet(replyqueue, false/*noAck*/);

                    if (res != null && res.BasicProperties.CorrelationId == correlationId)
                    {
                        var result = EventMessage.BuildEventMessageXMLResult(res.Body, this.EncodingName);
                        SendChannel.BasicAck(res.DeliveryTag, false);
                        return MessageSerializerFactory.CreateMessageXMLSerializerInstance(this.EncodingName)
                                .Deserialize(result.MessageBytes);
                    }

                    if (sw.ElapsedMilliseconds > 30000)
                        throw new Exception("等待响应超时");
                }
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
            finally
            {
                if (!SendConnection.IsNull())
                {
                    if (SendConnection.IsOpen)
                        SendConnection.Close();

                    SendConnection.Dispose();
                }
            }
        }
        #endregion

        #region IDispose

        /// <summary>
        /// Dispose.
        /// </summary>
        public void Dispose()
        {
            Context = null;
        }
        #endregion

    }
}
