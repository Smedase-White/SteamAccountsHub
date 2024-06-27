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
            new CryptoFieldViewModel("Email", (value) => { _data.Email = value; DataUpdate?.Invoke(); }, _data.Email.Decrypted),
            new CryptoFieldViewModel("Password", (value) => { _data.Password = value; DataUpdate?.Invoke(); }, _data.Password.Decrypted),
        ];
    }

    public ObservableCollection<CryptoFieldViewModel> CryptoFields { get; set; }

    public event IDataUpdatable.DataUpdateHandler? DataUpdate;
}
