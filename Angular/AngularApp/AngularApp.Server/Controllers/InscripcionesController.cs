using AngularApp.Server.Data;
using AngularApp.Server.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AngularApp.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InscripcionesController : ControllerBase
{
    private readonly ServerDbContext _context;

    public InscripcionesController(ServerDbContext context)
    {
        _context = context;
    }

    public record InscripcionDto(
        int InscripcionId,
        int EventoId,
        string EventoNombre,
        int ParticipanteId,
        string ParticipanteNombre,
        string ParticipanteEmail,
        DateTime FechaInscripcion);

    public class CreateInscripcionRequest
    {
        public int EventoId { get; set; }
        public int ParticipanteId { get; set; }
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<InscripcionDto>>> GetInscripciones([FromQuery] int? eventoId, [FromQuery] int? participanteId)
    {
        var query = _context.Inscripciones
            .Include(i => i.Evento)
            .Include(i => i.Participante)
            .AsQueryable();

        if (eventoId.HasValue)
        {
            query = query.Where(i => i.EventoId == eventoId.Value);
        }

        if (participanteId.HasValue)
        {
            query = query.Where(i => i.ParticipanteId == participanteId.Value);
        }

        var result = await query
            .OrderByDescending(i => i.FechaInscripcion)
            .Select(i => new InscripcionDto(
                i.InscripcionId,
                i.EventoId,
                i.Evento!.Nombre,
                i.ParticipanteId,
                $"{i.Participante!.Nombre} {i.Participante.Apellido}",
                i.Participante.Email,
                i.FechaInscripcion))
            .ToListAsync();

        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<InscripcionDto>> GetInscripcion(int id)
    {
        var inscripcion = await _context.Inscripciones
            .Include(i => i.Evento)
            .Include(i => i.Participante)
            .Where(i => i.InscripcionId == id)
            .Select(i => new InscripcionDto(
                i.InscripcionId,
                i.EventoId,
                i.Evento!.Nombre,
                i.ParticipanteId,
                $"{i.Participante!.Nombre} {i.Participante.Apellido}",
                i.Participante.Email,
                i.FechaInscripcion))
            .FirstOrDefaultAsync();

        if (inscripcion is null)
        {
            return NotFound();
        }

        return Ok(inscripcion);
    }

    [HttpPost]
    public async Task<ActionResult<InscripcionDto>> CreateInscripcion(CreateInscripcionRequest request)
    {
        var evento = await _context.Eventos.FindAsync(request.EventoId);
        if (evento is null)
        {
            return NotFound($"No se encontró el evento con id {request.EventoId}");
        }

        var participante = await _context.Participantes.FindAsync(request.ParticipanteId);
        if (participante is null)
        {
            return NotFound($"No se encontró el participante con id {request.ParticipanteId}");
        }

        var alreadyRegistered = await _context.Inscripciones
            .AnyAsync(i => i.EventoId == request.EventoId && i.ParticipanteId == request.ParticipanteId);

        if (alreadyRegistered)
        {
            return Conflict("El participante ya está inscrito en este evento.");
        }

        var inscripcion = new Inscripcion
        {
            EventoId = request.EventoId,
            ParticipanteId = request.ParticipanteId,
            FechaInscripcion = DateTime.UtcNow
        };

        _context.Inscripciones.Add(inscripcion);
        await _context.SaveChangesAsync();

        var dto = new InscripcionDto(
            inscripcion.InscripcionId,
            evento.EventoId,
            evento.Nombre,
            participante.ParticipanteId,
            $"{participante.Nombre} {participante.Apellido}",
            participante.Email,
            inscripcion.FechaInscripcion);

        return CreatedAtAction(nameof(GetInscripcion), new { id = inscripcion.InscripcionId }, dto);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteInscripcion(int id)
    {
        var inscripcion = await _context.Inscripciones.FindAsync(id);
        if (inscripcion is null)
        {
            return NotFound();
        }

        _context.Inscripciones.Remove(inscripcion);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
