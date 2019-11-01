using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Inmobiliaria_.Net_Core.Models
{
    public class Inquilino
    {
        [Key]
        [Display(Name = "Código")]
        public int IdInquilino { get; set; }
        [Required]
        public int Dni { get; set; }
        [Required]
        public String Apellido { get; set; }
        [Required]
        public String Nombre { get; set; }
        [Required]
        public String Direccion { get; set; }
        [Required]
        public int Telefono { get; set; }
        public int Borrado { get; set; }
    }
}
