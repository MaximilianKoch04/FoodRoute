using Microsoft.EntityFrameworkCore;
using Proyecto.Models.Domain;
using Proyecto.Models.Emuns;
using Proyecto.Repositories.Abstract;

namespace Proyecto.Repositories.Implementation
{
    public class PedidoService : IPedidoService
    {
        private readonly DBContext _context;

        public PedidoService(DBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Pedido>> ObtenerTodosAsync()
        {
            // Traemos los pedidos e incluimos los datos del Cliente para poder mostrar el nombre en la tabla
            return await _context.Pedidos
                .Include(p => p.Cliente)
                .OrderByDescending(p => p.FechaEmision)
                .ToListAsync();
        }

        public async Task<Pedido?> ObtenerPorIdAsync(int id)
        {
            // Traemos el pedido completo: Cabecera + Cliente + Detalle (Líneas) + Producto de cada línea
            return await _context.Pedidos
                .Include(p => p.Cliente)
                .Include(p => p.Lineas)
                    .ThenInclude(l => l.Producto)
                .FirstOrDefaultAsync(p => p.Idpedido == id);
        }

        public async Task<bool> ActualizarEstadoAsync(int id, string nuevoEstado)
        {
            try
            {
                var pedido = await _context.Pedidos.FindAsync(id);
                if (pedido == null) return false;

                // Convertimos el string que viene del HTML al Enum
                if (Enum.TryParse(typeof(EstadoPedido), nuevoEstado, out var estadoParseado))
                {
                    pedido.EstadoActual = (EstadoPedido)estadoParseado;
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch { return false; }
        }

public async Task<bool> CrearPedidoTransaccionalAsync(Pedido pedido)
{
    using var transaction = await _context.Database.BeginTransactionAsync();

    try
    {
        foreach (var linea in pedido.Lineas)
        {
            int cantidadNecesaria = linea.CantidadSolicitada;

            // 1. Buscamos lotes usando IdProducto
            var lotesDisponibles = await _context.Lotes
                .Where(l => l.IdProducto == linea.IdProducto 
                         && l.StockActualLote > 0 
                         && (l.Estado == EstadoLote.Disponible || l.Estado == EstadoLote.ProximoAVencer))
                .OrderBy(l => l.FechaVencimiento)
                .ToListAsync();

            // 2. Descontamos stock aprovechando tu método encapsulado
            foreach (var lote in lotesDisponibles)
            {
                if (cantidadNecesaria == 0) break;

                if (lote.StockActualLote >= cantidadNecesaria)
                {
                    lote.DescontarStock(cantidadNecesaria); // Usamos tu método
                    cantidadNecesaria = 0;
                }
                else
                {
                    cantidadNecesaria -= lote.StockActualLote;
                    lote.DescontarStock(lote.StockActualLote); // Lo vacía y tu método le pone Agotado
                }

                _context.Lotes.Update(lote);
            }

            if (cantidadNecesaria > 0)
            {
                throw new Exception($"Stock insuficiente para el producto ID {linea.IdProducto}");
            }
        }

        _context.Pedidos.Add(pedido);
        await _context.SaveChangesAsync();
        await transaction.CommitAsync();
        
        return true;
    }
    catch
    {
        await transaction.RollbackAsync();
        return false;
    }
}
    }
}