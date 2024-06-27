using SteamAccountsHub.Core.Utils.Cryptography;

namespace SteamAccountsHub.Core.Modules;

public class SaveFile
{
    public const string Filename = "Accounts.sah";
    public const string BackupFilename = "AccountsBackup.sah";

    public const string VerifyWord = "SteamAccountsHub";

    public CryptoString KeyVerify { get; set; }

    public List<Account>? Accounts { get; set; }
}
