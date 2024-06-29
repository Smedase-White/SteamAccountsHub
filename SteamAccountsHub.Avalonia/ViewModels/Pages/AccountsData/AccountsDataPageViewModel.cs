using System.Collections.Generic;
using System.IO;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

using SteamAccountsHub.Avalonia.ViewModels.Bases;
using SteamAccountsHub.Core.Modules;
using SteamAccountsHub.Core.Utils.Cryptography;
using SteamAccountsHub.Core.Utils.Files;

namespace SteamAccountsHub.Avalonia.ViewModels.Pages.AccountsData;

public class AccountsDataPageViewModel : PageViewModelBase
{
    public AccountsDataPageViewModel()
    {
        AccountsCardList = new(this);

        Toolbar = new(this);

        KeyInputMode mode = File.Exists(SaveFile.Filename) ? KeyInputMode.Check : KeyInputMode.Set;
        KeyInput = new(this, SaveFile.Filename, mode)
        {
            CheckCommand = mode switch
            {
                KeyInputMode.Set => ReactiveCommand.Create(() => { CryptoManager.Key = KeyInput!.Key; KeyInput.Close(); }),
                KeyInputMode.Check => ReactiveCommand.Create(KeyInputInitCheck),
            }
        };
        KeyInput.Open();
    }

    [Reactive]
    public AccountsCardListViewModel AccountsCardList { get; set; }

    [Reactive]
    public ToolbarViewModel Toolbar { get; set; }

    [Reactive]
    public KeyInputViewModel KeyInput { get; set; }

    [Reactive]
    public bool IsEnabled { get; set; }

    [Reactive]
    public int Blur { get; set; }

    [Reactive]
    public bool KeyInputVisible { get; set; }

    private void KeyInputInitCheck()
    {
        if (KeyInput!.IsValidKey())
        {
            CryptoManager.Key = KeyInput!.Key;
            KeyInput.Close();
            CreateBackup();
            LoadAccounts();
        }
        else
            KeyInput.Invalid();
    }

    public void CreateBackup()
    {
        FileManager.Copy(SaveFile.Filename, SaveFile.BackupFilename);
    }

    public void LoadAccounts(string filePath = SaveFile.Filename, string? key = null)
    {
        string tempKey = CryptoManager.Key;
        if (key != null)
            CryptoManager.Key = key;

        SaveFile? save = FileManager.Load<SaveFile>(filePath);
        if (save?.KeyVerify.Value != SaveFile.VerifyWord)
            return;

        AccountsCardList.AddAccounts(save.Accounts ?? [], AddPosition.Start);

        CryptoManager.Key = tempKey;

        AccountsCardList.UpdateRowsCount();
    }

    public void SaveAccounts(List<Account> accounts, string filePath = SaveFile.Filename, string? key = null)
    {
        string tempKey = CryptoManager.Key;
        if (key != null)
            CryptoManager.Key = key;

        SaveFile save = new()
        {
            KeyVerify = new(SaveFile.VerifyWord),
            Accounts = accounts,
        };
        FileManager.Save(save, filePath);

        CryptoManager.Key = tempKey;
    }
}
