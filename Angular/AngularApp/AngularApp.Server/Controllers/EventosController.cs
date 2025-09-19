using AngularApp.Server.Data;
using AngularApp.Server.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AngularApp.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventosController : ControllerBase
{
    private readonly ServerDbContext _context;

    public EventosController(ServerDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Evento>>> GetEventos()
    {
        var eventos = await _context.Eventos
            .OrderBy(e => e.Fecha)
            .AsNoTracking()
            .ToListAsync();

        return Ok(eventos);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Evento>> GetEvento(int id)
    {
        var evento = await _context.Eventos.FindAsync(id);

        if (evento is null)
        {
            return NotFound();
        }

        return Ok(evento);
    }

    [HttpPost]
    public async Task<ActionResult<Evento>> CreateEvento(Evento evento)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        _context.Eventos.Add(evento);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetEvento), new { id = evento.EventoId }, evento);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateEvento(int id, Evento updatedEvento)
    {
        if (id != updatedEvento.EventoId)
        {
            return BadRequest();
        }

        var existingEvento = await _context.Eventos.FindAsync(id);
        if (existingEvento is null)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        existingEvento.Nombre = updatedEvento.Nombre;
        existingEvento.Fecha = updatedEvento.Fecha;
        existingEvento.Ubicacion = updatedEvento.Ubicacion;
        existingEvento.Descripcion = updatedEvento.Descripcion;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteEvento(int id)
    {
        var evento = await _context.Eventos.FindAsync(id);
        if (evento is null)
        {
            return NotFound();
        }

        _context.Eventos.Remove(evento);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
