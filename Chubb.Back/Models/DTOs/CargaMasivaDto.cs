using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public class CargaMasivaDto
    {
        public string NombreArchivo { get; set; } = string.Empty;
        public List<AseguradoCargaDto> Asegurados { get; set; } = new();
    }

    public class AseguradoCargaDto
    {
        public string Cedula { get; set; } = string.Empty;
        public string NombreCompleto { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public int Edad { get; set; }
        public int NumeroLinea { get; set; } // Para rastrear errores
    }

    public class ResultadoCargaMasivaDto
    {
        public int TotalRegistros { get; set; }
        public int RegistrosExitosos { get; set; }
        public int RegistrosFallidos { get; set; }
        public List<ErrorCargaDto> Errores { get; set; } = new();
        public string Mensaje { get; set; } = string.Empty;
    }

    public class ErrorCargaDto
    {
        public int NumeroLinea { get; set; }
        public string Cedula { get; set; } = string.Empty;
        public string MensajeError { get; set; } = string.Empty;
    }
}
