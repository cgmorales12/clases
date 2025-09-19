using System.ComponentModel.DataAnnotations;

namespace AngularApp.Server.Model;

public class Inscripcion
{
    public int InscripcionId { get; set; }

    [Required]
    public int EventoId { get; set; }

    public Evento? Evento { get; set; }

    [Required]
    public int ParticipanteId { get; set; }

    public Participante? Participante { get; set; }

    public DateTime FechaInscripcion { get; set; } = DateTime.UtcNow;
}
