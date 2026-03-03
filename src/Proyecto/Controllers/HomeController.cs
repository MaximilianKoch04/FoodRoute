using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // Clave para usar .Include()
using Proyecto.Models.Domain; // Para que reconozca DBContext y Lote
using Proyecto.Strategies; // Para que reconozca tus clases del Patrón Strategy
using Proyecto.Models.Emuns; // Para que reconozca el enum EstadoLote
using Microsoft.AspNetCore.Authorization; // Para usar [Authorize]

namespace Proyecto.Controllers
{   
    [Authorize]
    public class HomeController(DBContext context) : Controller
    {
        // 1. Variable para guardar la conexión a la base de datos
        private readonly DBContext _context = context;

        public async Task<IActionResult> Index()
        {
           var lotes = await _context.Lotes.Include(l => l.Producto).ToListAsync();

    // Calculamos los datos para las tarjetas
    ViewBag.StockTotal = lotes.Sum(l => l.StockActualLote);
    ViewBag.ProximosAVencer = lotes.Count(l => l.Estado == EstadoLote.ProximoAVencer);
    ViewBag.LotesVencidos = lotes.Count(l => l.Estado == EstadoLote.Vencido);


            foreach (var lote in lotes)
            {
                IEvaluadorEstadoStrategy estrategia;

                // Si el producto lleva frío, usamos la estrategia estricta
                if (lote.Producto.RequiereFrio)
                {
                    estrategia = new EstadoFrescosStrategy();
                }
                else // Si es seco/congelado, usamos la estrategia más flexible
                {
                    estrategia = new EstadoCongeladosStrategy();
                }

                // Calculamos el estado real y lo guardamos en el lote
                lote.Estado = estrategia.EvaluarEstado(lote.FechaVencimiento);
            }

            // 5. ¡Le mandamos los lotes calculados a la Vista HTML!
            return View(lotes);
        }
    }
}