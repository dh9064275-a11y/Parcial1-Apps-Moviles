
using ProyectoParcial1.Models;

namespace ProyectoParcial1.Services;

public interface IPokemonService
{
    Task<Pokemon> GetPokemonAsync(int id);

    Task<List<Pokemon>> GetPokemonsAsync(int cantidad = 20);
}
