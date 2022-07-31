using Freka.Services.Base;

namespace Freka.Services.Generators.Tests;

public class ProcessorTestGenerator : InputQueueSourceGenBase, IInputQueueSourceCodeGen
{
    public ProcessorTestGenerator(DirectoryInfo sourceDirectory, MessageQueueInfo messageQueueInfo)
        : base(sourceDirectory, messageQueueInfo, $"{messageQueueInfo.Model}ProcessorTests") { }

    public string GenerateSourceCode(MessageQueueInfo messageQueueInfo)
    {
        var model = messageQueueInfo.Model;
        return $@"// Inspired by: https://github.com/andrewjpoole/azure-messaging-servicebus-handler-tests
using OutputProject.Tests.ServiceBusTests.{model}ProcessorTests.TestElements;

namespace OutputProject.Tests.{model}ProcessorTests;

public class {model}ProcessorTests
{{
    [Fact]
    public void a_{model.ToLower()}_sent_to_the_queue_is_handled_and_completed()
    {{
        using var host = new TestHost();

        Given.OnThe(host)
            .ATestPayloadIsGenerated(out var testPayload);

        When.OnThe(host)
            .A{model}IsSentToInputQueue(testPayload);

        Then.OnThe(host)
            .The{model}ProcessorWasCalled(times: 1)
            .And().A{model}WasSentToOutputQueue()
            .And().The{model}WasCompleted();
    }}

    [Fact]
    public void a_{model.ToLower()}_sent_to_the_queue_is_handled_and_when_permanentException_is_thrown_is_dead_lettered()
    {{
        using var host = new TestHost();

        Given.OnThe(host)
            .ATestPayloadIsGenerated(out var testPayload)
            .And().The{model}ProcessorWillThrowAPermanentException();

        When.OnThe(host)
            .A{model}IsSentToInputQueue(testPayload);

        Then.OnThe(host)
            .A{model}WasSentToOutputQueue(times: 0)
            .And().The{model}WasDeadLettered();
    }}

    [Fact]
    public void a_{model.ToLower()}_sent_to_the_queue_is_handled_and_when_transientException_is_thrown_is_retried_and_is_eventually_completed()
    {{
        using var host = new TestHost();

        Given.OnThe(host)
            .ATestPayloadIsGenerated(out var testPayload)
            .And().The{model}ProcessorWillThrowANumberOfTransientExceptions(times: 3);

        When.OnThe(host)
            .A{model}IsSentToInputQueue(testPayload, simulateRetries: true);

        Then.OnThe(host)
            .The{model}WasRetried(times: 4)
            .And().The{model}ProcessorWasCalled(times: 4)
            .And().A{model}WasSentToOutputQueue(times: 1)
            .And().The{model}WasCompleted();
    }}

    [Fact]
    public void a_{model.ToLower()}_sent_to_the_queue_is_handled_and_when_5_transientExceptions_are_thrown_is_eventually_deadlettered()
    {{
        using var host = new TestHost();

        Given.OnThe(host)
            .ATestPayloadIsGenerated(out var testPayload)
            .And().The{model}ProcessorWillThrowANumberOfTransientExceptions(times: 5);

        When.OnThe(host)
            .A{model}IsSentToInputQueue(testPayload, simulateRetries: true);

        Then.OnThe(host)
            .The{model}WasRetried(times: 5)
            .And().A{model}WasSentToOutputQueue(times: 0)
            .And().The{model}WasDeadLettered();
    }}
}}

";
    }
}

