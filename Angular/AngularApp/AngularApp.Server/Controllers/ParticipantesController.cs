using AngularApp.Server.Data;
using AngularApp.Server.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AngularApp.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ParticipantesController : ControllerBase
{
    private readonly ServerDbContext _context;

    public ParticipantesController(ServerDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Participante>>> GetParticipantes()
    {
        var participantes = await _context.Participantes
            .OrderBy(p => p.Apellido)
            .ThenBy(p => p.Nombre)
            .AsNoTracking()
            .ToListAsync();

        return Ok(participantes);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Participante>> GetParticipante(int id)
    {
        var participante = await _context.Participantes.FindAsync(id);

        if (participante is null)
        {
            return NotFound();
        }

        return Ok(participante);
    }

    [HttpPost]
    public async Task<ActionResult<Participante>> CreateParticipante(Participante participante)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        _context.Participantes.Add(participante);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetParticipante), new { id = participante.ParticipanteId }, participante);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateParticipante(int id, Participante updatedParticipante)
    {
        if (id != updatedParticipante.ParticipanteId)
        {
            return BadRequest();
        }

        var existingParticipante = await _context.Participantes.FindAsync(id);
        if (existingParticipante is null)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        existingParticipante.Nombre = updatedParticipante.Nombre;
        existingParticipante.Apellido = updatedParticipante.Apellido;
        existingParticipante.Email = updatedParticipante.Email;
        existingParticipante.Telefono = updatedParticipante.Telefono;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteParticipante(int id)
    {
        var participante = await _context.Participantes.FindAsync(id);
        if (participante is null)
        {
            return NotFound();
        }

        _context.Participantes.Remove(participante);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
