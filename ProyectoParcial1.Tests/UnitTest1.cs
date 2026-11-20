namespace ProyectoParcial1.Tests;

public class UnitTest1
{
    [Fact]
    public void Suma_DeberiaDarCuatro()
    {
        // valores que vamos a utilizar.
        int numero1 = 2;
        int numero2 = 2;

        // Operación que queremos comprobar.
        int resultado = numero1 + numero2;

        // verificamos que el resultado sea el esperado.
        Assert.Equal(4, resultado);
    }
}