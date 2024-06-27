using System.Reactive;

using Avalonia.Media;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace SteamAccountsHub.Avalonia.ViewModels.Bases;

public class PageSelectorViewModelBase : ViewModelBase
{
    public PageSelectorViewModelBase(PageViewModelBase page)
    {
        Page = page;
    }

    public PageViewModelBase Page { get; }

    [Reactive]
    public SolidColorBrush? Background { get; set; }

    public ReactiveCommand<Unit, Unit>? ChangePage { get; set; }
}
