using Chess.ViewModels;
using Chess.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Chess
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(RulesPage), typeof(RulesPage));
            Routing.RegisterRoute(nameof(PlayWithComputerPage), typeof(PlayWithComputerPage));
        }

    }
}
