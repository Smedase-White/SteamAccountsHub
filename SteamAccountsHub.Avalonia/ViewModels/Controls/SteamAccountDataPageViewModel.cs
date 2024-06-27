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
            new CryptoFieldViewModel("Login", _data.Login),
            new CryptoFieldViewModel("Password", _data.Password),
            new CryptoFieldViewModel("API Key", _data.ApiKey),
        ];
        foreach (CryptoFieldViewModel field in CryptoFields)
            field.Update = () => DataUpdate?.Invoke();
    }

    public ObservableCollection<CryptoFieldViewModel> CryptoFields { get; set; }

    public event IDataUpdatable.DataUpdateHandler? DataUpdate;
}
