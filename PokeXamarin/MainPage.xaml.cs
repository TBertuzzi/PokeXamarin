using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokeXamarin.ViewModels.Interfaces;
using Xamarin.Forms;

namespace PokeXamarin
{
    public partial class MainPage : ContentPage
    {
        public MainPage(IMainViewModel viewModel = null)
        {
            InitializeComponent();

            BindingContext = viewModel;
        }
    }
}
