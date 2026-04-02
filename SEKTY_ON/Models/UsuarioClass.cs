using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEKTY_ON.Models
{
    public partial class Usuario
    {
        public int Id { get; set; }

        public string Nombre { get; set; }

        public byte[] Contraseña { get; set; }

        public bool Abilitado { get; set; }

        public int PuestoId { get; set; }

        public virtual Puesto Puesto { get; set; }
    }
}
