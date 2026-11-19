
namespace ProyectoParcial1;

public partial class AppShell : Shell
{
    public AppShell(MainPage mainPage)
    {
        InitializeComponent();

        MainPageContent.ContentTemplate =
            new DataTemplate(() => mainPage);
    }
}
