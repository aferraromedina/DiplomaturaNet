namespace EjemploJoin
{
    public class Program
    {
        static void Main(string[] args)
        {
            // Este codigo genera 3 hilos, uno Principal que es creado de forma implicita
            // y dos que creamos de forma explicita.
            var hiloUNO = new Thread(new ParameterizedThreadStart(Saludar));
            var hiloDOS = new Thread(new ParameterizedThreadStart(Saludar));

            hiloUNO.Start("UNO");
            hiloDOS.Start("DOS");

            // Esta linea hace que el Thread que la invoca (en este caso "Principal") espere hasta que
            // El Thread UNO termine.
            // Si comentamos estas 2 lineas, los 3 hilos van a ejecutarse en paralelo y el orden de la salida no esta
            // garantizado
            hiloUNO.Join();
            hiloDOS.Join();

            Saludar($"Soy el Thread PRINCIPAL Id {Thread.CurrentThread.ManagedThreadId}");
        }

        private static void Saludar(object? nombre)
        {
            Thread.Sleep(100);
            Console.WriteLine($"Hola, soy {nombre} desde el hilo {Thread.CurrentThread.ManagedThreadId}");
        }
    }
}