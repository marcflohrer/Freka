using Freka.Services.Base;

namespace Freka.Services.Generators.Tests;

public class ThenGenerator : InputQueueSourceGenBase, IInputQueueSourceCodeGen
{
    public ThenGenerator(DirectoryInfo sourceDirectory, MessageQueueInfo messageQueueInfo)
        : base(sourceDirectory, messageQueueInfo, $"Then") { }

    public string GenerateSourceCode(MessageQueueInfo messageQueueInfo)
    {
        var model = messageQueueInfo.Model;
        return $@"// Inspired by: https://github.com/andrewjpoole/azure-messaging-servicebus-handler-tests
using Azure.Messaging.ServiceBus;
using FluentAssertions;
using Moq;

namespace OutputProject.Tests.ServiceBusTests.{model}ProcessorTests.TestElements;

public class Then
{{
    private readonly TestHost _host;

    public Then(TestHost host)
    {{
        _host = host;
    }}

    public static Then OnThe(TestHost host) => new(host);
    public Then And() => this;

    public Then The{model}WasCompleted()
    {{
        _host.TestableInputQueueMessageProcessor.MessageDeliveryAttempts.Last().WasCompleted.Should().BeTrue();
        return this;
    }}

    public Then The{model}WasDeadLettered()
    {{
        _host.TestableInputQueueMessageProcessor.MessageDeliveryAttempts.Last().WasDeadLettered.Should().BeTrue();
        return this;
    }}

    public Then The{model}WasRetried(int times)
    {{
        _host.TestableInputQueueMessageProcessor.MessageDeliveryAttempts.Count.Should().Be(times);
        return this;
    }}

    public Then The{model}ProcessorWasCalled(int times)
    {{
        _host.{model}Processor.Verify(x => x.ProcessMessageAsync(It.IsAny<ProcessMessageEventArgs>()), Times.Exactly(times));
        return this;
    }}

    public Then A{model}WasSentToOutputQueue(int times = 1)
    {{
        _host.TestableOutputQueueResponseSender.Verify(x => x.SendMessageAsync(It.IsAny<ServiceBusMessage>(), It.IsAny<CancellationToken>()), Times.Exactly(times));
        return this;
    }}
}}

";
    }
}

