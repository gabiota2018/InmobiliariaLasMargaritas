using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Inmobiliaria_.Net_Core.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace InmobiliariaLasMargaritas.Api
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PagosController : Controller
    {
        private readonly DataContext contexto;
        public PagosController(DataContext contexto)
        {
            this.contexto = contexto;
        }
        public async Task<IActionResult> Get()
        {
            try
            {
                var usuario = User.Identity.Name;
                var lista = contexto.Inmuebles.Include(e => e.propietario).ToList();
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var usuario = User.Identity.Name;
                return Ok(contexto.Inmuebles.Include(e => e.propietario).Where(e => e.propietario.Mail == usuario).Single(e => e.IdInmueble == id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
