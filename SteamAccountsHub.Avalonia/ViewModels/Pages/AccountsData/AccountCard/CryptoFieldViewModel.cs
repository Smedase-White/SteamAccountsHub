using System.Reactive;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.VisualTree;

using ReactiveUI;

using SteamAccountsHub.Avalonia.ViewModels.Bases;
using SteamAccountsHub.Core.Utils.Cryptography;

namespace SteamAccountsHub.Avalonia.ViewModels.Pages.AccountsData.AccountCard;

public class CryptoFieldViewModel : ViewModelBase
{
    private readonly CryptoString _cryptoString;
    private string _oldData;

    public CryptoFieldViewModel(string label, CryptoString cryptoString)
    {
        _cryptoString = cryptoString;
        _oldData = _cryptoString.Value;

        Label = label;
        Copy = ReactiveCommand.Create(CopyToClipboard);
    }

    public string Label { get; }

    public string Data
    {
        get => _cryptoString?.Value ?? "";
        set
        {
            _cryptoString.Value = value;
        }
    }

    public ReactiveCommand<Unit, Unit>? Copy { get; set; }

    public void DataPropertyChanged()
    {
        if (Data == _oldData)
            return;

        _oldData = Data;
        this.RaisePropertyChanged(nameof(Data));
    }

    private void CopyToClipboard()
    {
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime { MainWindow: { } window })
            window.Clipboard?.SetTextAsync(Data);
        else if (Application.Current?.ApplicationLifetime is ISingleViewApplicationLifetime { MainView: { } mainView })
            (mainView.GetVisualRoot() as TopLevel)?.Clipboard?.SetTextAsync(Data);
    }
}
