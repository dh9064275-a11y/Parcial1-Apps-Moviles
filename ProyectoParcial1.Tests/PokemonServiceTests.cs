using System.Net;
using ProyectoParcial1.Services;

namespace ProyectoParcial1.Tests;

public class PokemonServiceTests
{
    [Fact]
    public async Task GetPokemonAsync_CuandoApiDevuelve404_LanzaApiException()
    {
        // simulamos una respuesta HTTP 404.
        var handler = new FakeHttpMessageHandler(
            HttpStatusCode.NotFound);

        var httpClient = new HttpClient(handler);
        var service = new PokemonService(httpClient);

        // ejecutamos el servicio y capturamos el error.
        var exception = await Assert.ThrowsAsync<ApiException>(
            () => service.GetPokemonAsync(99999));

        // comprobamos que el error sea 404.
        Assert.Equal(404, exception.StatusCode);
    }

    [Fact]
    public async Task GetPokemonAsync_CuandoRespuestaEsCorrecta_DevuelvePokemon()
    {
        // Arrange: simulamos los datos de Pikachu.
        string json = """
    {
        "id": 25,
        "name": "pikachu",
        "height": 4,
        "weight": 60,
        "sprites": {
            "front_default": "https://ejemplo.com/pikachu.png"
        }
    }
    """;

        var handler = new FakeHttpMessageHandler(
            HttpStatusCode.OK, json);

        var httpClient = new HttpClient(handler);
        var service = new PokemonService(httpClient);

        // Act
        var pokemon = await service.GetPokemonAsync(25);

        // Assert
        Assert.Equal(25, pokemon.Id);
        Assert.Equal("pikachu", pokemon.Name);
        Assert.Equal(4, pokemon.Height);
        Assert.Equal(60, pokemon.Weight);
        Assert.Equal(
            "https://ejemplo.com/pikachu.png",
            pokemon.ImageUrl);
    }

    [Fact]
    public async Task GetPokemonAsync_CuandoFallaConexion_MuestraError()
    {
        // simulamos una falla de conexión.
        var handler = new FakeHttpMessageHandler(
            simularErrorConexion: true);

        var httpClient = new HttpClient(handler);
        var service = new PokemonService(httpClient);

        // Act
        var exception = await Assert.ThrowsAsync<Exception>(
            () => service.GetPokemonAsync(25));

        // Assert
        Assert.Contains(
            "No se pudo establecer conexión",
            exception.Message);
    }

    [Fact]
    public async Task GetPokemonAsync_CuandoApiDevuelve500_LanzaApiException()
    {
        // Arrange: simulamos un error del servidor.
        var handler = new FakeHttpMessageHandler(
            HttpStatusCode.InternalServerError);

        var httpClient = new HttpClient(handler);
        var service = new PokemonService(httpClient);

        // Act
        var exception = await Assert.ThrowsAsync<ApiException>(
            () => service.GetPokemonAsync(25));

        // Assert
        Assert.Equal(500, exception.StatusCode);
    }

    [Fact]
    public async Task GetPokemonsAsync_CuandoCantidadEsCero_DevuelveListaVacia()
    {
        // Arrange
        var handler = new FakeHttpMessageHandler();
        var httpClient = new HttpClient(handler);
        var service = new PokemonService(httpClient);

        // Act
        var pokemons = await service.GetPokemonsAsync(0);

        // Assert
        Assert.Empty(pokemons);
    }
}

// Simula una respuesta de PokeAPI sin utilizar Internet.
public class FakeHttpMessageHandler : HttpMessageHandler
{
    private readonly HttpStatusCode statusCode;
    private readonly string? contenido;
    private readonly bool simularErrorConexion;

    public FakeHttpMessageHandler(
        HttpStatusCode statusCode = HttpStatusCode.OK,
        string? contenido = null,
        bool simularErrorConexion = false)
    {
        this.statusCode = statusCode;
        this.contenido = contenido;
        this.simularErrorConexion = simularErrorConexion;
    }

    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        if (simularErrorConexion)
        {
            throw new HttpRequestException(
                "Error de conexión simulado.");
        }

        var response = new HttpResponseMessage(statusCode);

        if (contenido != null)
        {
            response.Content = new StringContent(
                contenido,
                System.Text.Encoding.UTF8,
                "application/json");
        }

        return Task.FromResult(response);
    }
}