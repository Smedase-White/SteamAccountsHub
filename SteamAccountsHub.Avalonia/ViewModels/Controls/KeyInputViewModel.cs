using System.IO;
using System.Reactive;

using Avalonia.Media;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

using SteamAccountsHub.Avalonia.ViewModels.Bases;
using SteamAccountsHub.Core.Modules;
using SteamAccountsHub.Core.Utils.Cryptography;
using SteamAccountsHub.Core.Utils.Files;

namespace SteamAccountsHub.Avalonia.ViewModels.Controls;

public class KeyInputViewModel : ViewModelBase
{
    private readonly static SolidColorBrush BaseColor = ResourceExtension.FindResource<SolidColorBrush>("SecondColor")!;
    private readonly static SolidColorBrush InvalidColor = ResourceExtension.FindResource<SolidColorBrush>("InvalidColor")!;
    private readonly static SolidColorBrush SuccessColor = ResourceExtension.FindResource<SolidColorBrush>("SuccessColor")!;

    private readonly MainViewModel _main;

    public KeyInputViewModel(MainViewModel main)
    {
        _main = main;

        CheckCommand = ReactiveCommand.Create(Check);
    }

    [Reactive]
    public string Key { get; set; } = "";

    [Reactive]
    public SolidColorBrush TextBorderColor { get; set; } = BaseColor;

    [Reactive]
    public ReactiveCommand<Unit, Unit>? CheckCommand { get; set; }

    private void Check()
    {
        CryptoManager.Key = Key;
        if (File.Exists(SaveFile.Filename) == false)
        {
            Success();
            return;
        }

        SaveFile? save = FileManager.Load<SaveFile>(SaveFile.Filename);
        if (save?.KeyVerify.Value == SaveFile.VerifyWord)
            Success();
        else
            Invalid();
    }

    private void Success()
    {
        TextBorderColor = SuccessColor;
        _main.Load();
    }

    private void Invalid()
    {
        TextBorderColor = InvalidColor;
    }

    public void Update()
    {
        TextBorderColor = BaseColor;
    }
}
