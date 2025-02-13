namespace EjemploJoin
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            // Ejecua en paralelo este metodo y espera a que termine, util para las llamadas bloqueantes (Entrada Salida)
            // dado que el hilo que queda bloqueado es un hilo dentro del pool y no la propia aplicacion
            // esto es especialmente util en entornos concurrentes (Aplicaciones webs, servicios Rest, etc)
            await Task.Run(SaludarDesdeTarea);
            Console.WriteLine("Principal");

            // Tambien se puede ejecutar tareas en paralelo sobre listas
            var numeros = new List<int>();
            for (int i = 0; i < 1500; i++)
            {
                numeros.Add(i);
            }

            // Ejecuta una accion (en paralelo si es posible) sobre una coleccion
            Parallel.ForEach(numeros, MostrarNumeros);

            // Tambien existe la posibilidad de ejecutar muchas acciones en paralelo
            // Para simplificar repito el mismo metodo, pero en un uso real seria diferentes acciones
            // que tengan sentido lanzarlas en paralelo.
            var actions = new Action[]
            {
                Saludar,
                Saludar,
                Saludar,
            };
            Parallel.Invoke(actions);
        }
        static void SaludarDesdeTarea()
        {
            for (int i = 0; i < 10; i++)
            {
                // Simulamos un tiempo de trabajo
                Thread.Sleep(200);
                Console.WriteLine("Hola desde una tarea");
            }
        }

        private static void Saludar()
        {
            Console.WriteLine($"hola, desde un hilo manejado por el framework: {Thread.CurrentThread.ManagedThreadId}");
        }

        private static void MostrarNumeros(int i)
        {
            Console.WriteLine($"Numero {i}");
        }
    }
}