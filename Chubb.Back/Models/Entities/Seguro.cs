using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities
{
    public class Seguro
    {
        public int SeguroId { get; set; }
        public string CodigoSeguro { get; set; } = string.Empty;
        public string NombreSeguro { get; set; } = string.Empty;
        public decimal SumaAsegurada { get; set; }
        public decimal Prima { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public bool Activo { get; set; }
    }
}
