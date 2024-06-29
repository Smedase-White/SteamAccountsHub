using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;

using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

using SteamAccountsHub.Avalonia.ViewModels.Bases;
using SteamAccountsHub.Core.Modules;
using SteamAccountsHub.Core.Utils.Files;

namespace SteamAccountsHub.Avalonia.ViewModels.Pages.AccountsData;

public class ToolbarViewModel : ViewModelBase
{
    private static readonly FilePickerFileType[] FileTypes =
    [
        new FilePickerFileType("SAH encrypted files") { Patterns = ["*.sah"] },
        new FilePickerFileType("Json files") { Patterns = ["*.json"] },
        new FilePickerFileType("All available files") { Patterns = ["*.sah", "*.json"] },
    ];

    private readonly AccountsDataPageViewModel _dataView;

    public ToolbarViewModel(AccountsDataPageViewModel dataView)
    {
        _dataView = dataView;

        CreateAccountCommand = ReactiveCommand.Create(CreateAccount);
        ImportAccountsCommand = ReactiveCommand.CreateFromTask(ImportAccounts);
        OpenExportListCommand = ReactiveCommand.Create(OpenExportList);
        ExportAccountsCommand = ReactiveCommand.CreateFromTask(ExportAccounts);
        SearchAccountCommand = ReactiveCommand.Create(SearchAccountsByName);
        ResetSearchCommand = ReactiveCommand.Create(ResetSearch);
    }

    [Reactive]
    public string? SearchText { get; set; }

    public ReactiveCommand<Unit, Unit>? CreateAccountCommand { get; set; }

    public ReactiveCommand<Unit, Unit>? ImportAccountsCommand { get; set; }

    public ReactiveCommand<Unit, Unit>? OpenExportListCommand { get; set; }

    public ReactiveCommand<Unit, Unit>? ExportAccountsCommand { get; set; }

    public ReactiveCommand<Unit, Unit>? SearchAccountCommand { get; set; }

    public ReactiveCommand<Unit, Unit>? ResetSearchCommand { get; set; }

    public bool? AllChecked
    {
        get
        {
            bool withChecked = CheckAccounts.Any(check => check.IsChecked);
            bool withUnchecked = CheckAccounts.Any(check => !check.IsChecked);
            if (withUnchecked == false)
                return true;
            if (withChecked == false)
                return false;
            return null;
        }
        set
        {
            foreach (CheckAccountViewModel check in CheckAccounts)
                check.IsChecked = value ?? false;
            this.RaisePropertyChanged(nameof(AllChecked));
        }
    }

    [Reactive]
    public ObservableCollection<CheckAccountViewModel> CheckAccounts { get; set; }

    private AccountsCardListViewModel Accounts => _dataView.AccountsCardList;

    private void CreateAccount()
    {
        ResetSearch();
        Accounts.AddAccount(new(), AddPosition.Start);
    }

    private void SearchAccountsByName()
    {
        Accounts.Filter = account => account.Name?.Contains(SearchText ?? "", StringComparison.OrdinalIgnoreCase) ?? false;
    }

    private void ResetSearch()
    {
        SearchText = null;
        Accounts.Filter = null;
    }

    private void OpenExportList()
    {
        CheckAccounts = new(Accounts.GetData().Select(account => new CheckAccountViewModel(account)));
        foreach (CheckAccountViewModel check in CheckAccounts)
            check.PropertyChanged += CheckPropertyChanged;
        this.RaisePropertyChanged(nameof(AllChecked));
    }

    private async Task ImportAccounts()
    {
        ResetSearch();

        if (Application.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop ||
            desktop.MainWindow?.StorageProvider is not { } provider)
            throw new NullReferenceException("Missing StorageProvider instance.");

        IReadOnlyList<IStorageFile> files = await provider.OpenFilePickerAsync(new FilePickerOpenOptions()
        {
            Title = "Import accounts",
            AllowMultiple = true,
            FileTypeFilter = FileTypes,
        });

        if ((files?.Count > 0) == false)
            return;

        List<KeyInputViewModel> inputs = [];

        for (int i = 0; i < files.Count; i++)
        {
            int j = i;
            switch (FileManager.GetFileExtension(files[j].Path.AbsolutePath))
            {
                case FileExtension.Json:
                    _dataView.LoadAccounts(files[j].Path.AbsolutePath);
                    break;
                case FileExtension.Sah:
                    inputs.Add(new(_dataView, files[j].Path.AbsolutePath, KeyInputMode.Check)
                    {
                        CheckCommand = ReactiveCommand.Create(() =>
                        {
                            if (_dataView.KeyInput.IsValidKey())
                            {
                                _dataView.LoadAccounts(files[j].Path.AbsolutePath, _dataView.KeyInput.Key);
                                _dataView.KeyInput.CloseCommand?.Execute().Subscribe();
                            }
                            else
                                _dataView.KeyInput.Invalid();
                        })
                    });
                    break;
            }
        }

        if (inputs.Count > 0)
        {
            for (int i = inputs.Count - 2; i >= 0; i--)
            {
                int j = i;
                inputs[i].CloseCommand = ReactiveCommand.Create(() => { _dataView.KeyInput = inputs[j + 1]; });
            }
            inputs[^1].CloseCommand = ReactiveCommand.Create(() => { _dataView.KeyInput.Close(); });
            _dataView.KeyInput = inputs[0];
            _dataView.KeyInput.Open();
        }
    }

    private async Task ExportAccounts()
    {
        ResetSearch();

        List<Account> accounts = CheckAccounts.Where(check => check.IsChecked).Select(check => check.Account).ToList();

        if (Application.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop ||
            desktop.MainWindow?.StorageProvider is not { } provider)
            throw new NullReferenceException("Missing StorageProvider instance.");

        IStorageFile? file = await provider.SaveFilePickerAsync(new FilePickerSaveOptions()
        {
            Title = "Export accounts",
            FileTypeChoices = FileTypes,
        });

        if (file is null)
            return;

        switch (FileManager.GetFileExtension(file.Path.AbsolutePath))
        {
            case FileExtension.Json:
                _dataView.SaveAccounts(accounts, file.Path.AbsolutePath);
                break;
            case FileExtension.Sah:
                _dataView.KeyInput = new(_dataView, file.Path.AbsolutePath, KeyInputMode.Set)
                {
                    CheckCommand = ReactiveCommand.Create(() =>
                    {
                        _dataView.SaveAccounts(accounts, file.Path.AbsolutePath, _dataView.KeyInput.Key);
                        _dataView.KeyInput.CloseCommand?.Execute().Subscribe();
                    })
                };
                _dataView.KeyInput.Open();
                break;
        }
    }

    private void CheckPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        this.RaisePropertyChanged(nameof(AllChecked));
    }
}

public class CheckAccountViewModel : ViewModelBase
{
    public CheckAccountViewModel(Account account)
    {
        Account = account;
    }

    public Account Account { get; }

    [Reactive]
    public bool IsChecked { get; set; } = true;

    public string AccountName => Account.Name;
}
