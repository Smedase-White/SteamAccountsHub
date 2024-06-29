using System.Collections.ObjectModel;
using System.ComponentModel;

using ReactiveUI;

using SteamAccountsHub.Avalonia.ViewModels.Bases;
using SteamAccountsHub.Core.Modules;

namespace SteamAccountsHub.Avalonia.ViewModels.Pages.AccountsData.AccountCard;

public class SteamAccountDataPageViewModel : PageViewModelBase
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
            field.PropertyChanged += DataPropertyChanged;
    }

    public ObservableCollection<CryptoFieldViewModel> CryptoFields { get; set; }

    private void DataPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        this.RaisePropertyChanged((sender as CryptoFieldViewModel)!.Label);
    }
}
