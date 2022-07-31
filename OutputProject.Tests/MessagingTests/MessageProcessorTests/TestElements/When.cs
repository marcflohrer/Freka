// Inspired by: https://github.com/andrewjpoole/azure-messaging-servicebus-handler-tests
using OutputProject.Tests.ServiceBusTests.TestFramework.ServiceBusMessageHandler;

namespace OutputProject.Tests.ServiceBusTests.MessageProcessorTests.TestElements;

public class When
{
    private readonly TestHost _host;

    public When(TestHost host)
    {
        _host = host;
    }

    public static When OnThe(TestHost host) => new(host);
    public When And() => this;

    public When AMessageIsSentToInputQueue(string testPayload, bool simulateRetries = false)
    {
        var payload = new TestQueueMessage(testPayload);

        if (simulateRetries)
        {
            _host.TestableInputQueueMessageProcessor.SendMessageWithRetries(payload, _host.NumberOfSimulatedServiceBusMessageRetries).GetAwaiter().GetResult();
        }
        else
        {
            var task = _host.TestableInputQueueMessageProcessor.SendMessage(payload);
            task.GetAwaiter().GetResult();
        }

        return this;
    }
}

