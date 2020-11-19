using System;
using System.Collections.ObjectModel;
using PokeXamarin.Helpers;
using PokeXamarin.Model;
using PokeXamarin.Services;
using PokeXamarin.ViewModels.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Net.Http;
using Acr.UserDialogs;
using System.Windows.Input;
using Xamarin.Forms;
using Rg.Plugins.Popup.Services;
using System.Linq;
using MonkeyCache.LiteDB;
using System.Collections.Generic;
using Xamarin.Essentials;

namespace PokeXamarin.ViewModels
{
    public class MainViewModel : ViewModelBase, IMainViewModel
    {
        private string _key = "pokemon";

        public ObservableCollection<Pokemon> Pokemons { get; }
        IPokemonService _PokemonService;
        bool IsNotConnected { get; set; }

        private ICommand _itemTappedCommand;
        public ICommand ItemTappedCommand => _itemTappedCommand ?? (_itemTappedCommand =
            new Command<Pokemon>(async (pokemon) => await ItemTappedCommandExecute(pokemon), (pokemon) => !IsBusy));

        public MainViewModel(ILogger<MainViewModel> logger)
        {
            IsNotConnected = false;
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            Title = "Poke Xamarin";
            Pokemons = new ObservableCollection<Pokemon>();
            _PokemonService = App.ServiceProvider.GetService<IPokemonService>();
            logger.LogCritical("Acessando o Aplicativo");

        }

        async void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            var IsNotConnected = e.NetworkAccess != NetworkAccess.Internet;

            if (IsNotConnected)
                await Application.Current.MainPage.DisplayAlert("Atenção", "Estamos sem internet :(", "OK");
        }

        public async Task Carregar()
        {
            try
            {
                using (var Dialog = UserDialogs.Instance.Loading("Carregando Pokemons", null, null, true, MaskType.Black))
                {
                    var existingList = Barrel.Current.Get<List<Pokemon>>(_key) ?? new List<Pokemon>();
                    Pokemons.Clear();


                    if (existingList.Count == 0 && !IsNotConnected)
                        await GravarPokemons();
                    else
                    {
                        foreach (var pokemon in existingList)
                        {
                            Pokemons.Add(pokemon);
                        }
                    }
                }

            }
            catch (Exception ex)
            {

            }
            finally
            {

            }
        }

        private async Task GravarPokemons()
        {
            IsBusy = true;

            var pokemons = await _PokemonService.GetPokemonsAsync();

            if (pokemons != null && pokemons.Count > 0)
            {
                Pokemons.Clear();

                var existingList = Barrel.Current.Get<List<Pokemon>>(_key) ?? new List<Pokemon>();

                foreach (var pokemon in pokemons)
                {
                    var isExist = existingList.Any(e => e.Id == pokemon.Id);

                    pokemon.Image = ImageHelpers.GetImageStreamFromUrl(pokemon.Sprites.FrontDefault.AbsoluteUri);

                    pokemon.ImageBack = ImageHelpers.GetImageStreamFromUrl(pokemon.Sprites.BackDefault.AbsoluteUri);

                    pokemon.AllTypes = String.Join(",", pokemon.Types.Select(p => p.Type.Name));

                    if (!isExist)
                    {
                        existingList.Add(pokemon);
                    }

                    Pokemons.Add(pokemon);
                }

                existingList = existingList.OrderBy(e => e.Id).ToList();

                Barrel.Current.Add(_key, existingList, TimeSpan.FromDays(30));
            }

            IsBusy = false;
        }



        private async Task ItemTappedCommandExecute(Pokemon pokemon)
        {
            var page = new PokemonProfilePage(pokemon);

            await PopupNavigation.Instance.PushAsync(page);
        }
    }
}
