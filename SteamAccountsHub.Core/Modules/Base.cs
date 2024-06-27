using SteamAccountsHub.Core.Utils.Cryptography;

namespace SteamAccountsHub.Core.Modules;

public class Base
{
    public CryptoString Email { get; set; } = new();

    public CryptoString Password { get; set; } = new();
}
