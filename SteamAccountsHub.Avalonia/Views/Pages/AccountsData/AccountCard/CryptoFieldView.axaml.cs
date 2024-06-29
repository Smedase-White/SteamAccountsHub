using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;

using SteamAccountsHub.Avalonia.ViewModels.Pages.AccountsData.AccountCard;

namespace SteamAccountsHub.Avalonia.Views.Pages.AccountsData.AccountCard
{
    public partial class CryptoFieldView : UserControl
    {
        public CryptoFieldView()
        {
            InitializeComponent();
        }

        private void OnKeyDown(object? sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                (DataContext as CryptoFieldViewModel)?.DataPropertyChanged();
        }

        private void OnLostFocus(object? sender, RoutedEventArgs e)
        {
            (DataContext as CryptoFieldViewModel)?.DataPropertyChanged();
        }
    }
}
