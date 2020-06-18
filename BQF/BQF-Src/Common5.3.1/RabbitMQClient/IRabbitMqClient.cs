using System;

namespace SCM.RabbitMQClient
{
    /// <summary>
    /// RabbitMq client 接口。
    /// </summary>
    public interface IRabbitMqClient : IDisposable
    {
        /// <summary>
        /// 编码名称
        /// </summary>
        String EncodingName { get; set; }

        /// <summary>
        /// 是否需要声明队列
        /// </summary>
        bool NeedDeclare { get; set; }

        bool AutoAck { get; set; }

        ushort HeartBeat { get; set; }

        /// <summary>
        /// 同步标识
        /// 即RPC模式标识
        /// </summary>
        bool SynchronizationFlag { get; set; }

        /// <summary>
        /// RabbitMqClient 数据上下文。
        /// </summary>
        RabbitMqClientContext Context { get; set; }

        /// <summary>
        /// 消息被本地激活事件。通过绑定该事件来获取消息队列推送过来的消息。只能绑定一个事件处理程序。
        /// </summary>
        event ActionEvent ActionEventMessage;

        /// <summary>
        /// 触发一个事件，向队列推送一个事件消息。
        /// </summary>
        /// <param name="eventMessage">消息类型实例</param>
        /// <param name="exChange">Exchange名称</param>
        /// <param name="queue">队列名称</param>
        /// <param name="replyto"></param>
        /// <param name="corelationid"></param>
        /// <param name="contenttype"></param>
        void TriggerEventMessage(EventMessage eventMessage, string exChange, string queue, string replyto = "", string corelationid = "", string contenttype = "");

        /// <summary>
        /// 开始消息队列的默认监听。
        /// </summary>
        void OnListening();

        /// <summary>
        /// RPC Client
        /// 设定同步模式
        /// 在服务端搭建监听服务，与回复消息函数
        /// </summary>
        /// <param name="eventMessage"></param>
        /// <param name="exChange"></param>
        /// <param name="queue"></param>
        /// <param name="replyqueue"></param>
        /// <returns></returns>
        string RpcClient(EventMessage eventMessage, string exChange, string queue, string replyqueue);

        event RabbitMqClientFactory.ThreadExceptionEventHandler ThreadExceptionEvents;
    }
}