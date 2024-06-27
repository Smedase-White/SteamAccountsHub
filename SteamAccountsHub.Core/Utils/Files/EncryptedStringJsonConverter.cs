using System.Text.Json;
using System.Text.Json.Serialization;

using SteamAccountsHub.Core.Utils.Cryptography;

namespace SteamAccountsHub.Core.Utils.Files;

public class EncryptedStringJsonConverter : JsonConverter<CryptoString>
{
    public override CryptoString Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return CryptoString.CreateByEncrypted(reader.GetString() ?? "");
    }

    public override void Write(Utf8JsonWriter writer, CryptoString value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.Encrypted);
    }
}
