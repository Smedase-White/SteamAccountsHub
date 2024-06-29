using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

using Avalonia;

using DynamicData;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

using SteamAccountsHub.Avalonia.ViewModels.Bases;
using SteamAccountsHub.Avalonia.ViewModels.Pages.AccountsData.AccountCard;
using SteamAccountsHub.Core.Modules;

namespace SteamAccountsHub.Avalonia.ViewModels.Pages.AccountsData;

public class AccountsCardListViewModel : ViewModelBase
{
    private readonly AccountsDataPageViewModel _dataPage;
    private Func<CardViewModel, bool>? _filter;

    public AccountsCardListViewModel(AccountsDataPageViewModel dataPage)
    {
        _dataPage = dataPage;

        AllAccounts = [];
        AllAccounts.CollectionChanged += AllAccountsChanged;

        VisibleAccounts = AllAccounts;
    }

    private ObservableCollection<CardViewModel> AllAccounts { get; set; }

    public ObservableCollection<CardViewModel> VisibleAccounts { get; set; }

    public Func<CardViewModel, bool>? Filter
    {
        get => _filter;
        set
        {
            _filter = value;
            Update();
        }
    }

    [Reactive]
    public int ColumnsCount { get; set; }

    [Reactive]
    public int RowsCount { get; set; }

    public IEnumerable<Account> GetData()
    {
        return AllAccounts.Select(card => card.Data);
    }

    public CardViewModel CreateAccountCard(Account account)
    {
        CardViewModel card = new(account, this);
        card.PropertyChanged += DataPropertyChanged;
        return card;
    }

    public void AddAccount(Account account, AddPosition position = AddPosition.Start)
    {
        CardViewModel card = CreateAccountCard(account);
        switch (position)
        {
            case AddPosition.Start:
                AllAccounts.Insert(0, card);
                break;
            case AddPosition.End:
                AllAccounts.Add(card);
                break;
        }
        UpdateRowsCount();
    }

    public void AddAccounts(IEnumerable<Account> accounts, AddPosition position = AddPosition.Start)
    {
        int index = position switch
        {
            AddPosition.Start => 0,
            AddPosition.End => AllAccounts.Count,
        };
        AllAccounts.AddOrInsertRange(accounts.Select(CreateAccountCard), index);
        UpdateRowsCount();
    }


    public void MoveAccountCard(CardViewModel card, int newIndex, bool isRelative)
    {
        int oldIndex = AllAccounts.IndexOf(card);

        if (isRelative)
            newIndex += oldIndex;

        if (newIndex == -1)
            newIndex = AllAccounts.Count - 1;

        if (newIndex < 0 || newIndex >= AllAccounts.Count)
            return;

        AllAccounts.Move(oldIndex, newIndex);
    }

    public void RemoveAccountCard(CardViewModel card)
    {
        AllAccounts.Remove(card);
    }

    public void Update()
    {
        if (Filter is null)
        {
            if (VisibleAccounts != AllAccounts)
                VisibleAccounts = AllAccounts;
        }
        else
        {
            VisibleAccounts = new(AllAccounts.Where(Filter));
        }
        this.RaisePropertyChanged(nameof(VisibleAccounts));
    }

    private void Save()
    {
        _dataPage.SaveAccounts(AllAccounts.Select(account => account.Data).ToList());
    }

    public void ControlResize(Rect rect)
    {
        UpdateColumnsCount(rect.Width);
        UpdateRowsCount();
    }

    public void UpdateColumnsCount(double width)
    {
        int columnsCount = (int)((width - 10.0) / (CardViewModel.CardWidth + 20.0));
        if (columnsCount != ColumnsCount)
            ColumnsCount = columnsCount;
    }

    public void UpdateRowsCount()
    {
        int rowsCount = (int)Math.Ceiling(1.0 * AllAccounts.Count / ColumnsCount);
        if (rowsCount != RowsCount)
            RowsCount = rowsCount;
    }

    private void DataPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        Save();
    }

    private void AllAccountsChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        Update();
        Save();
    }
}

public enum AddPosition
{
    Start,
    End
}
