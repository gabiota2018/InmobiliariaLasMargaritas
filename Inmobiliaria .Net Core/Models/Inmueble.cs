using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Inmobiliaria_.Net_Core.Models
{
    public class Inmueble
    {
        [Key]
        [Display(Name = "Código")]
        public int InmuebleId { get; set; }
        [Required]
        public String Direccion { get; set; }
        [Required]
        public int? Ambientes { get; set; }
        [Required]
        public String Tipo { get; set; }
        [Required]
        public String Uso { get; set; }
        [Required]
        public decimal? Precio { get; set; }
        [Required]
        public int? Disponible { get; set; }
        public int Borrado { get; set; }
        [ForeignKey("PropietarioId")]
        public int? PropietarioId { get; set; }

       public Propietario Propietario { get; set; }
    }
}
