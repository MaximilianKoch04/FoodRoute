using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Proyecto.Models.Domain;
using Proyecto.Models.Emuns;
using System.Linq;
using System;
using Microsoft.AspNetCore.Authorization; // Para usar [Authorize]
// Agregá acá el using de tu contexto de base de datos si hace falta (ej: using Proyecto.Data;)

namespace Proyecto.Controllers
{
    [Authorize]
    public class LoteController : Controller
    {
        private readonly DBContext _context; // Cambiá ApplicationDbContext por el nombre exacto de tu conexión si es distinto

        
        public LoteController(DBContext context)
        {
            _context = context;
        }

        // ==========================================
        // 1. CREAR LOTE (Muestra el formulario vacío)
        // ==========================================
        [HttpGet]
        public IActionResult Crear(int? productoId)
        {
            // 1. PRIMERO descargamos los productos de la base de datos a la memoria de forma segura
            var listaProductos = _context.Productos.OrderBy(p => p.Nombre).ToList();

            // 2. DESPUÉS armamos el menú desplegable
            ViewBag.Productos = new SelectList(listaProductos, "IdProducto", "Nombre", productoId);

            var nuevoLote = new Lote
            {
                FechaIngreso = DateTime.Now,
                IdProducto = productoId ?? 0 
            };

            return View(nuevoLote);
        }

        // ==========================================
        // 2. GUARDAR LOTE (Recibe los datos del formulario)
        // ==========================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Crear(Lote lote)
        {
            // Limpiamos la validación del objeto "Producto" porque desde el formulario 
            // solo nos llega el número (IdProducto), no el objeto entero.
            ModelState.Remove("Producto");

            if (ModelState.IsValid)
            {
                // Le decimos al sistema que este lote entra HOY
                lote.FechaIngreso = DateTime.Now; 
                
                // Por defecto, al crearlo, está "Disponible". 
                // (Tu patrón Strategy en el Dashboard se va a encargar de cambiarle el estado después si se vence)
                lote.Estado = EstadoLote.Disponible;

                _context.Lotes.Add(lote);
                _context.SaveChanges(); // ¡Se guarda el stock en SQL Server!

                // Una vez guardado, lo mandamos al Home (Tu Dashboard principal) 
                // para que vea su nuevo lote brillando en la tabla FEFO
                return RedirectToAction("Index", "Home"); 
            }

            // Si el usuario se olvidó de llenar algo, recargamos la lista desplegable para que no explote
            ViewBag.Productos = new SelectList(_context.Productos.OrderBy(p => p.Nombre), "IdProducto", "Nombre", lote.IdProducto);
            return View(lote);
        }
    }
}