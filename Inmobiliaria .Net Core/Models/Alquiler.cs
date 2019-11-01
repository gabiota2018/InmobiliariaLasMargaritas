using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Inmobiliaria_.Net_Core.Models
{
    public class Alquiler
    {
        [Key]
        [Display(Name = "Código")]
        public int IdAlquiler { get; set; }

        [Display(Name = "Cuota Mensual")]
        public decimal Precio { get; set; }

        [Display(Name = "Desde el")]
        public String Fecha_inicio { get; set; }

        [Display(Name = "hasta el")]
        public String Fecha_fin { get; set; }

        public int IdInquilino { get; set; }

        [Display(Name = "Inquilino")]
        public Inquilino inquilino { get; set; }

        public int IdInmueble { get; set; }

        [Display(Name = "Inmueble")]
        public Inmueble inmueble { get; set; }

        public int Borrado { get; set; }
    }
}
