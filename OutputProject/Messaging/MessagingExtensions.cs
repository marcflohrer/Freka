using Microsoft.Extensions.Azure;
using OutputProject.Messaging.Message;
using OutputProject.Messaging.Message.Generated;
using OutputProject.Messaging.Response.Generated;

namespace OutputProject.Messaging;

public static class ServiceBusExtensions
{
    private static IServiceCollection AddServiceBus(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAzureClients(clientsBuilder =>
        {
            clientsBuilder.AddServiceBusClient(configuration.GetSection("ServiceBus").GetValue<string>("ConnectionString"))
              // (Optional) Provide name for instance to retrieve by with DI
              .WithName("ServiceBusClient")
              // (Optional) Override ServiceBusClientOptions (e.g. change retry settings)
              .ConfigureOptions(options =>
              {
                  options.RetryOptions.Delay = TimeSpan.FromMilliseconds(50);
                  options.RetryOptions.MaxDelay = TimeSpan.FromSeconds(30);
                  options.RetryOptions.MaxRetries = 30;
              });
        });
        return services;
    }

    private static IServiceCollection AddMessageProcessor(this IServiceCollection services)
    {
        services.AddSingleton<IMessageProcessorProxy, MessageProcessorProxy>();
        services.AddSingleton<IMessageProcessor, MessageProcessor>();
        services.AddSingleton<MessageQueue>();
        services.AddHostedService<MessageWorker>();
        services.AddSingleton<IResponseSender, ResponseSender>();
        return services.AddSingleton<IResponseSenderProxy, ResponseSenderProxy>();
    }

    public static IServiceCollection AddMessagingServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddServiceBus(configuration);
        services.AddMessageProcessor();
        return services;
    }
}

