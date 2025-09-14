using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tipo_Datos.Data;
using Tipo_Datos.Models.Entidades;

namespace Tipo_Datos.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class VentasAPIController : ControllerBase
    {
        private readonly DatosDbContext _context;

        public VentasAPIController(DatosDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<VentasModel>> PostVentasModel(VentasModel ventasModel)
        {
            if (ventasModel == null || ventasModel.Productos_Vendidos == null
                || ventasModel.Productos_Vendidos.Count == 0)
            {
                return BadRequest("No existen prodcutos cargados");
            }
            if (ventasModel.ClientesModelId <= 0) {
                return BadRequest("No existe el cliente");
            }
            await using var tx = await _context.Database.BeginTransactionAsync();
            try
            {
                var ultimoid = await _context.Ventas.AnyAsync()
                    ? await _context.Ventas.MaxAsync(v => v.Id) : 0; 

                ventasModel.FechaVenta = (ventasModel.FechaVenta == default)
                    ? DateTime.Now : ventasModel.FechaVenta;
                ventasModel.Codigo_Venta = (ultimoid + 1).ToString("D6"); //000001 000002

                double subtotal = 0;
                var itemValidos = new List<ProductosVendidosModel>();
                foreach (var item in ventasModel.Productos_Vendidos)
                {
                    if (item.ProductosModelId <= 0 || item.Cantidad < 0)
                    {
                        return BadRequest($"El producto {item.ProductosModelId} no existe");
                    }
                    var producto = await _context.Productos
                        .FirstOrDefaultAsync(p => p.Id == item.ProductosModelId);
                    if (producto == null) {
                        return BadRequest($"El producto {item.ProductosModelId} no existe");
                    }

                    if (producto.Stock < item.Cantidad) {
                        return BadRequest($"No existe la cantidad desea. Disponible: {producto.Stock}");
                    }

                    var precioUnit = producto.Precio;
                    var monto = Math.Round(precioUnit * item.Cantidad, 2);
                    producto.Stock -= item.Cantidad;
                    itemValidos.Add(new ProductosVendidosModel {
                        ProductosModelId = producto.Id,
                        Nombre = producto.Nombre,
                        Precio = precioUnit,
                        Cantidad = item.Cantidad,
                        Monto = monto
                    });
                    subtotal += monto;
                }
                ventasModel.Productos_Vendidos = itemValidos;
                ventasModel.Sub_Total_Venta = Math.Round(subtotal, 2);
                ventasModel.Total_Venta = Math.Round(ventasModel.Sub_Total_Venta 
                    - (ventasModel.Descuento ?? 0), 2);

                _context.Ventas.Add(ventasModel);
                await _context.SaveChangesAsync();
                await tx.CommitAsync();

                return CreatedAtAction(nameof(GetVentasModel), 
                    new { id = ventasModel.Id }, ventasModel);
            }
            catch (Exception ex)
            {
                await tx.RollbackAsync();
                return StatusCode(500, "Error al registrar la venta: " + ex.Message);
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<VentasModel>> GetVentasModel(int id) {
            var venta = await _context.Ventas
                .Include(v => v.Productos_Vendidos)
                .Include(v => v.ClientesModel)
                .FirstOrDefaultAsync(v=> v.Id == id);

            if (venta == null) return NotFound();
            return Ok(venta);
        }
       
    }
}
