using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEKTY_ON.Models
{
    public class Peticion
    {
        public int Id { get; set; }

        public int UsuarioId { get; set; }

        public int LaboratorioId { get; set; }

        public DateTime FechaInicio { get; set; }

        public DateTime FechaTermino { get; set; }

        public bool? Estatus { get; set; }

        public DateTime FechaCreacion { get; set; }

        public string Notas { get; set; }

        public virtual Laboratorio Laboratorio { get; set; }

        public virtual Usuario Usuario { get; set; }
    }
}
