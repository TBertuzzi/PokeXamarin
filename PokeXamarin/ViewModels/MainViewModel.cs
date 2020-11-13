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

namespace PokeXamarin.ViewModels
{
    public class MainViewModel : ViewModelBase, IMainViewModel
    {
        public ObservableCollection<Pokemon> Pokemons { get; }
        IPokemonService _PokemonService;

        private ICommand _itemTappedCommand;
        public ICommand ItemTappedCommand => _itemTappedCommand ?? (_itemTappedCommand =
            new Command<Pokemon>(async (pokemon) => await ItemTappedCommandExecute(pokemon), (pokemon) => !IsBusy));

        public MainViewModel(ILogger<MainViewModel> logger)
        {
            Title = "Poke Xamarin";
            Pokemons = new ObservableCollection<Pokemon>();
            _PokemonService = App.ServiceProvider.GetService<IPokemonService>();
            logger.LogCritical("Acessando o Aplicativo");

        }

        public async Task Carregar()
        {
            try
            {
                using (var Dialog = UserDialogs.Instance.Loading("Carregando Pokemons", null, null, true, MaskType.Black))
                {
                    var pokemons = await _PokemonService.GetPokemonsAsync();

                    Pokemons.Clear();

                    foreach (var pokemon in pokemons)
                    {
                        pokemon.Image = GetImageStreamFromUrl(pokemon.Sprites.FrontDefault.AbsoluteUri);
                        Pokemons.Add(pokemon);
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

        private static byte[] GetImageStreamFromUrl(string url)
        {
            try
            {
                using (var webClient = new HttpClient())
                {
                    var imageBytes = webClient.GetByteArrayAsync(url).Result;

                    return imageBytes;

                }
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                return null;

            }
        }

        private async Task ItemTappedCommandExecute(Pokemon pokemon)
        {
            var teste = pokemon;
        }
    }
}
