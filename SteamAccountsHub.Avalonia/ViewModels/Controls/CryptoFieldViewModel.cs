using System;
using System.Reactive;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.VisualTree;

using ReactiveUI;

using SteamAccountsHub.Avalonia.ViewModels.Bases;
using SteamAccountsHub.Core.Utils.Cryptography;

namespace SteamAccountsHub.Avalonia.ViewModels.Controls;

public class CryptoFieldViewModel : ViewModelBase
{
    private readonly CryptoString _cryptoString;

    public CryptoFieldViewModel(string label, CryptoString cryptoString)
    {
        _cryptoString = cryptoString;

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
            this.RaisePropertyChanged(nameof(Data));
            Update?.Invoke();
        }
    }

    public ReactiveCommand<Unit, Unit>? Copy { get; set; }

    public Action? Update { get; set; }

    private void CopyToClipboard()
    {
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime { MainWindow: { } window })
            window.Clipboard?.SetTextAsync(Data);
        else if (Application.Current?.ApplicationLifetime is ISingleViewApplicationLifetime { MainView: { } mainView })
            (mainView.GetVisualRoot() as TopLevel)?.Clipboard?.SetTextAsync(Data);
    }
}
