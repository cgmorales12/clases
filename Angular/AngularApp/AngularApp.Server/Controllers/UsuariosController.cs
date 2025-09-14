using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AngularApp.Server.Data;
using AngularApp.Server.Model;

namespace AngularApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly ServerDbContext _context;

        public UsuariosController(ServerDbContext context)
        {
            _context = context;
        }

        // GET: api/Usuarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuariosModel>>> GetUsuariosModel()
        {
            return await _context.UsuariosModel.ToListAsync();
        }

        // GET: api/Usuarios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UsuariosModel>> GetUsuariosModel(int id)
        {
            var usuariosModel = await _context.UsuariosModel.FindAsync(id);

            if (usuariosModel == null)
            {
                return NotFound();
            }

            return usuariosModel;
        }

        // PUT: api/Usuarios/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuariosModel(int id, UsuariosModel usuariosModel)
        {
            if (id != usuariosModel.Id)
            {
                return BadRequest();
            }

            _context.Entry(usuariosModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuariosModelExists(id))
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

        // POST: api/Usuarios
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UsuariosModel>> PostUsuariosModel(UsuariosModel usuariosModel)
        {
            _context.UsuariosModel.Add(usuariosModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUsuariosModel", new { id = usuariosModel.Id }, usuariosModel);
        }

        [HttpPost(Name ="login")]
        public async Task<ActionResult<UsuariosModel>> login(string email, string pwd)
        {
            
            var uid = _context.UsuariosModel.Where(u => u.correo == email && u.pwd == pwd).FirstOrDefault();
            

            return Ok(uid);
        }

        // DELETE: api/Usuarios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuariosModel(int id)
        {
            var usuariosModel = await _context.UsuariosModel.FindAsync(id);
            if (usuariosModel == null)
            {
                return NotFound();
            }

            _context.UsuariosModel.Remove(usuariosModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UsuariosModelExists(int id)
        {
            return _context.UsuariosModel.Any(e => e.Id == id);
        }
    }
}
