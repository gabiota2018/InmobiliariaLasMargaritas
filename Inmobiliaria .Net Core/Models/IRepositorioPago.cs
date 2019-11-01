using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inmobiliaria_.Net_Core.Models
{
    public interface IRepositorioPago :IRepositorio<Pago>
    {
        IList<Pago> ObtenerPorIdAlquiler(int id);
        int ObtenerUltimoNro(int id);
    }
}
