using Freka.Services.Extensions;

namespace Freka.Services.Base
{
    public class StaticSourceCodeGenBase
    {
        public FileInfo SourceFile { get; set; }
        public string? SourceCode { get; set; }

        public StaticSourceCodeGenBase(DirectoryInfo sourceDirectory, string fileName)
        {
            SourceFile = new FileInfo(Path.Join(sourceDirectory.FullName, $"{fileName}.cs"));
        }
    }
}

