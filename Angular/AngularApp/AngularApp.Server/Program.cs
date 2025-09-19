using AngularApp.Server.Data;
using AngularApp.Server.Model;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ServerDbContext>(options =>
    options.UseInMemoryDatabase("EventosDb"));

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("MyPolicy",
        corsBuilder =>
        {
            corsBuilder.WithOrigins("http://localhost:4200")
                       .AllowAnyHeader()
                       .AllowAnyMethod();
        });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ServerDbContext>();

    if (!context.Eventos.Any())
    {
        var runningClub = new Evento
        {
            Nombre = "Carrera 10K Ciudad",
            Fecha = DateTime.UtcNow.AddDays(15),
            Ubicacion = "Parque Central",
            Descripcion = "Carrera urbana para corredores intermedios"
        };

        var swimmingTournament = new Evento
        {
            Nombre = "Torneo Regional de Natación",
            Fecha = DateTime.UtcNow.AddDays(30),
            Ubicacion = "Complejo Acuático Municipal",
            Descripcion = "Competencia para nadadores juveniles"
        };

        var participantLaura = new Participante
        {
            Nombre = "Laura",
            Apellido = "Gómez",
            Email = "laura.gomez@example.com",
            Telefono = "+34 600 111 222"
        };

        var participantDiego = new Participante
        {
            Nombre = "Diego",
            Apellido = "Martínez",
            Email = "diego.martinez@example.com",
            Telefono = "+34 600 333 444"
        };

        context.Eventos.AddRange(runningClub, swimmingTournament);
        context.Participantes.AddRange(participantLaura, participantDiego);
        context.SaveChanges();

        context.Inscripciones.Add(new Inscripcion
        {
            EventoId = runningClub.EventoId,
            ParticipanteId = participantLaura.ParticipanteId,
            FechaInscripcion = DateTime.UtcNow.AddDays(-2)
        });

        context.Inscripciones.Add(new Inscripcion
        {
            EventoId = swimmingTournament.EventoId,
            ParticipanteId = participantDiego.ParticipanteId,
            FechaInscripcion = DateTime.UtcNow.AddDays(-1)
        });

        context.SaveChanges();
    }
}

app.UseDefaultFiles();
app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("MyPolicy");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapFallbackToFile("/index.html");

app.Run();
