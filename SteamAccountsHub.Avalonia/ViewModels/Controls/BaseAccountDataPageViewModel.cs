using System.Collections.ObjectModel;

using SteamAccountsHub.Avalonia.Utils;
using SteamAccountsHub.Avalonia.ViewModels.Bases;
using SteamAccountsHub.Core.Modules;

namespace SteamAccountsHub.Avalonia.ViewModels.Controls;

public class BaseAccountDataPageViewModel : PageViewModelBase, IDataUpdatable
{
    private readonly Base _data;

    public BaseAccountDataPageViewModel()
        : this(new())
    { }

    public BaseAccountDataPageViewModel(Base data)
    {
        _data = data;

        CryptoFields =
        [
            new CryptoFieldViewModel("Email", _data.Email),
            new CryptoFieldViewModel("Password", _data.Password),
        ];
        foreach (CryptoFieldViewModel field in CryptoFields)
            field.Update = () => DataUpdate?.Invoke();
    }

    public ObservableCollection<CryptoFieldViewModel> CryptoFields { get; set; }

    public event IDataUpdatable.DataUpdateHandler? DataUpdate;
}
