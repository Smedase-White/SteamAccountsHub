using Avalonia.Media;

using FluentIcons.Common;

using ReactiveUI.Fody.Helpers;

using SteamAccountsHub.Avalonia.ViewModels.Bases;
using SteamAccountsHub.Avalonia.ViewModels.Controls;
using SteamAccountsHub.Avalonia.ViewModels.Pages;

namespace SteamAccountsHub.Avalonia.ViewModels;

public class MainViewModel : PageControllerViewModelBase<MainSelectorViewModel>
{
    public MainViewModel()
        : base(ResourceExtension.FindResource<SolidColorBrush>("PanelBackgroundColor")!,
            ResourceExtension.FindResource<SolidColorBrush>("SecondColor")!)
    {
        KeyInput = new(this);
    }

    [Reactive]
    public bool IsEnabled { get; set; } = false;

    [Reactive]
    public int Blur { get; set; } = 5;

    [Reactive]
    public KeyInputViewModel KeyInput { get; set; }

    [Reactive]
    public bool KeyInputVisible { get; set; } = true;

    [Reactive]
    public bool IsMenuOpen { get; set; } = false;

    public void Load()
    {
        InitSelectors([
            new(new AccountsDataPageViewModel(), Symbol.Accessibility, "Accounts"),
            new(new SettingsPageViewModel(), Symbol.Settings, "Settings"),
            new(new InfoPageViewModel(), Symbol.Info, "Information"),

        ]);
        IsEnabled = true;
        KeyInputVisible = false;
        Blur = 0;
    }

    public void ChangeMenuState()
    {
        IsMenuOpen = !IsMenuOpen;
    }

}

public class MainSelectorViewModel : PageSelectorViewModelBase
{
    public Symbol Symbol { get; }

    public string Label { get; }

    public MainSelectorViewModel(PageViewModelBase page, Symbol symbol, string label)
        : base(page)
    {
        Symbol = symbol;
        Label = label;
    }
}
