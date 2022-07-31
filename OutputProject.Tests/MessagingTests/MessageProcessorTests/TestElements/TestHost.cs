// Inspired by:https://github.com/andrewjpoole/azure-messaging-servicebus-handler-tests
using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;
using OutputProject.Messaging;
using OutputProject.Messaging.Message.Generated;
using OutputProject.Tests.ServiceBusTests.TestFramework.AzureMessagingServiceBusTestSupport;

namespace OutputProject.Tests.ServiceBusTests.MessageProcessorTests.TestElements;

public class TestHost : IDisposable
{
    private readonly IHost _server;
    public Mock<ServiceBusSender> TestableOutputQueueResponseSender { get; } = new();
    public TestableServiceBusProcessor TestableInputQueueMessageProcessor { get; } = new();
    public Mock<IMessageProcessor> MessageProcessor { get; } = new();
    public IServiceProvider Services => _server.Services;
    public int NumberOfSimulatedServiceBusMessageRetries = 5;
    public int InitialRetryDelayForServiceBusMessageRetriesInMs = 100;

    public TestHost()
    {
        var builder = new HostBuilder()
            .ConfigureWebHost(webHost =>
            {
                webHost.UseTestServer()
                    .ConfigureServices((context, services) => ServiceBusExtensions.AddMessagingServices(services, context.Configuration))
                    .ConfigureTestServices((services) =>
                    {
                        var client = new Mock<ServiceBusClient>();

                        client.Setup(t => t.CreateSender(It.Is<string>(s => s == "Response")))
                            .Returns(TestableOutputQueueResponseSender.Object);

                        client.Setup(t => t.CreateProcessor(It.Is<string>(s => s == "Message"), It.IsAny<ServiceBusProcessorOptions>()))
                            .Returns(TestableInputQueueMessageProcessor);

                        var azureClientFactory = new Mock<IAzureClientFactory<ServiceBusClient>>();
                        azureClientFactory.Setup(mock => mock.CreateClient("ServiceBusClient")).Returns(client.Object);
                        services.AddSingleton(azureClientFactory.Object);

                        // register other test services here...
                        services.AddSingleton(MessageProcessor.Object);

                    }).Configure(app => { });
            });

        _server = builder.Build();
        _server.StartAsync();
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        _server?.StopAsync().GetAwaiter().GetResult();
        _server?.Dispose();
    }
}

