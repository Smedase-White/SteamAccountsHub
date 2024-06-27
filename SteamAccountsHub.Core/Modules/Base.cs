using SteamAccountsHub.Core.Utils.Cryptography;

namespace SteamAccountsHub.Core.Modules;

public class Base
{
    public CryptoString Email { get; set; }

    public CryptoString Password { get; set; }
}
