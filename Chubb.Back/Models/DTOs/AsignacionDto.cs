using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public class AsignacionDto
    {
        public int AseguradoId { get; set; }
        public int SeguroId { get; set; }
    }


    public class AsignacionDetalleDto
    {
        public int AseguradoSeguroId { get; set; }
        public int AseguradoId { get; set; }
        public string CedulaAsegurado { get; set; } = string.Empty;
        public string NombreAsegurado { get; set; } = string.Empty;
        public int SeguroId { get; set; }
        public string CodigoSeguro { get; set; } = string.Empty;
        public string NombreSeguro { get; set; } = string.Empty;
        public DateTime FechaAsignacion { get; set; }
    }
}
