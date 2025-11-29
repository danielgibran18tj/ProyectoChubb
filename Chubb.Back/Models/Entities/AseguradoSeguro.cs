using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities
{
    public class AseguradoSeguro
    {
        public int AseguradoSeguroId { get; set; }
        public int AseguradoId { get; set; }
        public int SeguroId { get; set; }
        public DateTime FechaAsignacion { get; set; }
        public bool Activo { get; set; }


        public Asegurado? Asegurado { get; set; }
        public Seguro? Seguro { get; set; }
    }
}
