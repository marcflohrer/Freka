using System.Text.Json;
using Azure.Messaging.ServiceBus;
using OutputProject.Messaging.Message.Generated;
using OutputProject.Messaging.Response.Generated;

namespace OutputProject.Messaging.Message;

public partial class MessageProcessor : IMessageProcessor
{
    // handle received messages
    public async Task ProcessMessageAsync(ProcessMessageEventArgs args)
    {
        string body = args.Message.Body.ToString();
        Models.Message? message = JsonSerializer.Deserialize<Models.Message>(body);
        await Task.Delay(TimeSpan.Zero);
        Console.WriteLine($"{message?.Greeting}, {message?.Name}");
    }

    public void ProcessErrorAsync(ProcessErrorEventArgs processErrorEventArgs)
    {
        Console.WriteLine($"{processErrorEventArgs.Exception.Message}");
    }
}
