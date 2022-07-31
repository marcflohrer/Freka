using Freka.Services.Extensions;
using Freka.Services.Generators;

namespace Freka.Services;

public interface IStaticSourceCodeGen
{
    public FileInfo SourceFile { get; set; }

    public string? SourceCode { get; set; }

    public string GenerateSourceCode(IReadOnlyList<MessageQueueInfo> messageQueueInfoList);

    public IStaticSourceCodeGen WriteToFile(IReadOnlyList<MessageQueueInfo> messageQueueInfoList)
    {
        _ = SourceFile ?? throw new ArgumentException("SourceFile is null");

        SourceFile.Directory!.CreateIfNotExists();
        SourceFile.CreateIfNotExists();
        SourceCode = GenerateSourceCode(messageQueueInfoList);
        File.WriteAllText(SourceFile.FullName, SourceCode);
        return this;
    }
}
