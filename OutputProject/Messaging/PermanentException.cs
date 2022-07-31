// Inspired by: https://github.com/andrewjpoole/azure-messaging-servicebus-handler-tests

namespace OutputProject.Messaging;

public class PermanentException : Exception
{
    public PermanentException(string message) : base(message)
    {
    }

    public PermanentException(string message, Exception innerException) : base(message, innerException)
    {
    }
}

