using Freka.Services.Extensions;
using Freka.Services.Generators;

namespace Freka.Services.Base
{
    public class InputQueueSourceGenBase
    {
        public FileInfo SourceFile { get; set; }
        public string? SourceCode { get; set; }
        public MessageQueueInfo ModelAndQueueNames { get; }

        public InputQueueSourceGenBase(DirectoryInfo directory, MessageQueueInfo messageQueueInfo, string fileName)
        {
            SourceFile = new FileInfo(Path.Join(directory.FullName, $"{fileName}.cs"));
            ModelAndQueueNames = messageQueueInfo;
        }

        public IInputQueueSourceCodeGen WriteToFile()
        {
            if (SourceFile != null)
            {
                SourceCode = ((IInputQueueSourceCodeGen)this).GenerateSourceCode(ModelAndQueueNames);
                if (SourceCode == null)
                {
                    throw new ArgumentException("SourceCode is null");
                }
                File.WriteAllText(SourceFile.FullName, SourceCode);
                return (IInputQueueSourceCodeGen)this;
            }
            else
            {
                throw new ArgumentException("SourceFile is null");
            }
        }
    }
}

