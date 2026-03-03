using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proyecto.Models.Domain;
using Proyecto.Repositories.Abstract;

namespace Proyecto.Controllers
{
    [Authorize(Roles = "Admin")] // Podés sumar "Vendedor" después si querés
    public class ProveedorController : Controller
    {
        private readonly IProveedorService _proveedorService;

        public ProveedorController(IProveedorService proveedorService)
        {
            _proveedorService = proveedorService;
        }

        // GET: /Proveedor/Index
        public async Task<IActionResult> Index()
        {
            var proveedores = await _proveedorService.ObtenerTodosAsync();
            return View(proveedores);
        }

        // ==========================================
        // 1. CREAR (ALTA)
        // ==========================================
        
        // GET: Muestra el formulario vacío
        [HttpGet]
        public IActionResult Crear()
        {
            return View();
        }

        // POST: Recibe los datos y los guarda en la base
        [HttpPost]
        public async Task<IActionResult> Crear(Proveedor proveedor)
        {
            if (!ModelState.IsValid)
            {
                return View(proveedor); // Si falta algún dato, recarga el formulario
            }

            var exito = await _proveedorService.AgregarAsync(proveedor);
            if (exito)
            {
                TempData["Msg"] = "Proveedor agregado con éxito.";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Error = "Ocurrió un error al guardar el proveedor en la base de datos.";
            return View(proveedor);
        }

        // ==========================================
        // 2. EDITAR (MODIFICACIÓN)
        // ==========================================

        // GET: Busca al proveedor por ID y llena el formulario
        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var proveedor = await _proveedorService.ObtenerPorIdAsync(id);
            if (proveedor == null)
            {
                TempData["Msg"] = "Proveedor no encontrado.";
                return RedirectToAction(nameof(Index));
            }
            return View(proveedor);
        }

        // POST: Recibe los datos modificados y los actualiza
        [HttpPost]
        public async Task<IActionResult> Editar(Proveedor proveedor)
        {
            if (!ModelState.IsValid)
            {
                return View(proveedor);
            }

            var exito = await _proveedorService.ActualizarAsync(proveedor);
            if (exito)
            {
                TempData["Msg"] = "Datos del proveedor actualizados correctamente.";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Error = "Error al intentar actualizar el proveedor.";
            return View(proveedor);
        }

        // ==========================================
        // 3. ELIMINAR (BAJA)
        // ==========================================

        // POST: Recibe el ID desde el botón de la tabla y lo borra
        [HttpPost]
        public async Task<IActionResult> Eliminar(int id)
        {
            var exito = await _proveedorService.EliminarAsync(id);
            if (exito)
            {
                TempData["Msg"] = "Proveedor eliminado del sistema.";
            }
            else
            {
                TempData["Msg"] = "No se pudo eliminar el proveedor (puede que esté asociado a otros registros).";
            }
            
            return RedirectToAction(nameof(Index));
        }
    }
}