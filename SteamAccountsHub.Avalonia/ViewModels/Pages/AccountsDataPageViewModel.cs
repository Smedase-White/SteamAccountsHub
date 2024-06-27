using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;

using Avalonia;

using DynamicData;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

using SteamAccountsHub.Avalonia.ViewModels.Bases;
using SteamAccountsHub.Avalonia.ViewModels.Controls;
using SteamAccountsHub.Core.Modules;
using SteamAccountsHub.Core.Utils.Files;

namespace SteamAccountsHub.Avalonia.ViewModels.Pages;

public class AccountsDataPageViewModel : PageViewModelBase
{
    private List<AccountCardViewModel>? _cashAccounts;

    public AccountsDataPageViewModel()
    {
        Accounts = [];

        CreateAccountCommand = ReactiveCommand.Create(CreateAccount);
        SearchAccountCommand = ReactiveCommand.Create(SearchAccountsByName);
        ResetSearchCommand = ReactiveCommand.Create(ResetSearch);

        LoadAccounts();
    }

    [Reactive]
    public int ColumnsCount { get; set; }

    [Reactive]
    public int RowsCount { get; set; }

    public ObservableCollection<AccountCardViewModel> Accounts { get; set; }

    [Reactive]
    public string? SearchText { get; set; }

    public ReactiveCommand<Unit, Unit>? CreateAccountCommand { get; set; }

    public ReactiveCommand<Unit, Unit>? SearchAccountCommand { get; set; }

    public ReactiveCommand<Unit, Unit>? ResetSearchCommand { get; set; }

    public AccountCardViewModel InitAccountCard(Account data)
    {
        AccountCardViewModel accountCard = new(data, Accounts);
        accountCard.DataUpdate += SaveAccounts;
        return accountCard;
    }

    public void LoadAccounts()
    {
        SaveFile? save = FileManager.Load<SaveFile>(SaveFile.Filename);
        if (save?.KeyVerify.Value != SaveFile.VerifyWord)
            return;
        FileManager.Save(save, SaveFile.BackupFilename);

        foreach (Account account in save.Accounts ?? [])
            Accounts.Add(InitAccountCard(account));
        UpdateRowsCount();
    }

    public void SaveAccounts()
    {
        SaveFile save = new()
        {
            KeyVerify = new(SaveFile.VerifyWord),
            Accounts = Accounts.Select(account => account.Data).ToList(),
        };
        FileManager.Save(save, SaveFile.Filename);
    }

    public void ImportAccounts()
    {
        throw new NotImplementedException();
    }

    public void ExportAccounts()
    {
        throw new NotImplementedException();
    }

    private void CreateAccount()
    {
        ResetSearch();
        Accounts.Insert(0, InitAccountCard(new()));
        UpdateRowsCount();
    }

    private void SearchAccountsByName()
    {
        _cashAccounts ??= new(Accounts);
        Accounts.Clear();
        Accounts.AddRange(_cashAccounts.Where(acc => acc.Name?.Contains(SearchText ?? "", StringComparison.OrdinalIgnoreCase) ?? false));
    }

    private void ResetSearch()
    {
        SearchText = null;
        if (_cashAccounts is null)
            return;
        Accounts.Clear();
        Accounts.AddRange(_cashAccounts);
        _cashAccounts = null;
    }

    public void ControlResize(Rect rect)
    {
        UpdateColumnsCount(rect.Width);
        UpdateRowsCount();
    }

    private void UpdateColumnsCount(double width)
    {
        int columnsCount = (int)(width / (AccountCardViewModel.CardWidth + 20));
        if (columnsCount != ColumnsCount)
            ColumnsCount = columnsCount;
    }

    private void UpdateRowsCount()
    {
        int rowsCount = (int)Math.Ceiling(1.0 * Accounts.Count / ColumnsCount);
        if (rowsCount != RowsCount)
            RowsCount = rowsCount;
    }
}
