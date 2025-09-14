using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Tipo_Datos.Data;
using Tipo_Datos.Models.Entidades;

namespace Tipo_Datos.Controllers
{
    public class VentasController : Controller
    {
        private readonly DatosDbContext _context;

        public VentasController(DatosDbContext context)
        {
            _context = context;
        }

        // GET: Ventas
        public async Task<IActionResult> Index()
        {
            var datosDbContext = _context.Ventas.Include(v => v.ClientesModel);
            return View(await datosDbContext.ToListAsync());
        }

        // GET: Ventas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ventasModel = await _context.Ventas
                .Include(v => v.ClientesModel)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ventasModel == null)
            {
                return NotFound();
            }

            return View(ventasModel);
        }

        // GET: Ventas/Create
        public IActionResult Create()
        {
            ViewData["ClientesModelId"] = new SelectList(_context.Clientes, "Id", "Nombres");
            return View();
        }

        // POST: Ventas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FechaVenta,Codigo_Venta,Notas,Sub_Total_Venta,Estado_Venta,Descuento,Total_Venta,Metodo_Pago,ClientesModelId,Id,Create_At,Update_At,isDelete")] VentasModel ventasModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ventasModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClientesModelId"] = new SelectList(_context.Clientes, "Id", "Cedula_RUC", ventasModel.ClientesModelId);
            return View(ventasModel);
        }

        // GET: Ventas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ventasModel = await _context.Ventas.FindAsync(id);
            if (ventasModel == null)
            {
                return NotFound();
            }
            ViewData["ClientesModelId"] = new SelectList(_context.Clientes, "Id", "Cedula_RUC", ventasModel.ClientesModelId);
            return View(ventasModel);
        }

        // POST: Ventas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FechaVenta,Codigo_Venta,Notas,Sub_Total_Venta,Estado_Venta,Descuento,Total_Venta,Metodo_Pago,ClientesModelId,Id,Create_At,Update_At,isDelete")] VentasModel ventasModel)
        {
            if (id != ventasModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ventasModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VentasModelExists(ventasModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClientesModelId"] = new SelectList(_context.Clientes, "Id", "Cedula_RUC", ventasModel.ClientesModelId);
            return View(ventasModel);
        }

        // GET: Ventas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ventasModel = await _context.Ventas
                .Include(v => v.ClientesModel)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ventasModel == null)
            {
                return NotFound();
            }

            return View(ventasModel);
        }

        // POST: Ventas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ventasModel = await _context.Ventas.FindAsync(id);
            if (ventasModel != null)
            {
                _context.Ventas.Remove(ventasModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VentasModelExists(int id)
        {
            return _context.Ventas.Any(e => e.Id == id);
        }
    }
}
