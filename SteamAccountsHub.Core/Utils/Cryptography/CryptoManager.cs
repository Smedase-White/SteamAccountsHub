using System.Security.Cryptography;
using System.Text;

namespace SteamAccountsHub.Core.Utils.Cryptography;

public static class CryptoManager
{
    private static readonly byte[] Vector = [232, 130, 236, 249, 225, 80, 9, 78, 105, 179, 205, 25, 163, 34, 145, 153];

    private static readonly UTF8Encoding UTFEncoder = new();

    private static readonly Aes Algorithm = Aes.Create();

    private static string _key = "";
    private static byte[] _keyHash = SHA256.HashData(UTFEncoder.GetBytes(_key));

    private static ICryptoTransform _encryptor = Algorithm.CreateEncryptor(_keyHash, Vector);
    private static ICryptoTransform _decryptor = Algorithm.CreateDecryptor(_keyHash, Vector);

    public static string Key
    {
        get => _key;
        set
        {
            _key = value;
            UpdateKey();
        }
    }

    private static void UpdateKey()
    {
        _keyHash = SHA256.HashData(UTFEncoder.GetBytes(_key));
        _encryptor = Algorithm.CreateEncryptor(_keyHash, Vector);
        _decryptor = Algorithm.CreateDecryptor(_keyHash, Vector);
    }

    public static string EncryptString(string text)
    {
        return text == "" ? "" : Convert.ToBase64String(Encrypt(text));
    }

    public static byte[] Encrypt(string text)
    {
        using MemoryStream msEncrypt = new();
        using CryptoStream csEncrypt = new(msEncrypt, _encryptor, CryptoStreamMode.Write);
        using StreamWriter swEncrypt = new(csEncrypt);

        swEncrypt.Write(text);
        swEncrypt.Close();
        csEncrypt.Close();

        byte[] encrypted = msEncrypt.ToArray();
        msEncrypt.Close();

        return encrypted;
    }

    public static string DecryptString(string encryptedText)
    {
        return encryptedText == "" ? "" : Decrypt(Convert.FromBase64String(encryptedText));
    }

    public static string Decrypt(byte[] encryptedArray)
    {
        using MemoryStream msDecrypt = new(encryptedArray);
        using CryptoStream csDecrypt = new(msDecrypt, _decryptor, CryptoStreamMode.Read);
        using StreamReader srDecrypt = new(csDecrypt);

        try
        {
            string decrypted = srDecrypt.ReadToEnd();
            return decrypted;
        }
        catch (CryptographicException)
        {
            return "";
        }
        finally
        {
            srDecrypt.Close();
            csDecrypt.Close();
            msDecrypt.Close();
        }
    }
}