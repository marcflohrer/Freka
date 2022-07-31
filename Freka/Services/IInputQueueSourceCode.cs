using Freka.Services.Generators;

namespace Freka.Services
{
    public interface IInputQueueSourceCodeGen
    {
        public FileInfo SourceFile { get; set; }

        public string? SourceCode { get; set; }

        public string GenerateSourceCode(MessageQueueInfo messageQueueInfo);
    }
}