// Inspired by: https://github.com/andrewjpoole/azure-messaging-servicebus-handler-tests
using Azure.Messaging.ServiceBus;
using OutputProject.Messaging;
using OutputProject.Messaging.Message.Generated;
using OutputProject.Messaging.Response.Generated;
using OutputProject.Messaging.Response.Models;

namespace OutputProject.Tests.ServiceBusTests.TestFramework.ServiceBusMessageHandler
{
    public class TestQueueHandler : ServiceBusMessageHandlerWorker
    {
        private readonly IMessageProcessor messageProcessor;
        private readonly IResponseSender _testQueue2Sender;
        private const string QueueName = "testQueue1";

        public TestQueueHandler(
            ServiceBusClient serviceBusClient,
            IMessageProcessor messageProcessor,
            IResponseSender responseSender,
            int intInitialBackoffInMs = 250)
            : base(serviceBusClient, QueueName, intInitialBackoffInMs)
        {
            this.messageProcessor = messageProcessor;
            _testQueue2Sender = responseSender;
        }


        public override async Task OnReceiveMessage(ProcessMessageEventArgs args)
        {
            try
            {
                await messageProcessor.ProcessMessageAsync(args); // simulate some processing in a service

                await _testQueue2Sender.SendMessageAsync(new Response
                {
                    ErrorMessage = string.Empty,
                    IsSuccessful = true
                });
            }
            catch (Exception ex)
            {
                throw new PermanentException("Payload is null!", ex);
            }
        }
    }
}

