using System.IO;
using System.Reactive;

using Avalonia.Media;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

using SteamAccountsHub.Avalonia.ViewModels.Bases;
using SteamAccountsHub.Core.Modules;
using SteamAccountsHub.Core.Utils.Cryptography;
using SteamAccountsHub.Core.Utils.Files;

namespace SteamAccountsHub.Avalonia.ViewModels.Pages.AccountsData;

public class KeyInputViewModel : ViewModelBase
{

    private readonly static SolidColorBrush BaseColor = ResourceExtension.FindResource<SolidColorBrush>("SecondColor")!;
    private readonly static SolidColorBrush InvalidColor = ResourceExtension.FindResource<SolidColorBrush>("InvalidColor")!;
    private readonly static SolidColorBrush SuccessColor = ResourceExtension.FindResource<SolidColorBrush>("SuccessColor")!;

    private readonly AccountsDataPageViewModel _dataView;
    private readonly string _filePath;

    public KeyInputViewModel(AccountsDataPageViewModel dataView, string filePath, KeyInputMode mode)
    {
        _dataView = dataView;

        _filePath = filePath;

        Label = mode switch
        {
            KeyInputMode.Set => $"Set secret key to {Path.GetFileName(_filePath)}",
            KeyInputMode.Check => $"Enter secret key to {Path.GetFileName(_filePath)}"
        };

        CloseCommand = ReactiveCommand.Create(Close);
    }

    public string Label { get; }

    [Reactive]
    public string Key { get; set; } = "";

    [Reactive]
    public SolidColorBrush TextBorderColor { get; set; } = BaseColor;

    public ReactiveCommand<Unit, Unit>? CheckCommand { get; set; }

    public ReactiveCommand<Unit, Unit>? CloseCommand { get; set; }

    public bool IsValidKey()
    {
        string tempKey = CryptoManager.Key;
        CryptoManager.Key = Key;
        SaveFile? save = FileManager.Load<SaveFile>(_filePath);
        CryptoManager.Key = tempKey;
        return save?.KeyVerify.Value == SaveFile.VerifyWord;
    }

    public void Success()
    {
        TextBorderColor = SuccessColor;
    }

    public void Invalid()
    {
        TextBorderColor = InvalidColor;
    }

    public void Update()
    {
        TextBorderColor = BaseColor;
    }

    public void Open()
    {
        ChangeKeyInputState(true);
    }

    public void Close()
    {
        ChangeKeyInputState(false);
    }

    private void ChangeKeyInputState(bool isEnabled)
    {
        if (isEnabled == true)
        {

            _dataView.IsEnabled = false;
            _dataView.KeyInputVisible = true;
            _dataView.Blur = 5;
        }
        else
        {
            _dataView.IsEnabled = true;
            _dataView.KeyInputVisible = false;
            _dataView.Blur = 0;
        }
    }
}

public enum KeyInputMode
{
    Set,
    Check
}
