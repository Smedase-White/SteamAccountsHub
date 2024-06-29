using System.ComponentModel;
using System.Reactive;

using Avalonia.Media;

using ReactiveUI;
using SteamAccountsHub.Avalonia.Utils;
using SteamAccountsHub.Avalonia.ViewModels.Bases;
using SteamAccountsHub.Core.Modules;

namespace SteamAccountsHub.Avalonia.ViewModels.Pages.AccountsData.AccountCard;

public class CardViewModel : PageControllerViewModelBase<AccountDataSelectorViewModel>
{
    //Костыль, да-да
    public static double CardWidth { get; } = 400;

    private readonly Account _data;
    private readonly AccountsCardListViewModel _cardsList;
    //private readonly ObservableCollection<CardViewModel> _accounts;

    public CardViewModel()
        : this(new(), new(new()))
    { }

    public CardViewModel(Account data, AccountsCardListViewModel cardsList)
        : base(ResourceExtension.FindResource<SolidColorBrush>("PanelBackgroundColor")!,
            ResourceExtension.FindResource<SolidColorBrush>("SecondColor")!,
            [
                new(new BaseAccountDataPageViewModel(data.Base), "Base"),
                new(new SteamAccountDataPageViewModel(data.Steam), "Steam"),
            ])
    {
        _data = data;
        _cardsList = cardsList;

        MoveToStartCommand = ReactiveCommand.Create(() => Move(MoveDirection.Start));
        MoveBackCommand = ReactiveCommand.Create(() => Move(MoveDirection.Back));
        MoveForwardCommand = ReactiveCommand.Create(() => Move(MoveDirection.Forward));
        MoveToEndCommand = ReactiveCommand.Create(() => Move(MoveDirection.End));

        DeleteCommand = ReactiveCommand.Create(Delete);

        foreach (AccountDataSelectorViewModel selector in Selectors)
            selector.PropertyChanged += DataPropertyChanged;
    }

    private void DataPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        this.RaisePropertyChanged(nameof(Data));
    }

    public string Name
    {
        get => _data.Name;
        set
        {
            _data.Name = value;
            this.RaisePropertyChanged(nameof(Name));
        }
    }

    public ReactiveCommand<Unit, Unit>? MoveToStartCommand { get; set; }
    public ReactiveCommand<Unit, Unit>? MoveBackCommand { get; set; }
    public ReactiveCommand<Unit, Unit>? MoveForwardCommand { get; set; }
    public ReactiveCommand<Unit, Unit>? MoveToEndCommand { get; set; }
    public ReactiveCommand<Unit, Unit>? DeleteCommand { get; set; }

    public Account Data => _data;

    private void Move(MoveDirection direction)
    {
        int newIndex;
        bool isRelative;
        (newIndex, isRelative) = direction switch
        {
            MoveDirection.Start => (0, false),
            MoveDirection.Back => (-1, true),
            MoveDirection.Forward => (1, true),
            MoveDirection.End => (-1, false),
        };

        _cardsList.MoveAccountCard(this, newIndex, isRelative);
    }

    private void Delete()
    {
        _cardsList.RemoveAccountCard(this);
    }
}

public class AccountDataSelectorViewModel : PageSelectorViewModelBase
{
    public AccountDataSelectorViewModel(PageViewModelBase page, string label)
        : base(page)
    {
        Label = label;
    }

    public string Label { get; }
}
public enum MoveDirection
{
    Start,
    Back,
    Forward,
    End
}
