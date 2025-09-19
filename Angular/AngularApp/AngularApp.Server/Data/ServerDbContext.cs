using AngularApp.Server.Model;
using Microsoft.EntityFrameworkCore;

namespace AngularApp.Server.Data
{
    public class ServerDbContext : DbContext
    {
        public ServerDbContext(DbContextOptions<ServerDbContext> options) : base(options)
        {
        }

        public DbSet<Evento> Eventos => Set<Evento>();
        public DbSet<Participante> Participantes => Set<Participante>();
        public DbSet<Inscripcion> Inscripciones => Set<Inscripcion>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Evento>(entity =>
            {
                entity.HasKey(e => e.EventoId);
                entity.Property(e => e.Nombre).IsRequired().HasMaxLength(150);
                entity.Property(e => e.Ubicacion).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Descripcion).HasMaxLength(500);
            });

            modelBuilder.Entity<Participante>(entity =>
            {
                entity.HasKey(p => p.ParticipanteId);
                entity.Property(p => p.Nombre).IsRequired().HasMaxLength(100);
                entity.Property(p => p.Apellido).IsRequired().HasMaxLength(100);
                entity.Property(p => p.Email).IsRequired();
                entity.HasIndex(p => p.Email).IsUnique();
            });

            modelBuilder.Entity<Inscripcion>(entity =>
            {
                entity.HasKey(i => i.InscripcionId);
                entity.HasIndex(i => new { i.EventoId, i.ParticipanteId }).IsUnique();

                entity.HasOne(i => i.Evento)
                    .WithMany(e => e.Inscripciones)
                    .HasForeignKey(i => i.EventoId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(i => i.Participante)
                    .WithMany(p => p.Inscripciones)
                    .HasForeignKey(i => i.ParticipanteId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
