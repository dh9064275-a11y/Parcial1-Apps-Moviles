using ProyectoParcial1.Models;
using ProyectoParcial1.Services;

namespace ProyectoParcial1.Repositories;

public class PokemonRepository : IPokemonRepository
{
    private readonly PokemonDatabase _database;

    public PokemonRepository(PokemonDatabase database)
    {
        _database = database;
    }

    public Task<List<Pokemon>> ObtenerPokemonsAsync()
    {
        return _database.ObtenerPokemonsAsync();
    }

    public Task<int> GuardarPokemonAsync(Pokemon pokemon)
    {
        return _database.GuardarPokemonAsync(pokemon);
    }

    public Task<int> EliminarPokemonsAsync()
    {
        return _database.EliminarPokemonsAsync();
    }
}