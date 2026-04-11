using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEKTY_ON.Models
{
    public class Laboratorio
    {
        public int Id { get; set; }

        public string Nombre { get; set; }

        public int Edificio { get; set; }

        public int EstadoId { get; set; }

        public bool Abierto { get; set; }

        public int Numero { get; set; }

        public virtual Estado Estado { get; set; }
    }
}
