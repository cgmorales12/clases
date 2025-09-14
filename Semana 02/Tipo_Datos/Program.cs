using Microsoft.EntityFrameworkCore;
using Tipo_Datos.Data;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);
var cn = builder.Configuration.GetConnectionString("cn") 
    ?? throw new InvalidOperationException("No existe la referencia a la conexion");

builder.Services.AddDbContext<DatosDbContext>(opciones => opciones.UseSqlServer(cn));

//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<Tipo_DatosContext>();

builder.Services.AddIdentity<IdentityUser, IdentityRole>(
    op => {
        op.SignIn.RequireConfirmedAccount = false;
        op.Password.RequiredLength = 3;
        op.Password.RequireNonAlphanumeric = true;
        op.Password.RequireUppercase = true;    
        op.Password.RequireLowercase = true;
        //LuisLlerena_2025
    }
    ).AddEntityFrameworkStores<Tipo_Datos.Data.DatosDbContext>()
    .AddDefaultTokenProviders();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
