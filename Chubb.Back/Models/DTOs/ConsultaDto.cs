using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public class ConsultaPorCedulaDto
    {
        public AseguradoDto Asegurado { get; set; } = new();
        public List<SeguroAsignadoDto> Seguros { get; set; } = new();
    }

    public class ConsultaPorCodigoSeguroDto
    {
        public SeguroDto Seguro { get; set; } = new();
        public List<AseguradoAsignadoDto> Asegurados { get; set; } = new();
    }

    public class SeguroAsignadoDto
    {
        public int SeguroId { get; set; }
        public string CodigoSeguro { get; set; } = string.Empty;
        public string NombreSeguro { get; set; } = string.Empty;
        public decimal SumaAsegurada { get; set; }
        public decimal Prima { get; set; }
        public DateTime FechaAsignacion { get; set; }
    }

    public class AseguradoAsignadoDto
    {
        public int AseguradoId { get; set; }
        public string Cedula { get; set; } = string.Empty;
        public string NombreCompleto { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public int Edad { get; set; }
        public DateTime FechaAsignacion { get; set; }
    }
}
