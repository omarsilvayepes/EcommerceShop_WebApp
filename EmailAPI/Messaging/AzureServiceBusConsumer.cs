using Azure.Messaging.ServiceBus;
using EmailAPI.Models.Dto;
using EmailAPI.Services;
using Newtonsoft.Json;
using System.Text;

namespace EmailAPI.Messaging
{
    public class AzureServiceBusConsumer:IAzureServiceBusConsumer
    {
        private readonly string serviceBusConnectionString;
        private readonly string emailCartQueue;
        private readonly IConfiguration _configuration;
        private ServiceBusProcessor _emailCartProcessor;
        private readonly EmailService _emailService;

        public AzureServiceBusConsumer(IConfiguration configuration, EmailService emailService)
        {
            _configuration = configuration;
            serviceBusConnectionString = _configuration.GetValue<string>("ServiceBusConnectionString");
            emailCartQueue = _configuration.GetValue<string>("TopicAndQueueNames:EmailShoppingCartQueue");

            var client = new ServiceBusClient(serviceBusConnectionString);
            _emailCartProcessor = client.CreateProcessor(emailCartQueue);
            _emailService = emailService;
        }

        public async Task Start()
        {
            _emailCartProcessor.ProcessMessageAsync += _emailCartProcessor_ProcessMessageAsync;
            _emailCartProcessor.ProcessErrorAsync += _emailCartProcessor_ProcessErrorAsync;
            await _emailCartProcessor.StartProcessingAsync();
        }

        public async Task Stop()
        {
           await _emailCartProcessor.StopProcessingAsync();
            await _emailCartProcessor.DisposeAsync();
        }

        private Task _emailCartProcessor_ProcessErrorAsync(ProcessErrorEventArgs arg)
        {
            Console.WriteLine(arg.Exception.ToString());
            return Task.CompletedTask;
        }

        private async Task _emailCartProcessor_ProcessMessageAsync(ProcessMessageEventArgs arg)
        {
            //Receive the messages
            var message=arg.Message;
            var body = Encoding.UTF8.GetString(message.Body);

            CartDto cartDto=JsonConvert.DeserializeObject<CartDto>(body);

            try
            {
                //TODO:Send Email

                //Log on DB

                await _emailService.EmailCartAndLog(cartDto);

                await arg.CompleteMessageAsync(arg.Message);
            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}
