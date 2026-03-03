using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proyecto.Models.Domain;
using Proyecto.Repositories.Abstract;

namespace Proyecto.Controllers
{
    [Authorize(Roles = "Admin,Vendedor")]
    public class ClienteController : Controller
    {
        private readonly IClienteService _clienteService;

        public ClienteController(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        // --- LISTAR ---
        public async Task<IActionResult> Index()
        {
            var clientes = await _clienteService.ObtenerTodosAsync();
            return View(clientes);
        }

        // --- CREAR ---
        [HttpGet]
        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Crear(Cliente cliente)
        {
            if (!ModelState.IsValid) return View(cliente);

            var exito = await _clienteService.AgregarAsync(cliente);
            if (exito)
            {
                TempData["Msg"] = "Cliente agregado con éxito.";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Error = "Ocurrió un error al guardar el cliente.";
            return View(cliente);
        }

        // --- EDITAR ---
        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var cliente = await _clienteService.ObtenerPorIdAsync(id);
            if (cliente == null)
            {
                TempData["Msg"] = "Cliente no encontrado.";
                return RedirectToAction(nameof(Index));
            }
            return View(cliente);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(Cliente cliente)
        {
            if (!ModelState.IsValid) return View(cliente);

            var exito = await _clienteService.ActualizarAsync(cliente);
            if (exito)
            {
                TempData["Msg"] = "Cliente actualizado correctamente.";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Error = "Error al actualizar el cliente.";
            return View(cliente);
        }

        // --- ELIMINAR ---
        [HttpPost]
        public async Task<IActionResult> Eliminar(int id)
        {
            var exito = await _clienteService.EliminarAsync(id);
            if (exito)
            {
                TempData["Msg"] = "Cliente eliminado del sistema.";
            }
            else
            {
                TempData["Msg"] = "No se pudo eliminar el cliente. Es posible que tenga pedidos asociados.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}