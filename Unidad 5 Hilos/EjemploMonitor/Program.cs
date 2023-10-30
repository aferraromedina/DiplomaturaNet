
namespace EjemploMonitor
{
    public class Program
    {
        private static volatile List<int> Numeros = new List<int>();
        static void Main(string[] args)
        {
            // Creamos 
            var productor = new Thread(new ThreadStart(Productor));
            var consumidor = new Thread(new ThreadStart(Consumidor));

            productor.Start();
            productor.Start();
        }

        private static void Consumidor()
        {
            while (true)
            {
                // Si nadie tiene tomado un bloqueo sobre este recurso, ingresa, de lo contrario queda bloqueado hasta que alguien lo libere
                // Tambien se puede simplificar por la instruccion lock, dado que en este ejemplo no estamos teniendo en cuenta
                // que por ejemplo, una excepcion puede causar que nunca se libere el bloqueo.
                
                // Ejemplo usando lock en lugar de trabajar directo con Monitor:
                //lock(Numeros)
                //{
                //    // Sumamos todos los Numeros que creo el productor
                //    decimal resultado = 0;
                //    foreach (var numero in Numeros)
                //    {
                //        resultado += numero;
                //    }
                //    var x = Numeros.Count();
                //    // Limpiamos los numeros
                //    Numeros.Clear();
                //    Console.WriteLine($"Soy el CONSUMIDOR, Sume {x} Numeros y el resultado fue {resultado}");
                //}

                Monitor.Enter(Numeros);

                // Sumamos todos los Numeros que creo el productor
                decimal resultado = 0;
                foreach (var numero in Numeros)
                {
                    resultado += numero;
                }

                var x = Numeros.Count();
                // Limpiamos los numeros
                Numeros.Clear();

                // Libera el bloqueo
                Monitor.Exit(Numeros);
                Console.WriteLine($"Soy el CONSUMIDOR, Sume {x} Numeros y el resultado fue {resultado}");
            }
        }

        private static void Productor()
        {
            while (true) 
            {
                // Si nadie tiene tomado un bloqueo sobre este recurso, ingresa, de lo contrario queda bloqueado hasta que alguien lo libere
                Monitor.Enter(Numeros);

                // Genera entre 0 y 500 numeros aleatorios
                var cantidadDeNumerosAGenerar = Random.Shared.Next(500);

                for (int i = 0; i < cantidadDeNumerosAGenerar; i++)
                {
                    var random = cantidadDeNumerosAGenerar;
                    Numeros.Add(random);
                }
                Thread.Sleep(1000);
                Console.WriteLine($"Soy el PRODUCTOR, cree {cantidadDeNumerosAGenerar} Numeros aleatorios");
                
                // Libera el bloqueo
                Monitor.Exit(Numeros);
            }
        }
    }
}