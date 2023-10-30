
namespace EjemploMutex
{
    public class Program
    {
        // Permite alternar la ejeucion entre 2 hilos/procesos, es un emboltorio de la funcionalidad del sistema operativo
        // por lo que sirve tambien para comunicar procesos diferentes.
        private static Mutex MutualExclusionLock = new Mutex();
        static void Main(string[] args)
        {
            // En este ejemplo, simulamos la sincronizcion entre 2 hilos, uno "rapido" y uno "lento"
            // pero no necesariamente tiene que haber una diferencia de tiempo entre uno y otro.

            // La clase Mutex resulve de forma simple la necesidad de tener 2 hilos y querer ejecutarlos
            // de forma alternativa donde ambos esperan a que el otro hilo "avise" que ya puede arrancar.
            // Tambien sirve para sincronizar hilos entre diferentes procesos.
            // Un ejemplo sencillo seria el no permitir la ejecucion de mas de una instancia del programa
            var hiloUNO = new Thread(new ThreadStart(TrabajoPesado));
            var hiloDOS = new Thread(new ThreadStart(TrabajoLiviano));

            hiloUNO.Start();
            hiloDOS.Start();
        }
        private static void TrabajoPesado()
        {
            for (int i = 0; i < 5; i++)
            {
                // Intento ejecutar, si otro hilo ya lo llamo, me quedo esperando a que me despierten
                MutualExclusionLock.WaitOne();

                // Simulo un trabajo
                Thread.Sleep(1000);
                Console.WriteLine($"Soy el trabajo Pesado ThreadId: {Thread.CurrentThread.ManagedThreadId}");

                // "Avsamos" al otro hilo que ya puede arrancar
                MutualExclusionLock.ReleaseMutex();
            }
        }

        private static void TrabajoLiviano()
        {
            for (int i = 0; i < 5; i++)
            {
                // Intento ejecutar, si otro hilo ya lo llamo, me quedo esperando a que me despierten
                MutualExclusionLock.WaitOne();

                // Simulo un trabajo
                Thread.Sleep(1);
                Console.WriteLine($"Soy el trabajo Liviano ThreadId: {Thread.CurrentThread.ManagedThreadId}");

                // "Avsamos" al otro hilo que ya puede arrancar
                MutualExclusionLock.ReleaseMutex();
            }
        }
    }
}