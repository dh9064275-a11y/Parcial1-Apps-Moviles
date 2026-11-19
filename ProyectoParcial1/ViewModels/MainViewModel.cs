using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ProyectoParcial1.Models;
using ProyectoParcial1.Services;
using ProyectoParcial1.Repositories;

namespace ProyectoParcial1.ViewModels;

public class MainViewModel : INotifyPropertyChanged
{
    private readonly IPokemonService _pokemonService;
    private readonly IPokemonRepository _pokemonRepository;

    public ObservableCollection<Pokemon> Pokemons { get; set; } = new();

    private string _statusMessage = "Presioná el botón para cargar los Pokémon.";
    public string StatusMessage
    {
        get => _statusMessage;
        set
        {
            _statusMessage = value;
            OnPropertyChanged();
        }
    }

    private bool _isLoading;
    public bool IsLoading
    {
        get => _isLoading;
        set
        {
            _isLoading = value;
            OnPropertyChanged();
        }
    }

    public Command LoadPokemonsCommand { get; }
    public Command<Pokemon> SelectPokemonCommand { get; }

    public MainViewModel(
    IPokemonService pokemonService,
    IPokemonRepository pokemonRepository)
    {
        _pokemonService = pokemonService;
        _pokemonRepository = pokemonRepository;

        LoadPokemonsCommand = new Command(async () => await CargarDatos());

        SelectPokemonCommand =
            new Command<Pokemon>(async (pokemon) => await SeleccionarPokemon(pokemon));
    }


    private async Task CargarDatos()
    {
        if (IsLoading)
            return;

        try
        {
            IsLoading = true;
            StatusMessage = "Cargando Pokémon...";

            try
            {
                // Obtener los Pokémon desde la API.
                var lista = await _pokemonService.GetPokemonsAsync();

                // Guardar cada Pokémon en la base de datos local.
                foreach (var pokemon in lista)
                {
                    await _pokemonRepository.GuardarPokemonAsync(pokemon);
                }

                // Mostrar los Pokémon descargados.
                Pokemons.Clear();

                foreach (var pokemon in lista)
                {
                    Pokemons.Add(pokemon);
                }

                StatusMessage = $"Se cargaron {Pokemons.Count} Pokémon.";
            }
            catch (Exception)
            {
                // Si falla la conexión, intentar recuperar los datos locales.
                var pokemonsGuardados =
                    await _pokemonRepository.ObtenerPokemonsAsync();

                Pokemons.Clear();

                foreach (var pokemon in pokemonsGuardados)
                {
                    Pokemons.Add(pokemon);
                }

                if (Pokemons.Count > 0)
                {
                    StatusMessage =
                        $"Sin conexión. Se muestran {Pokemons.Count} Pokémon guardados.";
                }
                else
                {
                    StatusMessage =
                        "No se pudo conectar con la API y no hay Pokémon guardados.";
                }
            }
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error al recuperar los Pokémon: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }


    private async Task SeleccionarPokemon(Pokemon pokemon)
    {
        if (pokemon == null)
            return;

        await Shell.Current.Navigation.PushAsync(new DetallePage(pokemon));
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}