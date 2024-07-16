namespace MessageBus
{
    public interface IMessageBus
    {
        Task PublisMessage(object message, string topic_queue_Name);
    }
}

