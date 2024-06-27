using System;

using Avalonia;
using Avalonia.Controls;

using SteamAccountsHub.Avalonia.ViewModels.Pages;

namespace SteamAccountsHub.Avalonia.Views.Pages
{
    public partial class AccountsDataPageView : UserControl
    {
        public AccountsDataPageView()
        {
            InitializeComponent();

            this.GetObservable(BoundsProperty).Subscribe(Resize);
        }

        private void Resize(Rect rect)
        {
            (DataContext as AccountsDataPageViewModel)?.ControlResize(rect);
        }
    }
}
