namespace SCM.RabbitMQClient
{
    /// <summary>
    /// 事件消息返回类型
    /// </summary>
    public class EventMessageResult
    {
        /// <summary>
        /// 完整消息对象，此对象是直接在MQ队列中传输的类型。
        /// </summary>
        public EventMessage EventMessageBytes { get; set; }

        /// <summary>
        /// 原始消息的bytes。
        /// </summary>
        public byte[] MessageBytes { get; set; }

        /// <summary>
        /// 消息处理是否成功
        /// </summary>
        public bool IsOperationOk { get; set; }

        /// <summary>
        /// 关联ID
        /// BasicProperties对象属性
        /// </summary>
        public string CorrelationId { get; set; }

        /// <summary>
        /// 内容类型
        /// BasicProperties对象属性
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// ReplyTo
        /// BasicProperties对象属性
        /// </summary>
        public string ReplyTo { get; set; }
    }
}