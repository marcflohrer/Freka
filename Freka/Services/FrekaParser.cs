using Freka.Services.Models;

namespace Freka.Services;

public static class FrekaParser
{
    public static FrekaFile? Parse(string pathToFrekaFile)
    {
        string frekaJson = File.ReadAllText(pathToFrekaFile);
        return System.Text.Json.JsonSerializer.Deserialize<FrekaFile>(frekaJson);
    }
}

