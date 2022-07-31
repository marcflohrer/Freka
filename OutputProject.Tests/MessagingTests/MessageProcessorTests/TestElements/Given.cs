// Inspired by: https://github.com/andrewjpoole/azure-messaging-servicebus-handler-tests
using Azure.Messaging.ServiceBus;
using Moq;
using OutputProject.Messaging;

namespace OutputProject.Tests.ServiceBusTests.MessageProcessorTests.TestElements;

public class Given
{
    private readonly TestHost _host;

    public Given(TestHost host)
    {
        _host = host;
    }

    public static Given OnThe(TestHost host) => new(host);
    public Given And() => this;

    public Given ATestPayloadIsGenerated(out string testPayload)
    {
        testPayload = Guid.NewGuid().ToString();

        return this;
    }

    public Given TheMessageProcessorWillThrowAPermanentException()
    {
        _host.MessageProcessor.Setup(x => x.ProcessMessageAsync(It.IsAny<ProcessMessageEventArgs>()))
            .ThrowsAsync(new PermanentException("something permanent has gone wrong!"));

        return this;
    }

    public Given TheMessageProcessorWillThrowANumberOfTransientExceptions(int times = 1)
    {
        var sequence = _host.MessageProcessor
                 .SetupSequence(x => x.ProcessMessageAsync(It.IsAny<ProcessMessageEventArgs>()));
        for (int i = 0; i < times; i++)
        {
            sequence.ThrowsAsync(new Exception("something transient has gone wrong!"));
        }
        sequence.Returns(Task.CompletedTask);

        return this;
    }
}

