using Avalonia.Controls;
using Avalonia.Interactivity;

using SteamAccountsHub.Avalonia.ViewModels.Pages.AccountsData;

namespace SteamAccountsHub.Avalonia.Views.Pages.AccountsData
{
    public partial class KeyInputView : UserControl
    {
        public KeyInputView()
        {
            InitializeComponent();
        }

        private void OnGotFocus(object? sender, RoutedEventArgs e)
        {
            (DataContext as KeyInputViewModel)?.Update();
        }
    }
}
