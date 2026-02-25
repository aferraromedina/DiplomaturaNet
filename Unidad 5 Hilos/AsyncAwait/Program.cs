namespace EjemploJoin
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            // Ejecuta SaludarDesdeTarea en un hilo del ThreadPool y espera su finalización.
            // Este enfoque es útil cuando el trabajo es bloqueante (por ejemplo, E/S), porque evita
            // ocupar el hilo principal y mejora la capacidad de respuesta en escenarios concurrentes.
            await Task.Run(SaludarDesdeTarea);
            Console.WriteLine("Principal");
            
            // También se pueden paralelizar operaciones sobre colecciones.
            var numeros = new List<int>();
            for (int i = 0; i < 1500; i++)
            {
                numeros.Add(i);
            }

            // Ejecuta una acción por cada elemento, usando varios hilos cuando es posible.
            Parallel.ForEach(numeros, MostrarNumeros);

            // También se pueden lanzar varias acciones independientes al mismo tiempo.
            // Aquí se reutiliza el mismo método para simplificar el ejemplo, pero normalmente
            // serían tareas distintas que no dependen entre sí.
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
                // Simula una operación costosa o bloqueante.
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
