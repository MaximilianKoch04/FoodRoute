using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Proyecto.Models.Domain;
using Microsoft.AspNetCore.Authorization; // Para usar [Authorize]

namespace Proyecto.Controllers
{   [Authorize]
    public class ProductoController : Controller 
    {
        private readonly DBContext _context;
        public ProductoController (DBContext context)
        {
            _context = context;
        }


        // 1. LISTADO DE PRODUCTOS
        public IActionResult Index()
        {
            var producto = _context.Productos.ToList();
            return View(producto);
        }

        // 2. VISTA PARA CREAR (GET)
        // Solo muestra el formulario vacío
        public IActionResult Crear()
        {
            return View();
        }

        // 3. PROCESAR LA CREACIÓN (POST)
        // Recibe los datos que el usuario escribió en el formulario
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Crear(Producto producto)
        {
            if (ModelState.IsValid)
            {
                _context.Productos.Add(producto);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(producto);
        }

        // ==========================================
        // 1. EDITAR (Muestra el formulario con los datos cargados)
        // ==========================================
        [HttpGet]
        public IActionResult Editar(int id)
        {
            // Buscamos el producto en SQL Server por su ID
            var producto = _context.Productos.Find(id);
            
            if (producto == null)
            {
                return NotFound(); // Si alguien pone un ID que no existe, tira error 404
            }

            return View(producto); // Le mandamos la cebolla encontrada al HTML
        }

        // ==========================================
        // 2. EDITAR (Guarda los cambios nuevos)
        // ==========================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Editar(Producto producto)
        {
            ModelState.Remove("Lotes"); // Limpiamos validaciones extra

            if (ModelState.IsValid)
            {
                _context.Productos.Update(producto);
                _context.SaveChanges(); // ¡Impacta el cambio en la BD!
                return RedirectToAction(nameof(Index)); // Vuelve al catálogo
            }
            return View(producto);
        }

        // ==========================================
        // 3. ELIMINAR (Borra el producto directo)
        // ==========================================
        // ==========================================
        // 3. ELIMINAR (Con red de seguridad SQL)
        // ==========================================
        [HttpPost]
        public IActionResult Eliminar(int id)
        {
            var producto = _context.Productos.Find(id);
            if (producto != null)
            {
                try
                {
                    _context.Productos.Remove(producto);
                    _context.SaveChanges(); 
                }
                catch (Exception) // <-- Esta es la red gigante que atrapa la explosión de la foto
                {
                    // Le mandamos el mensaje a la vista
                    TempData["Error"] = "No podés eliminar este producto porque actualmente tiene Lotes (stock) en el depósito.";
                    return RedirectToAction(nameof(Index));
                }
            }
            return RedirectToAction(nameof(Index));
        }
    }
}