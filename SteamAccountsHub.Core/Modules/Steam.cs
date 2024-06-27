using SteamAccountsHub.Core.Utils.Cryptography;

namespace SteamAccountsHub.Core.Modules;

public class Steam
{
    public CryptoString Login { get; set; } = new();

    public CryptoString Password { get; set; } = new();

    public CryptoString ApiKey { get; set; } = new();
}
