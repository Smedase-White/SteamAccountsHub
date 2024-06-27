using System.Collections.Generic;
using System.Collections.ObjectModel;

using Avalonia.Media;

using DynamicData;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace SteamAccountsHub.Avalonia.ViewModels.Bases;

public class PageControllerViewModelBase<SelectorType> : ViewModelBase
    where SelectorType : PageSelectorViewModelBase
{
    private readonly SolidColorBrush _unselectedColor;
    private readonly SolidColorBrush _selectedColor;
    private SelectorType? _currentSelector;

    public PageControllerViewModelBase(SolidColorBrush unselectedColor, SolidColorBrush selectedColor, IEnumerable<SelectorType>? selectors = null)
    {
        _unselectedColor = unselectedColor;
        _selectedColor = selectedColor;

        if (selectors != null)
            InitSelectors(selectors);
    }

    [Reactive]
    public PageViewModelBase? CurrentPage { get; set; }

    public ObservableCollection<SelectorType> Selectors { get; private set; } = [];

    public void InitSelectors(IEnumerable<SelectorType> selectors)
    {
        Selectors.Clear();
        Selectors.AddRange(selectors);
        foreach (SelectorType item in Selectors)
        {
            item.Background = _unselectedColor;
            item.ChangePage = ReactiveCommand.Create(() => ChangePage(item));
        }

        _currentSelector = Selectors[0];
        CurrentPage = _currentSelector.Page;
        _currentSelector.Background = _selectedColor;
    }

    public void ChangePage(SelectorType clickedItem)
    {
        if (_currentSelector == clickedItem)
            return;

        if (_currentSelector is not null)
            _currentSelector.Background = _unselectedColor;
        clickedItem.Background = _selectedColor;

        _currentSelector = clickedItem;
        CurrentPage = _currentSelector.Page;
    }
}
