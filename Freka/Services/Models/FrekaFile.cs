namespace Freka.Services.Models
{

    public partial class FrekaFile
    {
        public string? ProjectName { get; set; }
        public Messaging? Messaging { get; set; }
    }

    public partial class Messaging
    {
        public string? ClientName { get; set; }
        public InputQueue[]? Input { get; set; }
    }

    public partial class Queue
    {
        public string? QueueName { get; set; }
        public string? MessageName { get; set; }
        public string? Type { get; set; }
    }

    public partial class InputQueue : Queue
    {
        public Queue? Output { get; set; }
    }
}
