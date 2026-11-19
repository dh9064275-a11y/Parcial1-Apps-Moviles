using ProyectoParcial1.ViewModels;
namespace ProyectoParcial1;

public partial class MainPage : ContentPage
{
    public MainPage(MainViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
