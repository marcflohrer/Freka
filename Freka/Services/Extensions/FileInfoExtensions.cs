namespace Freka.Services.Extensions;

public static class FileInfoExtensions
{
    public static FileInfo CreateIfNotExists(this FileInfo fileInfo)
    {
        try
        {
            if (!fileInfo.Exists)
            {
                fileInfo.Create();
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Error creating file {fileInfo.FullName} {ex}");
        }
        return fileInfo;
    }
}

