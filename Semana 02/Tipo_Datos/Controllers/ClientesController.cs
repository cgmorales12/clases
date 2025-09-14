using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tipo_Datos.Data;
using Tipo_Datos.Models.Entidades;

namespace Tipo_Datos.Controllers
{
    public class ClientesController : Controller
    {
        private readonly DatosDbContext _dbContext;
        public ClientesController(DatosDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IActionResult> Index()
        {
            var listaClientes = await _dbContext.Clientes.ToListAsync();
            listaClientes.Insert(0, new ClientesModel {
                Id= 0,
                Nombres = "Seleccione un cliente",
                Cedula_RUC  = string.Empty,
                Create_At = DateTime.Now,
                Direccion = string.Empty,
                Email = string.Empty,
                isDelete = false,
                Telefono = string.Empty,
                Update_At = DateTime.Now,
            });
            return View(listaClientes);
        }

        public IActionResult Nuevo() {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> 
            Nuevo([Bind("Nombres,Email,Telefono,Direccion,Cedula_RUC,isDelete")] ClientesModel cliente)
        {
            if (ModelState.IsValid)
            {
                cliente.Create_At = DateTime.Now;
                _dbContext.Add(cliente);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return  View(cliente);
        }
        public async Task<IActionResult> Editar(int? Id)
        {
            if (Id == null) return NotFound();

            var cliente = await _dbContext.Clientes.FindAsync(Id);
            if (cliente == null) return NotFound();

            return View(cliente);
        }

        [HttpPost]
        
        public async Task<IActionResult> Editar(int id, [Bind("Id,Nombres,Email,Telefono,Direccion,Cedula_RUC,isDelete")] ClientesModel cliente) {
            if (id != cliente.Id) return NotFound();

            if (ModelState.IsValid) {
                try
                {
                    cliente.Update_At = DateTime.Now;
                    _dbContext.Update(cliente);
                    await _dbContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClienteExiste(cliente.Id))
                    {
                        return NotFound();
                    }
                    else {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(cliente);
    
        }


        public async Task<IActionResult> Eliminar(int? Id)
        {
            if (Id == null) return NotFound();

            var cliente = await _dbContext.Clientes.FindAsync(Id);
            if (cliente == null) return NotFound();

            return View(cliente);
        }

        [HttpDelete, ActionName("Eliminar")]
        public async Task<IActionResult> ConfirmacionEliminar(int Id) {
            var cliente = await _dbContext.Clientes.FindAsync(Id);
            if (cliente != null) {
                _dbContext.Clientes.Remove(cliente);
                await _dbContext.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }


        public bool ClienteExiste(int id) { 
            return _dbContext.Clientes.Any(c => c.Id == id);
        }

    }
}


