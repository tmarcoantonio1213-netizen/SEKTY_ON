using Newtonsoft.Json;
using System;

namespace SEKTY_ON.Models
{
    public class Peticion
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("usuarioId")]
        public int UsuarioId { get; set; }

        [JsonProperty("laboratorioId")]
        public int LaboratorioId { get; set; }

        [JsonProperty("fechaInicio")]
        public DateTime FechaInicio { get; set; }

        [JsonProperty("fechaTermino")]
        public DateTime FechaTermino { get; set; }

        [JsonProperty("estatus")]
        public bool? Estatus { get; set; }
    }
}