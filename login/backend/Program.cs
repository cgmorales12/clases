using backend.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var conexion = builder.Configuration.GetConnectionString("cn");
builder.Services.AddDbContext<LoginDbContext>(
    op=> op.UseMySql(conexion, ServerVersion.Parse("5.7.24"))
    );
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(op => {
    op.AddPolicy("login", credenciales =>
    {
        credenciales.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});
var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("login");      /////////////////////////
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
