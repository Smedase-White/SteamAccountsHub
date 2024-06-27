using SteamAccountsHub.Core.Utils.Cryptography;

namespace SteamAccountsHub.Tests;

public class CryptoManagerTests
{
    [Fact]
    public void DecryptEqualOriginal()
    {
        // Arrange
        string key = "random key";
        string text = "test string";
        // Act
        CryptoManager.Key = key;
        string encrypted = CryptoManager.EncryptString(text);
        string decrypted = CryptoManager.DecryptString(encrypted);
        // Assert
        Assert.Equal(text, decrypted);
    }

    [Fact]
    public void SameKeyMakeSameEncrypt()
    {
        // Arrange
        string[] sameKeys = ["random key (1)", "random key (1)"];
        string text = "test string";
        // Act
        string[] encrypted = new string[2];
        for (int i = 0; i < 2; i++)
        {
            CryptoManager.Key = sameKeys[i];
            encrypted[i] = CryptoManager.EncryptString(text);
        }
        // Assert
        Assert.Equal(encrypted[0], encrypted[1]);
    }

    [Fact]
    public void DifferentKeyMakeDifferentEncrypt()
    {
        // Arrange
        string[] differentKeys = ["random key (1)", "random key (2)"];
        string text = "test string";
        // Act
        string[] encrypted = new string[2];
        for (int i = 0; i < 2; i++)
        {
            CryptoManager.Key = differentKeys[i];
            encrypted[i] = CryptoManager.EncryptString(text);
        }
        // Assert
        Assert.NotEqual(encrypted[0], encrypted[1]);
    }

    [Fact]
    public void DecryptWithOtherKeyReturnEmptyText()
    {
        // Arrange
        string[] differentKeys = ["random key (1)", "random key (2)"];
        string text = "test string";
        // Act
        CryptoManager.Key = differentKeys[0];
        string encrypted = CryptoManager.EncryptString(text);
        CryptoManager.Key = differentKeys[1];
        string decrypted = CryptoManager.DecryptString(encrypted);
        // Assert
        Assert.Equal("", decrypted);
    }
}