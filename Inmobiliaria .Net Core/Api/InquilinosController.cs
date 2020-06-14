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
    [ApiController]
    public class InquilinosController : Controller
    {
        private readonly DataContext contexto;

        public InquilinosController(DataContext contexto)
        {
            this.contexto = contexto;
        }

        // GET: api/<controller>
        [HttpGet]
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
