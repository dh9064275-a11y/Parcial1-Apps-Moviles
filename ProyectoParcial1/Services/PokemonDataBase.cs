using SQLite;
using ProyectoParcial1.Models;

namespace ProyectoParcial1.Services;

public class PokemonDatabase
{
    private readonly SQLiteAsyncConnection _database;

    public PokemonDatabase(string dbPath)
    {
        _database = new SQLiteAsyncConnection(dbPath);
        _database.CreateTableAsync<Pokemon>().Wait();
    }

    public Task<List<Pokemon>> ObtenerPokemonsAsync()
    {
        return _database.Table<Pokemon>().ToListAsync();
    }

    public Task<int> GuardarPokemonAsync(Pokemon pokemon)
    {
        return _database.InsertOrReplaceAsync(pokemon);
    }

    public Task<int> EliminarPokemonsAsync()
    {
        return _database.DeleteAllAsync<Pokemon>();
    }
}