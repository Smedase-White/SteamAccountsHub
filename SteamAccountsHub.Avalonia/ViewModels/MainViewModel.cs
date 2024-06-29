using Avalonia.Media;

using FluentIcons.Common;

using ReactiveUI.Fody.Helpers;
using SteamAccountsHub.Avalonia.Utils;
using SteamAccountsHub.Avalonia.ViewModels.Bases;
using SteamAccountsHub.Avalonia.ViewModels.Pages.AccountsData;
using SteamAccountsHub.Avalonia.ViewModels.Pages.Info;
using SteamAccountsHub.Avalonia.ViewModels.Pages.Settings;

namespace SteamAccountsHub.Avalonia.ViewModels;

public class MainViewModel : PageControllerViewModelBase<MainSelectorViewModel>
{
    public MainViewModel()
        : base(ResourceExtension.FindResource<SolidColorBrush>("PanelBackgroundColor")!,
            ResourceExtension.FindResource<SolidColorBrush>("SecondColor")!)
    {
        InitSelectors([
            new(new AccountsDataPageViewModel(), Symbol.Accessibility, "Accounts"),
            new(new SettingsPageViewModel(), Symbol.Settings, "Settings"),
            new(new InfoPageViewModel(), Symbol.Info, "Information"),
        ]);
    }

    [Reactive]
    public bool IsMenuOpen { get; set; } = false;

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
