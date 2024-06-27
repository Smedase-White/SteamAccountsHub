namespace SteamAccountsHub.Avalonia.Utils;

public interface IDataUpdatable
{
    public delegate void DataUpdateHandler();

    public event DataUpdateHandler? DataUpdate;
}
