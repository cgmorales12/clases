using System.ComponentModel.DataAnnotations;

namespace AngularApp.Server.Model;

public class Participante
{
    public int ParticipanteId { get; set; }

    [Required]
    [MaxLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Apellido { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [MaxLength(30)]
    public string? Telefono { get; set; }

    public ICollection<Inscripcion> Inscripciones { get; set; } = new List<Inscripcion>();
}
