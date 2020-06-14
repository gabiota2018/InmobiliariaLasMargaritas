using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InmobiliariaLasMargaritas.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestPropietarioController : ControllerBase
    {

        // GET: api/<controller>
        [HttpGet]
       public async Task<IActionResult> Get()
        {
            try
            {
                return Ok(new
                {
                    Mensaje = "Éxito",
                    Error = 0,
                    Resultado = new
                    {
                        Clave = "Key",
                        Valor = "Value"
                    },
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }




        // GET: api/TestPropietario
    /*  [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }*/

        // GET: api/TestPropietario/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value"+id;
        }

        // POST: api/TestPropietario
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/TestPropietario/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
