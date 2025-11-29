using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public class SeguroDto
    {
        public int SeguroId { get; set; }
        public string CodigoSeguro { get; set; } = string.Empty;
        public string NombreSeguro { get; set; } = string.Empty;
        public decimal SumaAsegurada { get; set; }
        public decimal Prima { get; set; }
    }


    public class CrearSeguroDto
    {
        public string CodigoSeguro { get; set; } = string.Empty;
        public string NombreSeguro { get; set; } = string.Empty;
        public decimal SumaAsegurada { get; set; }
        public decimal Prima { get; set; }
    }

    public class ActualizarSeguroDto
    {
        public int SeguroId { get; set; }
        public string CodigoSeguro { get; set; } = string.Empty;
        public string NombreSeguro { get; set; } = string.Empty;
        public decimal SumaAsegurada { get; set; }
        public decimal Prima { get; set; }
    }
}
