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
    public class ConfiguracionesController : Controller
    {
        private readonly DatosDbContext _context;

        public ConfiguracionesController(DatosDbContext context)
        {
            _context = context;
        }

        // GET: Configuraciones
        public async Task<IActionResult> Index()
        {
            return View(await _context.ConfiguracionesEmpresa.ToListAsync());
        }

        // GET: Configuraciones/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var configuracionesModel = await _context.ConfiguracionesEmpresa
                .FirstOrDefaultAsync(m => m.Id == id);
            if (configuracionesModel == null)
            {
                return NotFound();
            }

            return View(configuracionesModel);
        }

        // GET: Configuraciones/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Configuraciones/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Logo,Nombre_Empresa,Email,Web,Telefono,Direccion,RUC,Contrasenia,Id,Create_At,Update_At,isDelete")] ConfiguracionesModel configuracionesModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(configuracionesModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(configuracionesModel);
        }

        // GET: Configuraciones/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var configuracionesModel = await _context.ConfiguracionesEmpresa.FindAsync(id);
            if (configuracionesModel == null)
            {
                return NotFound();
            }
            return View(configuracionesModel);
        }

        // POST: Configuraciones/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Logo,Nombre_Empresa,Email,Web,Telefono,Direccion,RUC,Contrasenia,Id,Create_At,Update_At,isDelete")] ConfiguracionesModel configuracionesModel)
        {
            if (id != configuracionesModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(configuracionesModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ConfiguracionesModelExists(configuracionesModel.Id))
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
            return View(configuracionesModel);
        }

        // GET: Configuraciones/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var configuracionesModel = await _context.ConfiguracionesEmpresa
                .FirstOrDefaultAsync(m => m.Id == id);
            if (configuracionesModel == null)
            {
                return NotFound();
            }

            return View(configuracionesModel);
        }

        // POST: Configuraciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var configuracionesModel = await _context.ConfiguracionesEmpresa.FindAsync(id);
            if (configuracionesModel != null)
            {
                _context.ConfiguracionesEmpresa.Remove(configuracionesModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ConfiguracionesModelExists(int id)
        {
            return _context.ConfiguracionesEmpresa.Any(e => e.Id == id);
        }
    }
}
