using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using ProyectoParcial1.Models;
using ProyectoParcial1.Services;

namespace ProyectoParcial1.ViewModels;

public class MainViewModel : INotifyPropertyChanged
{
    // Servicio que se comunica con la API (integrado por David Hernandez - integrante 1).
    private readonly PokemonService _pokemonService = new PokemonService();

    // Aca se definen las propiedades que se mostraran en la interfaz del usuario.
    public ObservableCollection<Pokemon> Pokemons { get; set; } = new ObservableCollection<Pokemon>();

    // Se define un mensaje para mostrar la accion que tiene que realizar el usuario o el estado del mismo.
    private string statusMsg = "Presiona 'Cargar datos' para iniciar.";
   
    // Se crea la propiedad del mensaje de estado que se mostrara en la interfaz.
    public string StatusMessage
    {
        get => statusMsg;
        set
        {
            // Se muestra el mensaje de estado en la interfaz de usuario.
            statusMsg = value;
            OnPropertyChanged(nameof(StatusMessage));
        }
    }

    // Se define una propiedad para indicar el estado del sistema de carga de datos.
    private bool isLoading;
    public bool IsLoading
    {
        get => isLoading;
        set
        {
            // Se muestra que esta cargando en la interfaz de usuario.
            isLoading = value;
            OnPropertyChanged(nameof(IsLoading));
        }
    }

    // Se crea un comando para darle la logica al boton de la interfaz de usuario que ejecutara la funcion CargarDatos.
    public ICommand LoadPokemonsCommand { get; set; }

    // Se define el constructor de la clase MainViewModel donde se inicializa el comando LoadPokemonsCommand.
    public MainViewModel()
    {
        // El comando ejecuta la funcion CargarDatos cuando se presiona el boton de la interfaz de usuario.
        LoadPokemonsCommand = new Command(async () => await CargarDatos());
    }

    // Se define la funcion CargarDatos que se ejecuta cuando se presiona el boton de la interfaz de usuario.
    private async Task CargarDatos()
    {
        // Si ya está cargando, ignora los siguientes clics para evitar multiples llamadas.
        if (IsLoading) return;

        // Si esta cargando, se muestra el mensaje de estado y se limpia la lista de Pokemons antes de cargar nuevos datos.
        IsLoading = true;
        StatusMessage = "Cargando datos desde la API...";
        Pokemons.Clear();

        try
        {
            // Se comunica con el servicio para obtener los datos de la API de Pokemon y se guarda en una variable.
            var pokemons = await _pokemonService.GetPokemonsAsync();

            // Se verifica que la variable no sea nula y tenga elementos para evitar errores y poder agregarlos a la lista visible en la interfaz de usuario.
            if (pokemons != null && pokemons.Count > 0)
            {
                // Por cada elemento obtenido se agrega a la lista.
                foreach (var pokemon in pokemons)
                {
                    Pokemons.Add(pokemon);
                }
                // Se muestra un mensaje de estado exitoso indicando la cantidad de elementos obtenidos.
                StatusMessage = $"¡Carga exitosa! Se obtuvieron {Pokemons.Count} elementos.";
            }
            else
            {
                // Caso contrario se muestra un mensaje de estado indicando que no se encontraron datos.
                StatusMessage = "No se encontraron datos.";
            }
        }
        catch (ApiException apiEx)
        {
            // Captura errores definidos en el Service.
            StatusMessage = apiEx.Message;
        }
        catch (Exception ex)
        {
            // Captura cualquier otro error no contemplado.
            StatusMessage = $"Error inesperado: {ex.Message}";
        }
        finally
        {
            // Se indica que ya no esta cargando para permitir nuevas llamadas al servicio.
            IsLoading = false; 
        }
    }

    // Se implementa la interfaz de notificacion de cambios para notificar a la interfaz de usuario cuando hay alguna modificacion.
    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged(string name) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}