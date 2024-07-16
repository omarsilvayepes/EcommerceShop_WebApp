using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using System.Text;

namespace MessageBus
{
    public class MessageBus:IMessageBus
    {
        public async Task PublisMessage(object message, string topic_queue_Name)
        {
            await using var client=new ServiceBusClient("my_conection_string");
            ServiceBusSender sender=client.CreateSender(topic_queue_Name);

            ServiceBusMessage serviceBusMessage = new ServiceBusMessage(
                Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message)))
            {
                CorrelationId=Guid.NewGuid().ToString()
            };

            await sender.SendMessageAsync(serviceBusMessage);
            await client.DisposeAsync();
        }
    }
}
