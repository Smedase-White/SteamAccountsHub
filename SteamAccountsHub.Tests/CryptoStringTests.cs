using SteamAccountsHub.Core.Utils.Cryptography;

namespace SteamAccountsHub.Tests;

public class CryptoStringTests
{
    [Fact]
    public void DecryptEqualOriginal()
    {
        // Arrange
        string key = "random key";
        string text = "test string";
        // Act
        CryptoManager.Key = key;
        CryptoString encrypted = CryptoString.CreateByDecrypted(text);
        CryptoString decrypted = CryptoString.CreateByEncrypted(encrypted.Encrypted);
        // Assert
        Assert.Equal(text, decrypted.Decrypted);
    }
}
