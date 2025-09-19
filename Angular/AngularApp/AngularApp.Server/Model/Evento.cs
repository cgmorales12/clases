using System.ComponentModel.DataAnnotations;

namespace AngularApp.Server.Model;

public class Evento
{
    public int EventoId { get; set; }

    [Required]
    [MaxLength(150)]
    public string Nombre { get; set; } = string.Empty;

    [Required]
    public DateTime Fecha { get; set; }

    [Required]
    [MaxLength(200)]
    public string Ubicacion { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Descripcion { get; set; }

    public ICollection<Inscripcion> Inscripciones { get; set; } = new List<Inscripcion>();
}
