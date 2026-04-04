using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEKTY_ON.Models
{
    public class Responsable
    {
        public int Id { get; set; }

        public string Nombre { get; set; }

        public byte[] Contraseña { get; set; }

        public bool Activado { get; set; }

        public string Correo { get; set; }
    }
}
