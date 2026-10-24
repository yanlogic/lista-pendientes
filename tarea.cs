using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace listas_pendientes
{
    public class Tarea
    {
        private string nombre = "";
        private bool completada;
        private string tema = "";

        public string Nombre { get => nombre; set => nombre = value; }
        public bool Completada { get => completada; set => completada = value; }
        public string Tema { get => tema; set => tema = value; }

        public override string ToString()
        {
            return nombre;
        }
    }
}
