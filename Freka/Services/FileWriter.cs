namespace Freka.Services;

public static class FileWriter
{
    public static void WriteAllText(this FileInfo fileInfo, string content)
    {
        File.WriteAllText(fileInfo.FullName, content);
    }
}
