
using ProyectoParcial1.Models;
using ProyectoParcial1.Services;
using SQLite;

namespace ProyectoParcial1.Tests;

public class PokemonDatabaseTests
{
    [Fact]
    public async Task GuardarPokemon_CuandoSeConsulta_DevuelvePokemonGuardado()
    {
        // creamos una base SQLite en memoria.
        var database = CrearBaseEnMemoria();

        var pokemon = new Pokemon
        {
            Id = 25,
            Name = "pikachu",
            Height = 4,
            Weight = 60,
            ImageUrl = "https://ejemplo.com/pikachu.png"
        };

        // guardamos y consultamos.
        await database.GuardarPokemonAsync(pokemon);

        var pokemons = await database.ObtenerPokemonsAsync();

        // verificamos los datos guardados.
        Assert.Single(pokemons);
        Assert.Equal(25, pokemons[0].Id);
        Assert.Equal("pikachu", pokemons[0].Name);
        Assert.Equal(4, pokemons[0].Height);
        Assert.Equal(60, pokemons[0].Weight);
    }

    [Fact]
    public async Task GuardarPokemon_CuandoYaExiste_ActualizaSusDatos()
    {
        // Arrange
        var database = CrearBaseEnMemoria();

        var pokemonOriginal = new Pokemon
        {
            Id = 25,
            Name = "pikachu",
            Height = 4,
            Weight = 60
        };

        var pokemonActualizado = new Pokemon
        {
            Id = 25,
            Name = "pikachu",
            Height = 5,
            Weight = 65
        };

        // Act
        await database.GuardarPokemonAsync(pokemonOriginal);
        await database.GuardarPokemonAsync(pokemonActualizado);

        var pokemons = await database.ObtenerPokemonsAsync();

        // Assert
        Assert.Single(pokemons);
        Assert.Equal(25, pokemons[0].Id);
        Assert.Equal(5, pokemons[0].Height);
        Assert.Equal(65, pokemons[0].Weight);
    }

    [Fact]
    public async Task EliminarPokemons_CuandoHayRegistros_DejaBaseVacia()
    {
        // Arrange
        var database = CrearBaseEnMemoria();

        var pokemon = new Pokemon
        {
            Id = 1,
            Name = "bulbasaur",
            Height = 7,
            Weight = 69
        };

        await database.GuardarPokemonAsync(pokemon);

        // Act
        await database.EliminarPokemonsAsync();

        var pokemons = await database.ObtenerPokemonsAsync();

        // Assert
        Assert.Empty(pokemons);
    }

    [Fact]
    public async Task BasesEnMemoria_DebenSerIndependientes()
    {
        // Arrange: creamos dos bases independientes.
        var database1 = CrearBaseEnMemoria();
        var database2 = CrearBaseEnMemoria();

        var pokemon = new Pokemon
        {
            Id = 25,
            Name = "pikachu",
            Height = 4,
            Weight = 60
        };

        // Act: guardamos solamente en la primera.
        await database1.GuardarPokemonAsync(pokemon);

        var pokemonsBase1 = await database1.ObtenerPokemonsAsync();
        var pokemonsBase2 = await database2.ObtenerPokemonsAsync();

        // Assert: los datos no deben compartirse.
        Assert.Single(pokemonsBase1);
        Assert.Empty(pokemonsBase2);
    }

    // Crea una conexión en memoria con identidad única.
    private static PokemonDatabase CrearBaseEnMemoria()
    {
        string nombreUnico = Guid.NewGuid().ToString("N");

        var connectionString = new SQLiteConnectionString(
            $"file:{nombreUnico}?mode=memory&cache=shared",
            true);

        return new PokemonDatabase(connectionString);
    }
}
