using System.Collections.ObjectModel;
using System.Reactive;

using Avalonia.Media;

using ReactiveUI;

using SteamAccountsHub.Avalonia.Utils;
using SteamAccountsHub.Avalonia.ViewModels.Bases;
using SteamAccountsHub.Core.Modules;

namespace SteamAccountsHub.Avalonia.ViewModels.Controls;

public class AccountCardViewModel : PageControllerViewModelBase<AccountDataSelectorViewModel>, IDataUpdatable
{
    public enum MoveDirection
    {
        Start,
        Back,
        Forward,
        End
    }

    //Костыль, да-да
    public static double CardWidth { get; } = 300;

    private readonly Account _data;
    private readonly ObservableCollection<AccountCardViewModel> _accounts;

    public AccountCardViewModel(Account data, ObservableCollection<AccountCardViewModel> accounts)
        : base(ResourceExtension.FindResource<SolidColorBrush>("PanelBackgroundColor")!,
            ResourceExtension.FindResource<SolidColorBrush>("SecondColor")!,
            [
                new(new BaseAccountDataPageViewModel(data.Base), "Base"),
                new(new SteamAccountDataPageViewModel(data.Steam), "Steam"),
            ])
    {
        _data = data;
        _accounts = accounts;

        MoveToStartCommand = ReactiveCommand.Create(() => Move(MoveDirection.Start));
        MoveBackCommand = ReactiveCommand.Create(() => Move(MoveDirection.Back));
        MoveForwardCommand = ReactiveCommand.Create(() => Move(MoveDirection.Forward));
        MoveToEndCommand = ReactiveCommand.Create(() => Move(MoveDirection.End));

        DeleteCommand = ReactiveCommand.Create(Delete);

        foreach (AccountDataSelectorViewModel selector in Selectors)
            (selector.Page as IDataUpdatable)!.DataUpdate += Update;
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

    public event IDataUpdatable.DataUpdateHandler? DataUpdate;

    private void Move(MoveDirection direction)
    {
        int oldIndex = _accounts.IndexOf(this);
        int newIndex = direction switch
        {
            MoveDirection.Start => 0,
            MoveDirection.Back => oldIndex - 1,
            MoveDirection.Forward => oldIndex + 1,
            MoveDirection.End => _accounts.Count - 1,
        };

        if (newIndex >= 0 && newIndex < _accounts.Count)
            _accounts.Move(oldIndex, newIndex);

        Update();
    }

    private void Delete()
    {
        _accounts.Remove(this);

        Update();
    }

    public void Update()
    {
        DataUpdate?.Invoke();
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
