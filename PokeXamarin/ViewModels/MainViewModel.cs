using System;
using System.Collections.ObjectModel;
using PokeXamarin.Helpers;
using PokeXamarin.Model;
using PokeXamarin.Services;
using PokeXamarin.ViewModels.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace PokeXamarin.ViewModels
{
    public class MainViewModel : ViewModelBase, IMainViewModel
    {
        public ObservableCollection<Pokemon> Pokemons { get; }
        IPokemonService _PokemonService;

        public MainViewModel(ILogger<MainViewModel> logger)
        {
            Title = "Poke Xamarin";
            Pokemons = new ObservableCollection<Pokemon>();
            _PokemonService = App.ServiceProvider.GetService<IPokemonService>();
            logger.LogCritical("Acessando o Aplicativo");

        }
    }
}
