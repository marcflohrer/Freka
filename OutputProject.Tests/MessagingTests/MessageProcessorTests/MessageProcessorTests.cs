// Inspired by: https://github.com/andrewjpoole/azure-messaging-servicebus-handler-tests
using OutputProject.Tests.ServiceBusTests.MessageProcessorTests.TestElements;

namespace OutputProject.Tests.MessageProcessorTests;

public class MessageProcessorTests
{
    [Fact]
    public void a_message_sent_to_the_queue_is_handled_and_completed()
    {
        using var host = new TestHost();

        Given.OnThe(host)
            .ATestPayloadIsGenerated(out var testPayload);

        When.OnThe(host)
            .AMessageIsSentToInputQueue(testPayload);

        Then.OnThe(host)
            .TheMessageProcessorWasCalled(times: 1)
            .And().AMessageWasSentToOutputQueue()
            .And().TheMessageWasCompleted();
    }

    [Fact]
    public void a_message_sent_to_the_queue_is_handled_and_when_permanentException_is_thrown_is_dead_lettered()
    {
        using var host = new TestHost();

        Given.OnThe(host)
            .ATestPayloadIsGenerated(out var testPayload)
            .And().TheMessageProcessorWillThrowAPermanentException();

        When.OnThe(host)
            .AMessageIsSentToInputQueue(testPayload);

        Then.OnThe(host)
            .AMessageWasSentToOutputQueue(times: 0)
            .And().TheMessageWasDeadLettered();
    }

    [Fact]
    public void a_message_sent_to_the_queue_is_handled_and_when_transientException_is_thrown_is_retried_and_is_eventually_completed()
    {
        using var host = new TestHost();

        Given.OnThe(host)
            .ATestPayloadIsGenerated(out var testPayload)
            .And().TheMessageProcessorWillThrowANumberOfTransientExceptions(times: 3);

        When.OnThe(host)
            .AMessageIsSentToInputQueue(testPayload, simulateRetries: true);

        Then.OnThe(host)
            .TheMessageWasRetried(times: 4)
            .And().TheMessageProcessorWasCalled(times: 4)
            .And().AMessageWasSentToOutputQueue(times: 1)
            .And().TheMessageWasCompleted();
    }

    [Fact]
    public void a_message_sent_to_the_queue_is_handled_and_when_5_transientExceptions_are_thrown_is_eventually_deadlettered()
    {
        using var host = new TestHost();

        Given.OnThe(host)
            .ATestPayloadIsGenerated(out var testPayload)
            .And().TheMessageProcessorWillThrowANumberOfTransientExceptions(times: 5);

        When.OnThe(host)
            .AMessageIsSentToInputQueue(testPayload, simulateRetries: true);

        Then.OnThe(host)
            .TheMessageWasRetried(times: 5)
            .And().AMessageWasSentToOutputQueue(times: 0)
            .And().TheMessageWasDeadLettered();
    }
}

