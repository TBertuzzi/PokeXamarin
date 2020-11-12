using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PokeXamarin.Model;

namespace PokeXamarin.Services
{
    public interface IPokemonService
    {
        Task<List<Pokemon>> GetPokemonsAsync();
    }
}
