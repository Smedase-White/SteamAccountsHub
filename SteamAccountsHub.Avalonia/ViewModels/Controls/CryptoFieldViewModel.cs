using System;
using System.Reactive;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.VisualTree;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

using SteamAccountsHub.Avalonia.ViewModels.Bases;
using SteamAccountsHub.Core.Utils.Cryptography;

namespace SteamAccountsHub.Avalonia.ViewModels.Controls;

public class CryptoFieldViewModel : ViewModelBase
{
    private readonly Action<CryptoString> _setter;

    public CryptoFieldViewModel(string label, Action<CryptoString> setter, string data = "")
    {
        _setter = setter;

        Label = label;
        Data = data;

        Copy = ReactiveCommand.Create(CopyToClipboard);
    }

    public string Label { get; }

    [Reactive]
    public string Data { get; set; }

    public ReactiveCommand<Unit, Unit>? Copy { get; set; }

    private void CopyToClipboard()
    {
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime { MainWindow: { } window })
            window.Clipboard?.SetTextAsync(Data);
        else if (Application.Current?.ApplicationLifetime is ISingleViewApplicationLifetime { MainView: { } mainView })
            (mainView.GetVisualRoot() as TopLevel)?.Clipboard?.SetTextAsync(Data);
    }

    public void Update()
    {
        _setter(CryptoString.CreateByDecrypted(Data));
    }
}
