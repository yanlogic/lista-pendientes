using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lista_pendientes
{
    public enum Tipo_tarea
    {
        general,
        academica,
        aireLibre,
        cocina,
    }

    public static class detector_tipo
    {
        private static readonly string[] palabrasAcademicas =
            { "informe", "exposición", "examen", "ensayo", "estudiar", "investigar", "monografía" };

        private static readonly string[] palabrasAireLibre =
            { "pasear", "parque", "correr", "caminar", "trotar" };

        private static readonly string[] palabrasCocina =
            { "cocinar", "cena", "almuerzo", "desayuno", "receta", "preparar" };

        public static Tipo_tarea Detectar(string nombre)
        {
            if (ContieneAlguna(nombre, palabrasAcademicas)) return Tipo_tarea.academica;
            if (ContieneAlguna(nombre, palabrasAireLibre)) return Tipo_tarea.aireLibre;
            if (ContieneAlguna(nombre, palabrasCocina)) return Tipo_tarea.cocina;
            return Tipo_tarea.general;
        }

        private static bool ContieneAlguna(string texto, string[] palabras)
        {
            foreach (string palabra in palabras)
            {
                if (texto.Contains(palabra, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }
    }
}
