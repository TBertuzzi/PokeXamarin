using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using PokeXamarin.Model;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace PokeXamarin
{
    public partial class PokemonProfilePage : PopupPage, INotifyPropertyChanged
    {
        #region Property

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(storage, value))
            {
                return false;
            }

            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        #endregion

        private Pokemon _pokemon;
        public Pokemon Pokemon
        {
            get => _pokemon;
            set => SetProperty(ref _pokemon, value);
        }

        public PokemonProfilePage(Pokemon pokemon)
        {
            InitializeComponent();

            this.BindingContext = this;

            Pokemon = pokemon;
        }

        private async void OnClose(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
        }

       
    }
}
