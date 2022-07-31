namespace Freka.Services.Extensions;

public static class DirectoryInfoExtensions
{
    #region Basic extensions
    public static DirectoryInfo CreateIfNotExists(this DirectoryInfo directory)
    {
        try
        {
            if (!directory.Exists)
            {
                directory.Create();
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Error creating directory {directory.FullName} {ex}");
        }
        return directory;
    }
    #endregion
}

