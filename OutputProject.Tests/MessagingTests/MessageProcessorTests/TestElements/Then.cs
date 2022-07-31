// Inspired by: https://github.com/andrewjpoole/azure-messaging-servicebus-handler-tests
using Azure.Messaging.ServiceBus;
using FluentAssertions;
using Moq;

namespace OutputProject.Tests.ServiceBusTests.MessageProcessorTests.TestElements;

public class Then
{
    private readonly TestHost _host;

    public Then(TestHost host)
    {
        _host = host;
    }

    public static Then OnThe(TestHost host) => new(host);
    public Then And() => this;

    public Then TheMessageWasCompleted()
    {
        _host.TestableInputQueueMessageProcessor.MessageDeliveryAttempts.Last().WasCompleted.Should().BeTrue();
        return this;
    }

    public Then TheMessageWasDeadLettered()
    {
        _host.TestableInputQueueMessageProcessor.MessageDeliveryAttempts.Last().WasDeadLettered.Should().BeTrue();
        return this;
    }

    public Then TheMessageWasRetried(int times)
    {
        _host.TestableInputQueueMessageProcessor.MessageDeliveryAttempts.Count.Should().Be(times);
        return this;
    }

    public Then TheMessageProcessorWasCalled(int times)
    {
        _host.MessageProcessor.Verify(x => x.ProcessMessageAsync(It.IsAny<ProcessMessageEventArgs>()), Times.Exactly(times));
        return this;
    }

    public Then AMessageWasSentToOutputQueue(int times = 1)
    {
        _host.TestableOutputQueueResponseSender.Verify(x => x.SendMessageAsync(It.IsAny<ServiceBusMessage>(), It.IsAny<CancellationToken>()), Times.Exactly(times));
        return this;
    }
}

