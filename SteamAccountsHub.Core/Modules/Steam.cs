using SteamAccountsHub.Core.Utils.Cryptography;

namespace SteamAccountsHub.Core.Modules;

public class Steam
{
    public CryptoString Login { get; set; }

    public CryptoString Password { get; set; }

    public CryptoString ApiKey { get; set; }
}
