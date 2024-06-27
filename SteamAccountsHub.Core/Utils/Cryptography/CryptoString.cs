namespace SteamAccountsHub.Core.Utils.Cryptography;

public readonly struct CryptoString
{
    public readonly string Decrypted;

    public readonly string Encrypted;

    private CryptoString(string decrypted, string encrypted)
    {
        Decrypted = decrypted;
        Encrypted = encrypted;
    }

    public static CryptoString CreateByDecrypted(string decryptedValue)
    {
        return new(decryptedValue, CryptoManager.EncryptString(decryptedValue));
    }

    public static CryptoString CreateByEncrypted(string encryptedValue)
    {
        return new(CryptoManager.DecryptString(encryptedValue), encryptedValue);
    }
}

