using System.Collections.ObjectModel;
using System.ComponentModel;

using ReactiveUI;

using SteamAccountsHub.Avalonia.ViewModels.Bases;
using SteamAccountsHub.Core.Modules;

namespace SteamAccountsHub.Avalonia.ViewModels.Pages.AccountsData.AccountCard;

public class BaseAccountDataPageViewModel : PageViewModelBase
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
            field.PropertyChanged += DataPropertyChanged;
    }

    public ObservableCollection<CryptoFieldViewModel> CryptoFields { get; set; }

    private void DataPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        this.RaisePropertyChanged((sender as CryptoFieldViewModel)!.Label);
    }
}
