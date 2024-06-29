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

    private static readonly Dictionary<FileExtension, JsonSerializerOptions> Options = new()
    {
        { FileExtension.Json, JsonOptions },
        { FileExtension.Sah, SahOptions },
    };

    public static FileExtension GetFileExtension(string filePath)
        => Path.GetExtension(filePath).ToLower() switch
        {
            ".json" => FileExtension.Json,
            ".sah" => FileExtension.Sah,
            _ => throw new NotImplementedException("Not implemented file extension."),
        };

    public static T? Load<T>(string filePath) where T : class
    {
        if (File.Exists(filePath) == false)
            return default;
        try
        {
            using FileStream stream = File.OpenRead(filePath);
            T? result = JsonSerializer.Deserialize<T>(stream, Options[GetFileExtension(filePath)]);
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
            JsonSerializer.Serialize(stream, obj, Options[GetFileExtension(filePath)]);
            stream.Close();
        }
        catch { }
    }

    public static void Copy(string sourceFileName, string targetFileName)
    {
        if (File.Exists(sourceFileName) == false)
            return;

        try
        {
            File.Copy(sourceFileName, targetFileName);
        }
        catch { }
    }
}

public enum FileExtension
{
    Json,
    Sah
}