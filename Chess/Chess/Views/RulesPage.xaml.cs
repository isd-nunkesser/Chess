using Chess.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace Chess.Views
{
    public partial class RulesPage : ContentPage
    {
        public RulesPage()
        {
            InitializeComponent();
            BindingContext = new ChessMenuViewModel();
        }
    }
}