using System.Text.Json;

namespace SteamAccountsHub.Core.Utils.Files;

public static class FileManager
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        Converters = { new DecryptedStringJsonConverter() },
    };

    private static readonly JsonSerializerOptions SahOptions = new()
    {
        WriteIndented = true,
        Converters = { new EncryptedStringJsonConverter() },
    };

    private static JsonSerializerOptions GetOptionsByFileType(string filePath)
        => Path.GetExtension(filePath).ToLower() switch
        {
            ".json" => JsonOptions,
            ".sah" => SahOptions,
            _ => throw new NotImplementedException("Not implemented file type."),
        };

    public static T? Load<T>(string filePath) where T : class
    {
        if (File.Exists(filePath) == false)
            return default;
        try
        {
            using FileStream stream = File.OpenRead(filePath);
            T? result = JsonSerializer.Deserialize<T>(stream, GetOptionsByFileType(filePath));
            stream.Close();
            return result;
        }
        catch
        {
            return default;
        }
    }

    public static void Save<T>(T obj, string filePath) where T : class
    {
        try
        {
            using FileStream stream = File.Create(filePath);
            JsonSerializer.Serialize(stream, obj, GetOptionsByFileType(filePath));
            stream.Close();
        }
        catch { }
    }
}