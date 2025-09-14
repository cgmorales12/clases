using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tipo_Datos.Data;
using Tipo_Datos.Models.Entidades;

namespace Tipo_Datos.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesApiController : ControllerBase
    {
        private readonly DatosDbContext _context;

        public ClientesApiController(DatosDbContext context)
        {
            _context = context;
        }

        // GET: api/ClientesApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClientesModel>>> GetClientes()
        {

            var listaClientes = await _context.Clientes.ToListAsync();

            listaClientes.Insert(0, new ClientesModel {
                Id= 0,
                Nombres = "Seleccione un cliente",
                Cedula_RUC  = string.Empty,
                Create_At = DateTime.Now,
                Direccion = string.Empty,
                Email = string.Empty,
                isDelete = false,
                Telefono = string.Empty,
                Update_At = DateTime.Now,
            });

            return listaClientes;
        }

        // GET: api/ClientesApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ClientesModel>> GetClientesModel(int id)
        {
            var clientesModel = await _context.Clientes.FindAsync(id);

            if (clientesModel == null)
            {
                return NotFound();
            }

            return clientesModel;
        }

        // PUT: api/ClientesApi/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClientesModel(int id, ClientesModel clientesModel)
        {
            if (id != clientesModel.Id)
            {
                return BadRequest();
            }

            _context.Entry(clientesModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClientesModelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ClientesApi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ClientesModel>> PostClientesModel(ClientesModel clientesModel)
        {
            _context.Clientes.Add(clientesModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetClientesModel", new { id = clientesModel.Id }, clientesModel);
        }

        // DELETE: api/ClientesApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClientesModel(int id)
        {
            var clientesModel = await _context.Clientes.FindAsync(id);
            if (clientesModel == null)
            {
                return NotFound();
            }

            _context.Clientes.Remove(clientesModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ClientesModelExists(int id)
        {
            return _context.Clientes.Any(e => e.Id == id);
        }
    }
}
