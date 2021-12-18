using Chess.Models;
using Chess.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Chess.Views
{
    public partial class PlayWithComputerPage : ContentPage
    {
        public Item Item { get; set; }

        public PlayWithComputerPage()
        {
            InitializeComponent();
            BindingContext = new NewItemViewModel();
        }
    }
}