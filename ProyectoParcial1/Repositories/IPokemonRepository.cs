using ProyectoParcial1.Models;

namespace ProyectoParcial1.Repositories;

public interface IPokemonRepository
{
    Task<List<Pokemon>> ObtenerPokemonsAsync();

    Task<int> GuardarPokemonAsync(Pokemon pokemon);

    Task<int> EliminarPokemonsAsync();
}