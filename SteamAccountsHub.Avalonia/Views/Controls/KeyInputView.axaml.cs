using Avalonia.Controls;
using Avalonia.Interactivity;

using SteamAccountsHub.Avalonia.ViewModels.Controls;

namespace SteamAccountsHub.Avalonia.Views.Controls
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
