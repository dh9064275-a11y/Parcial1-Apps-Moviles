
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ProyectoParcial1.Models;
using ProyectoParcial1.Services;

namespace ProyectoParcial1.ViewModels;

public class MainViewModel : INotifyPropertyChanged
{
    private readonly IPokemonService _pokemonService;

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

    public MainViewModel(IPokemonService pokemonService)
    {
        _pokemonService = pokemonService;

        // Ejecuta CargarDatos cuando se presiona el botón.
        LoadPokemonsCommand = new Command(async () => await CargarDatos());

        // Ejecuta SeleccionarPokemon cuando se selecciona un Pokémon.
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

            Pokemons.Clear();

            var lista = await _pokemonService.GetPokemonsAsync();

            foreach (var pokemon in lista)
            {
                Pokemons.Add(pokemon);
            }

            StatusMessage = $"Se cargaron {Pokemons.Count} Pokémon.";
        }
        catch (ApiException ex)
        {
            StatusMessage = $"Error al consultar la API: {ex.Message}";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Ocurrió un error: {ex.Message}";
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
