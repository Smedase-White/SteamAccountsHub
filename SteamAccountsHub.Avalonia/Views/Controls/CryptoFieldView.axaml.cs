using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;

using SteamAccountsHub.Avalonia.ViewModels.Controls;

namespace SteamAccountsHub.Avalonia.Views.Controls
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
                (DataContext as CryptoFieldViewModel)?.Update();
        }

        private void OnLostFocus(object? sender, RoutedEventArgs e)
        {
            (DataContext as CryptoFieldViewModel)?.Update();
        }
    }
}
