using System.Text;
using System.Text.Json;
using System.Xml.Serialization;

namespace EjemploEntradaSalida
{
    internal class Program
    {
        private static readonly string _ejemploFileStreams = "EjemploStreams.txt";
        private static readonly string _ejemploTextStreams = "EjemploTextStreams.txt";
        private static readonly string _ejemploBufferedStreams = "EjemploBufferStreams.txt";

        // La intencion de este programa es mostrar algunos ejemplos sencillos de uso de Streams
        static async Task Main(string[] args)
        {
            try
            {
                // File y Directory son dos clases que permiten crear y manipular archivos y carpetas.
                // Son puntos de partida utiles para tareas tribiales.

                // Algunos ejemplos de metodos utiles son:
                // File.Copy
                // File.Exists
                // Directory.Delete
                // Directory.EnumerateFiles
                // Directory.CreateDirectory
                
                File.Delete(_ejemploFileStreams);
                File.Delete(_ejemploTextStreams);
                File.Delete(_ejemploBufferedStreams);

                var listaNombres = new List<string>()
                    {
                        "Adriano",
                        "Alan",
                        "Gustavo"
                    };

                // Ejemplo de lectura y escritura de un archivo generico utilizando FileStream
                await EjemploFileStream(listaNombres);

                // El mismo ejemplo, pero utilizando StreamReader y StreamWriter, que periten
                // manipular texto de forma mas sencilla, 
                await EjemploTextStreams(listaNombres);

                // Ejemplo simple de como agregar un buffer a un stream
                await EjemploBufferedTextStream(listaNombres);

                // Ejemplo de reader/writer usando un string/StringBuilder como subyancente
                await EjemploStringReader(listaNombres);

                // Los siguientes ejemplos muestran dos formatos estandar de serializacion
                // Serializacion a JSON
                EjemploSerializarJSON();

                // Serializacion a XML
                await EjemploSerializarXMLAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocurrio un error: {ex.Message}");
            }
        }

        private static async Task EjemploStringReader(IList<string> listaNombres)
        {
            var subyacente = "AAAAAAAAAXXXXXXXXXXXXXXXXXXX";

            await using (var writer = new StringWriter())
            {
                foreach (var nombre in listaNombres)
                {
                    await writer.WriteLineAsync(nombre);
                }
            }

            using (var reader = new StringReader(subyacente))
            {
                var linea = await reader.ReadToEndAsync();
                Console.WriteLine($"EjemploStringReader: {linea}");
            }
        }
        private static async Task EjemploBufferedTextStream(IList<string> listaNombres)
        {
            await using (FileStream fileStream = File.Open(_ejemploBufferedStreams, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                await using (var fileConBuffer = new BufferedStream(fileStream))
                {
                    await using (var writer = new StreamWriter(fileConBuffer))
                    {
                        foreach (var nombre in listaNombres)
                        {
                            await writer.WriteLineAsync(nombre);
                        }
                    }
                }
            }
        }
        private static async Task EjemploSerializarXMLAsync()
        {
            var mate = new Mate()
            {
                Color = "Azul",
                Propietario = "Adriano",
                CaracteristicasEspeciales = new string[] { "No se rompe", "Plastico", "Barato" }
            };

            // Es un stream de lectura/escritura que trabaja sobre ram
            await using (var output = new MemoryStream())
            {
                // Serializo
                var xmlSerializer = new XmlSerializer(typeof(Mate));
                xmlSerializer.Serialize(output, mate);

                output.Seek(0, SeekOrigin.Begin);
                using (var reader = new StreamReader(output))
                {
                    var xml = await reader.ReadToEndAsync();
                    Console.WriteLine("Mate como XML:");
                    Console.WriteLine(xml);

                    // Posiono nuevamente el stream al principio dado que el ReadToEnd lo movio al final 
                    output.Seek(0, SeekOrigin.Begin);

                    // Creo una instancia en base a un XML (Deserializo)
                    var mateDeserializado = xmlSerializer.Deserialize(reader) as Mate;
                    Console.WriteLine($"Comparando el mate original vs el serializado: {mate.Equals(mateDeserializado)}");
                }
            }
        }
        private static void EjemploSerializarJSON()
        {
            var mate = new Mate()
            {
                Color = "Azul",
                Propietario = "Adriano",
                CaracteristicasEspeciales = new string[] { "No se rompe", "Plastico", "Barato" }
            };

            // Serializo un objeto a formato JSON
            string jsonString = JsonSerializer.Serialize(mate);

            // Convierto un JSON a una instancia de un objeto (tengo que saber de que tipo es)
            Mate? mate2 = JsonSerializer.Deserialize<Mate>(jsonString);

            Console.WriteLine("Mate serializado como JSON:");
            Console.WriteLine(jsonString);
            Console.WriteLine($"Comparando el mate original vs el serializado: {mate.Equals(mate2)}");
        }
        private static async Task EjemploTextStreams(IList<string> listaNombres)
        {
            await using (FileStream fileStream = File.Open(_ejemploTextStreams, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                // Por defecto al "envolver" un steam con otro al cerrarse uno cierra al subyacente
                // el parametro leaveOpen me permite utilizar el mismo fileStream sin tener que volver a abrirlo
                await using (var writer = new StreamWriter(fileStream, leaveOpen: true))
                {
                    foreach (var nombre in listaNombres)
                    {
                        await writer.WriteLineAsync(nombre);
                    }
                }

                // Posiciono el cursor de vuelta al principio
                fileStream.Seek(0, SeekOrigin.Begin);

                using (var reader = new StreamReader(fileStream))
                {
                    while (!reader.EndOfStream)
                    {
                        // Leo desde el archivo y muestro en consola
                        var linea = await reader.ReadLineAsync();
                        Console.WriteLine($"EjemploTextStreams: {linea}");
                    }
                }
            }
        }
        private static async Task EjemploFileStream(IList<string> listaNombres)
        {
            // await using es una construccion identica al using visto hasta ahora, con la particularidad que el 
            // dispose se realiza de forma asincrona sin bloquear el thread principal.
            await using (FileStream fileStream = File.Open(_ejemploFileStreams, FileMode.OpenOrCreate))
            {
                // Posiciona el cursor al final del archivo
                fileStream.Seek(0, SeekOrigin.End);

                // Escribe secuncialmente cada nombre en una linea
                foreach (var nombre in listaNombres)
                {
                    // .Net 6 es multiplaforma real, el final de linea varia entre Mac,Linux y Windows
                    // Resulta importante codificar de forma correcta los caracteres cuadno lo que deseamos guardar es texto
                    var bytes = Encoding.UTF8.GetBytes($"{nombre}{Environment.NewLine}");

                    //Escribe al archivo
                    await fileStream.WriteAsync(bytes);
                }

                // Lee desde el archivo por bloques, es necesario crear un buffer
                var tamañoBuffer = 1024;
                var buffer = new byte[tamañoBuffer];

                // Muy importante: el cursor quedo en el fin del archivo, tengo que moverlo al principio
                fileStream.Seek(0, SeekOrigin.Begin);
                while (await fileStream.ReadAsync(buffer, 0, tamañoBuffer) > 0)
                {
                    var salida = Encoding.UTF8.GetString(buffer, 0, buffer.Length);

                    // Muestro por consola lo que lei desde el archivo
                    Console.WriteLine($"EjemploFileStream: {salida}");
                }
            }
        }
    }
}