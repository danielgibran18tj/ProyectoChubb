using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities
{
    public class CargaMasiva
    {
        public int CargaMasivaId { get; set; }
        public string NombreArchivo { get; set; } = string.Empty;
        public int TotalRegistros { get; set; }
        public int RegistrosExitosos { get; set; }
        public int RegistrosFallidos { get; set; }
        public DateTime FechaCarga { get; set; }
        public string? UsuarioCarga { get; set; }
        public string? Observaciones { get; set; }
    }
}
