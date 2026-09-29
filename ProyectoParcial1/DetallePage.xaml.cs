using ProyectoParcial1.Models;

namespace ProyectoParcial1;

public partial class DetallePage : ContentPage
{
    public DetallePage(Pokemon pokemon)
    {
        InitializeComponent();

        // Se establece el Pokémon seleccionado como contexto de datos
        // para mostrar sus propiedades en la interfaz.
        BindingContext = pokemon;

        // Muestra en la consola la URL de la imagen para verificar que fue recibida correctamente.
        System.Diagnostics.Debug.WriteLine($"IMAGEN POKEMON: {pokemon.ImageUrl}");
    }
}