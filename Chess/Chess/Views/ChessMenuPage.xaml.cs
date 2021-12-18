using Chess.Models;
using Chess.ViewModels;
using Chess.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Chess.Views
{
    public partial class ChessMenuPage : ContentPage
    {
        ItemsViewModel _viewModel;

        public ChessMenuPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new ItemsViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
        async void OnPlayWithComputerButtonClicked(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new PlayWithComputerPage());
        }        
        async void OnPlayWithHumanButtonClicked(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new PlayWithHumanPage());
        }        
        async void OnRulesButtonClicked(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new RulesPage());
        }

    }
}