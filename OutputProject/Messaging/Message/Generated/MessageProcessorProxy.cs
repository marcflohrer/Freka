using Azure.Messaging.ServiceBus;
using OutputProject.Messaging.Response.Generated;

namespace OutputProject.Messaging.Message.Generated;

public class MessageProcessorProxy : IMessageProcessorProxy
{
    public IMessageProcessor MessageProcessor { get; }
    public IResponseSender ResponseSender { get; }

    private int _initialBackoffInMillis = 2000;

    public MessageProcessorProxy(IMessageProcessor messageProcessor, IResponseSender responseSender)
    {
        MessageProcessor = messageProcessor;
        ResponseSender = responseSender;
    }
    // handle received messages
    public async Task ProcessMessageAsync(ProcessMessageEventArgs args)
    {
        try
        {
            await MessageProcessor.ProcessMessageAsync(args);
            // complete the message. message is deleted from the queue. 
            await args.CompleteMessageAsync(args.Message);
            // send another message
            await ResponseSender.SendMessageAsync(new Response.Models.Response
            {
                IsSuccessful = true,
                ErrorMessage = string.Empty
            });
        }
        catch (PermanentException permanentException)
        {
            Console.WriteLine(permanentException.Message);
            // complete the message. message is deleted from the queue. 
            await args.DeadLetterMessageAsync(args.Message, string.Empty, string.Empty, CancellationToken.None);
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception.Message);

            // complete the message. message is deleted from the queue. 
            await args.AbandonMessageAsync(args.Message, null, CancellationToken.None);
        }
    }

    // handle any errors when receiving messages
    public Task ProcessErrorAsync(ProcessErrorEventArgs args)
    {
        MessageProcessor.ProcessErrorAsync(args);
        return Task.CompletedTask;
    }
}

