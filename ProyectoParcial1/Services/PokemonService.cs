
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using ProyectoParcial1.Models;

namespace ProyectoParcial1.Services;

public class PokemonService : IPokemonService
{
    private readonly HttpClient httpClient;

    public PokemonService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<Pokemon> GetPokemonAsync(int id)
    {
        string url = $"https://pokeapi.co/api/v2/pokemon/{id}";

        try
        {
            HttpResponseMessage response = await httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                int statusCode = (int)response.StatusCode;

                throw new ApiException(
                    statusCode,
                    $"PokéAPI respondió con el código HTTP {statusCode}.");
            }

            var data = await response.Content
                .ReadFromJsonAsync<PokemonApiResponse>();

            if (data == null)
            {
                throw new Exception(
                    "PokéAPI devolvió una respuesta vacía o inválida.");
            }

            return new Pokemon
            {
                Id = data.Id,
                Name = data.Name,
                Height = data.Height,
                Weight = data.Weight,
                ImageUrl = data.Sprites?.FrontDefault ?? string.Empty
            };
        }
        catch (ApiException)
        {
            throw;
        }
        catch (HttpRequestException ex)
        {
            throw new Exception(
                "No se pudo establecer conexión con PokéAPI. " +
                "Verifique la conexión a Internet.",
                ex);
        }
    }

    public async Task<List<Pokemon>> GetPokemonsAsync(int cantidad = 20)
    {
        var pokemons = new List<Pokemon>();

        for (int id = 1; id <= cantidad; id++)
        {
            Pokemon pokemon = await GetPokemonAsync(id);
            pokemons.Add(pokemon);
        }

        return pokemons;
    }
}

public class PokemonApiResponse
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public int Height { get; set; }

    public int Weight { get; set; }

    public PokemonSprites? Sprites { get; set; }
}

public class PokemonSprites
{
    [JsonPropertyName("front_default")]
    public string? FrontDefault { get; set; }
}
