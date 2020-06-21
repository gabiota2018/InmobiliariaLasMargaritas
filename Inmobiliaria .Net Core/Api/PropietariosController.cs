using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Inmobiliaria_.Net_Core.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace InmobiliariaLasMargaritas.Api
{
   
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PropietariosController : ControllerBase
    {
        private readonly DataContext contexto;
        private readonly IConfiguration config;

        public PropietariosController(DataContext contexto, IConfiguration config)
        {
            this.contexto = contexto;
            this.config = config;
        }

        // GET: api/DuenioEvento
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                return Ok(contexto.Propietario.SingleOrDefault(x => x.Mail == User.Identity.Name));
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // GET: api/DuenioEvento/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                return Ok(contexto.Propietario.SingleOrDefault(x => x.PropietarioId == id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // POST: api/DuenioEvento
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Post(Propietario entidad)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    entidad.Clave = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                                                password: entidad.Clave,
                                                salt: System.Text.Encoding.ASCII.GetBytes("SALADA"),
                                                prf: KeyDerivationPrf.HMACSHA1,
                                                iterationCount: 1000,
                                                numBytesRequested: 256 / 8));
                    contexto.Propietario.Add(entidad);
                    contexto.SaveChanges();
                    return CreatedAtAction(nameof(Get), new { id = entidad.PropietarioId }, entidad);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // PUT: api/DuenioEvento/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Propietario entidad)
        {
            try
            {
                //edita solo el propie logeado
                if (ModelState.IsValid && contexto.Propietario.AsNoTracking().SingleOrDefault(e => e.PropietarioId == id && e.Mail == User.Identity.Name) != null)
                {
                    entidad.PropietarioId = id;
                    entidad.Clave = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                                                password: entidad.Clave,
                                                salt: System.Text.Encoding.ASCII.GetBytes("SALADA"),
                                                prf: KeyDerivationPrf.HMACSHA1,
                                                iterationCount: 1000,
                                                numBytesRequested: 256 / 8));
                    contexto.Propietario.Update(entidad);
                    contexto.SaveChanges();
                    return Ok(entidad);
                }
                //return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
            try
            {
                return Ok("anduvo");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                //Borra solo el DuenioEvento logeado
                var entidad = contexto.Propietario.FirstOrDefault(e => e.PropietarioId == id && e.Mail == User.Identity.Name);
                if (entidad != null)
                {
                    entidad.Borrado = 0;
                    contexto.Propietario.Update(entidad);
                    contexto.SaveChanges();
                    return Ok();
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(Propietario entidad)
        {
            try
            {
                string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: entidad.Clave,
                    salt: System.Text.Encoding.ASCII.GetBytes(config["Salt"]),
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 1000,
                    numBytesRequested: 256 / 8));
                var p = contexto.Propietario.FirstOrDefault(x => x.Mail == entidad.Mail);
                if (p == null || p.Clave != hashed)
                {
                    return BadRequest("Nombre de usuario o clave incorrecta");
                }
                else
                {
                    var key = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(config["TokenAuthentication:SecretKey"]));
                    var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, p.Mail),
                        new Claim("FullName", p.Nombre + " " + p.Apellido),
                        new Claim(ClaimTypes.Role, "Propietario"),
                    };

                    var token = new JwtSecurityToken(
                        issuer: config["TokenAuthentication:Issuer"],
                        audience: config["TokenAuthentication:Audience"],
                        claims: claims,
                        expires: DateTime.Now.AddMinutes(60),
                        signingCredentials: credenciales
                    );
                    return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        public async Task<IActionResult> Test()
        {
            try
            {
                return Ok("anduvo");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
