namespace SteamAccountsHub.Core.Modules;

public partial class Account
{
    public string Name { get; set; } = "";

    public Base Base { get; set; } = new();

    public Steam Steam { get; set; } = new();
}
