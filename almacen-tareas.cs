using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Windows.Storage;

namespace listas_pendientes
{
    public static class almacen_tareas
    {
        private static string Ruta =>
            Path.Combine(ApplicationData.Current.LocalFolder.Path, "tareas.json");

        public static void Guardar(List<Tarea> todas)
        {
            string texto = JsonSerializer.Serialize(todas);
            File.WriteAllText(Ruta, texto);
            System.Diagnostics.Debug.WriteLine(Ruta);
        }

        public static List<Tarea> Cargar()
        {
            if (!File.Exists(Ruta)) return new List<Tarea>();

            string texto = File.ReadAllText(Ruta);
            return JsonSerializer.Deserialize<List<Tarea>>(texto) ?? new List<Tarea>();
        }

    }
}
