using System.Net.Http.Json;
using ProyectoParcial1.Models;

namespace ProyectoParcial1.Services
{
    public class PokemonService
    {
        private readonly HttpClient httpClient;

        public PokemonService()
        {
            httpClient = new HttpClient();
        }

        public async Task<Pokemon> GetPokemonAsync(int id)
        {
            string url = $"https://pokeapi.co/api/v2/pokemon/{id}";

            try
            {
                // Realizamos la petición a PokéAPI.
                HttpResponseMessage response = await httpClient.GetAsync(url);

                // La API respondió, pero con un código HTTP de error.
                if (!response.IsSuccessStatusCode)
                {
                    int statusCode = (int)response.StatusCode;

                    throw new ApiException(
                        statusCode,
                        $"PokéAPI respondió con el código HTTP {statusCode}.");
                }

                // La respuesta fue correcta.
                // Deserializamos el JSON recibido.
                var data = await response.Content
                    .ReadFromJsonAsync<PokemonApiResponse>();

                if (data == null)
                {
                    throw new Exception(
                        "PokéAPI devolvió una respuesta vacía o inválida.");
                }

                // Convertimos los datos recibidos de la API
                // a nuestro modelo Pokemon.
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
                // Conservamos el error HTTP para que
                // el ViewModel pueda identificar su código.
                throw;
            }
            catch (HttpRequestException ex)
            {
                // Este caso corresponde a problemas para
                // establecer comunicación con la API.
                throw new Exception(
                    "No se pudo establecer conexión con PokéAPI. " +
                    "Verifique la conexión a Internet.",
                    ex);
            }
        }

        // Obtiene una lista de Pokémon.
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
}


// Representa la estructura del JSON recibido desde PokéAPI.
public class PokemonApiResponse
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public int Height { get; set; }

    public int Weight { get; set; }

    public PokemonSprites? Sprites { get; set; }
}


// Representa la parte "sprites" del JSON.
public class PokemonSprites
{
    public string? FrontDefault { get; set; }
}