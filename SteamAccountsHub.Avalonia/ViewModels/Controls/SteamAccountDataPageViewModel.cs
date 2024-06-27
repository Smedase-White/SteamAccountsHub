using System.Collections.ObjectModel;

using SteamAccountsHub.Avalonia.Utils;
using SteamAccountsHub.Avalonia.ViewModels.Bases;
using SteamAccountsHub.Core.Modules;

namespace SteamAccountsHub.Avalonia.ViewModels.Controls;

public class SteamAccountDataPageViewModel : PageViewModelBase, IDataUpdatable
{
    private readonly Steam _data;

    public SteamAccountDataPageViewModel()
        : this(new())
    { }

    public SteamAccountDataPageViewModel(Steam data)
    {
        _data = data;
        CryptoFields =
        [
            new CryptoFieldViewModel("Login", (value) => { _data.Login = value; DataUpdate?.Invoke(); }, _data.Login.Decrypted),
            new CryptoFieldViewModel("Password", (value) => { _data.Password = value; DataUpdate?.Invoke(); }, _data.Password.Decrypted),
            new CryptoFieldViewModel("API Key", (value) => { _data.ApiKey = value; DataUpdate?.Invoke(); }, _data.ApiKey.Decrypted),
        ];
    }

    public ObservableCollection<CryptoFieldViewModel> CryptoFields { get; set; }

    public event IDataUpdatable.DataUpdateHandler? DataUpdate;
}
