using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inmobiliaria_.Net_Core.Models
{
  
     public interface IRepositorioAlquiler : IRepositorio<Alquiler>
    {
        IList<Alquiler> ObtenerPorIdInquilino(int id);

        IList<Alquiler> ObtenerPorIdInmueble(int id);

        IList<Alquiler> ObtenerVigentes();
       
    }
}
