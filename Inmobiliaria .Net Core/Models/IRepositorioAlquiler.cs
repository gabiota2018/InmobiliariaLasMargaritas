using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inmobiliaria_.Net_Core.Models
{
  
     public interface IRepositorioAlquiler : IRepositorio<Alquiler>
    {
        Alquiler ObtenerPorIdInquilino(int id);

        Alquiler ObtenerPorIdInmueble(int id);
    }
}
