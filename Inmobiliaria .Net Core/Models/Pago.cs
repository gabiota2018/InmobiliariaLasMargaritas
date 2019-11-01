using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Inmobiliaria_.Net_Core.Models
{
    public class Pago
    {
        [Display(Name = "Comprobante N°")]
        public int IdPago { get; set; }
        [Display(Name = "Pago N°")]
        public int NroPago { get; set; }
        [Display(Name = "Alquiler N°")]
        public int IdAlquiler { get; set; }
        public String Fecha { get; set; }
        public decimal Importe { get; set; }
        public int Borrado { get; set; }
    }
}
