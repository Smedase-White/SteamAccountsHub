namespace SteamAccountsHub.Core.Utils.Cryptography;

public class CryptoString
{
    public CryptoString(string value = "")
    {
        Value = value;
    }

    public string Value { get; set; }

    public string Encrypted { get => CryptoManager.EncryptString(Value); }

    public readonly static CryptoString Default = new("");

    public static CryptoString CreateByEncrypted(string encryptedValue)
        => new(CryptoManager.DecryptString(encryptedValue));
}

