using System;

using Avalonia;
using Avalonia.Controls;

using SteamAccountsHub.Avalonia.ViewModels.Pages.AccountsData;

namespace SteamAccountsHub.Avalonia.Views.Pages.AccountsData
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
            (DataContext as AccountsDataPageViewModel)?.AccountsCardList.ControlResize(rect);
        }
    }
}
