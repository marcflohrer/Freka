// Inspired by: https://github.com/andrewjpoole/azure-messaging-servicebus-handler-tests
namespace OutputProject.Tests.ServiceBusTests.TestFramework.ServiceBusMessageHandler
{
    public class TestQueueMessage
    {
        public string Message { get; }

        public TestQueueMessage(string message)
        {
            Message = message;
        }
    }
}

