using RabbitMQ.Client;
//using SCM.RabbitMQClient.Config;
using System;

namespace SCM.RabbitMQClient
{
    /// <summary>
    /// RabbitMQClient创建工厂。
    /// </summary>
    public class RabbitMqClientFactory
    {
        public static string MqListenQueueName { get; set; }
        public static string MqHostName { get; set; }
        public static string MqvHostName { get; set; }
        public static string MqUserName { get; set; }
        public static string MqPassword { get; set; }
        public static int MqPort { get; set; }

        public delegate void ThreadExceptionEventHandler(Exception ex);

        /// <summary>
        /// 创建一个单例的RabbitMqClient实例。
        /// </summary>
        /// <returns>IRabbitMqClient</returns>
        public static IRabbitMqClient CreateRabbitMqClientXMLInstance()
        {
            var rabbitMqClientContext = new RabbitMqClientContext
            {
                ListenQueueName = RabbitMqClientFactory.MqListenQueueName,//MqConfigDomFactory.CreateConfigDomInstance().MqListenQueueName,
                InstanceCode = Guid.NewGuid().ToString()
            };

            RabbitMqClientXMLSingle.Instance = new RabbitMqClientXMLSingle
            {
                Context = rabbitMqClientContext,
                NeedDeclare = true,
                SynchronizationFlag = false
            };

            return RabbitMqClientXMLSingle.Instance;
        }

        /// <summary>
        /// 创建一个单例的RabbitMqClient实例。
        /// </summary>
        /// <returns>IRabbitMqClient</returns>
        public static IRabbitMqClient CreateRabbitMqClientJSONInstance()
        {
            var rabbitMqClientContext = new RabbitMqClientContext
            {
                ListenQueueName = RabbitMqClientFactory.MqListenQueueName,//MqConfigDomFactory.CreateConfigDomInstance().MqListenQueueName,
                InstanceCode = Guid.NewGuid().ToString()
            };

            RabbitMqClientJSONSingle.Instance = new RabbitMqClientJSONSingle
            {
                Context = rabbitMqClientContext,
                NeedDeclare=true ,
                SynchronizationFlag=false
            };

            return RabbitMqClientJSONSingle.Instance;
        }

        /// <summary>
        /// 创建一个IConnection。
        /// </summary>
        /// <returns></returns>
        internal static IConnection CreateConnection()
        {
            //var mqConfigDom = MqConfigDomFactory.CreateConfigDomInstance(); //获取MQ的配置
            try
            {
                const ushort heartbeat = 60;
                var factory = new ConnectionFactory()
                {
                    HostName = RabbitMqClientFactory.MqHostName,
                    UserName = RabbitMqClientFactory.MqUserName,
                    Password = RabbitMqClientFactory.MqPassword,
                    RequestedHeartbeat = heartbeat, //心跳超时时间
                    AutomaticRecoveryEnabled = true //自动重连
                };

                if (RabbitMqClientFactory.MqvHostName != null && RabbitMqClientFactory.MqvHostName != "")
                    factory.VirtualHost = RabbitMqClientFactory.MqvHostName;
                if (RabbitMqClientFactory.MqPort > 0)
                    factory.Port = RabbitMqClientFactory.MqPort;

                return factory.CreateConnection(); //创建连接对象
            }
            catch (Exception ex1)
            {
                try
                {
                    const ushort heartbeat = 60;
                    var factory = new ConnectionFactory()
                    {
                        HostName = RabbitMqClientFactory.MqHostName,
                        UserName = RabbitMqClientFactory.MqUserName,
                        Password = RabbitMqClientFactory.MqPassword,
                        RequestedHeartbeat = heartbeat, //心跳超时时间
                        AutomaticRecoveryEnabled = true //自动重连
                    };

                    if (RabbitMqClientFactory.MqvHostName != null && RabbitMqClientFactory.MqvHostName != "")
                        factory.VirtualHost = RabbitMqClientFactory.MqvHostName;
                    if (RabbitMqClientFactory.MqPort > 0)
                        factory.Port = RabbitMqClientFactory.MqPort;

                    return factory.CreateConnection(); //创建连接对象
                }
                catch (Exception ex2)
                {
                    try
                    {
                        const ushort heartbeat = 60;
                        var factory = new ConnectionFactory()
                        {
                            HostName = RabbitMqClientFactory.MqHostName,
                            UserName = RabbitMqClientFactory.MqUserName,
                            Password = RabbitMqClientFactory.MqPassword,
                            RequestedHeartbeat = heartbeat, //心跳超时时间
                            AutomaticRecoveryEnabled = true //自动重连
                        };

                        if (RabbitMqClientFactory.MqvHostName != null && RabbitMqClientFactory.MqvHostName != "")
                            factory.VirtualHost = RabbitMqClientFactory.MqvHostName;
                        if (RabbitMqClientFactory.MqPort > 0)
                            factory.Port = RabbitMqClientFactory.MqPort;

                        return factory.CreateConnection(); //创建连接对象
                    }
                    catch (Exception ex3)
                    {
                        throw ex3;
                    }
                }
            }
            
        }

        /// <summary>
        /// 创建一个IConnection。
        /// </summary>
        /// <returns></returns>
        internal static IConnection CreateConnection(string host,string vhost,string user,string password,int port)
        {
            //var mqConfigDom = MqConfigDomFactory.CreateConfigDomInstance(); //获取MQ的配置
            try
            {
                const ushort heartbeat = 60;
                var factory = new ConnectionFactory()
                {
                    HostName = host,
                    UserName = user,
                    Password = password,
                    RequestedHeartbeat = heartbeat, //心跳超时时间
                    AutomaticRecoveryEnabled = true //自动重连
                };

                if (vhost != null && vhost != "")
                    factory.VirtualHost = vhost;
                if (port > 0)
                    factory.Port = port;

                return factory.CreateConnection(); //创建连接对象
            }
            catch (Exception ex1)
            {
                try
                {
                    const ushort heartbeat = 60;
                    var factory = new ConnectionFactory()
                    {
                        HostName = host,
                        UserName = user,
                        Password = password,
                        RequestedHeartbeat = heartbeat, //心跳超时时间
                        AutomaticRecoveryEnabled = true //自动重连
                    };

                    if (vhost != null && vhost != "")
                        factory.VirtualHost = vhost;
                    if (port > 0)
                        factory.Port = port;

                    return factory.CreateConnection(); //创建连接对象
                }
                catch (Exception ex2)
                {
                    try
                    {
                        const ushort heartbeat = 60;
                        var factory = new ConnectionFactory()
                        {
                            HostName = host,
                            UserName = user,
                            Password = password,
                            RequestedHeartbeat = heartbeat, //心跳超时时间
                            AutomaticRecoveryEnabled = true //自动重连
                        };

                        if (vhost != null && vhost != "")
                            factory.VirtualHost = vhost;
                        if (port > 0)
                            factory.Port = port;

                        return factory.CreateConnection(); //创建连接对象
                    }
                    catch (Exception ex3)
                    {
                        throw ex3;
                    }
                }
            }
            
        }

        /// <summary>
        /// 创建一个IModel。
        /// </summary>
        /// <param name="connection">IConnection.</param>
        /// <returns></returns>
        internal static IModel CreateModel(IConnection connection)
        {
            return connection.CreateModel(); //创建通道
        }
    }
}