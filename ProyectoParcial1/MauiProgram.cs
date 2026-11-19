using Microsoft.Extensions.Logging;
using ProyectoParcial1.Repositories;
using ProyectoParcial1.Services;
using ProyectoParcial1.ViewModels;

namespace ProyectoParcial1;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // Registro de servicios
        builder.Services.AddSingleton<HttpClient>();
        builder.Services.AddSingleton<IPokemonService, PokemonService>();

        // Registro de la base de datos SQLite
        string dbPath = Path.Combine(
            FileSystem.AppDataDirectory,
            "pokemon.db3"
        );

        builder.Services.AddSingleton<PokemonDatabase>(
            s => new PokemonDatabase(dbPath)
        );

        // Registro del Repository
        builder.Services.AddSingleton<IPokemonRepository, PokemonRepository>();

        // Registro de ViewModels y páginas
        builder.Services.AddTransient<MainViewModel>();
        builder.Services.AddTransient<MainPage>();

        // Registro de la navegación principal
        builder.Services.AddSingleton<AppShell>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}