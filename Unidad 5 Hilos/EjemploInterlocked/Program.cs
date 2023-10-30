using System.Threading;

namespace EjemploInterlocked
{
    public class Program
    {
        // El atributo que vamos a actualizar desde los hilos
        private static int SaldoInterlocked = 0;

        //Volaitle solo informa al compilador de que este atributo va a ser accedido desde mas de 1 hilo,
        // pero no evita las condiciones de carrera

        private static volatile int Saldo = 0;
        static void Main(string[] args)
        {
            var threads = new List<Thread>();

            // Creo 20 hilos
            for (int i = 0; i < 20; i++)
            {
                // Algunos hilos van a sumar en el contador y otros restar
                var threadStart = i % 2 == 0 ? new ThreadStart(Sumar) : new ThreadStart(Restar);
                var thread = new Thread(threadStart);
                threads.Add(thread);

                // Arranco la ejecucion del hilo
                thread.Start();
            }

            // Espero a que todos terminen
            threads.ForEach(x => x.Join());

            // Muestro el saldo, dado que todos los hilos suman y restan en paralelo, 
            // lo mas probable es que el resultado difira debido alguna condicion de carrera
            Console.WriteLine($"Saldo sin sincronizar {Saldo}, saldo usando interlocked {SaldoInterlocked}");
        }

        private static void Sumar()
        {
            for (int i = 0; i < 100000; i++)
            {
                Saldo++;
                Interlocked.Increment(ref SaldoInterlocked);
            }
        }

        private static void Restar()
        {
            for (int i = 0; i < 100000; i++)
            {
                Interlocked.Decrement(ref SaldoInterlocked);
                Saldo--;
            }
        }
    }
}
