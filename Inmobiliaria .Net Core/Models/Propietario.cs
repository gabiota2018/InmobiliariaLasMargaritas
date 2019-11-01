using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Inmobiliaria_.Net_Core.Models
{
    public class Propietario
    {
        [Key]
        [Display(Name = "Código")]
        public int IdPropietario { get; set; }
       
        [Required]
        public int? Dni { get; set; }
        [Required]
        public String Nombre { get; set; }
        [Required]
        public String Apellido { get; set; }
        [Required]
        public int? Telefono { get; set; }
        [Required, DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public String Mail { get; set; }
        [Required, DataType(DataType.Password)]
        public String Password { get; set; }
        public int Borrado { get; set; }
    }
}
