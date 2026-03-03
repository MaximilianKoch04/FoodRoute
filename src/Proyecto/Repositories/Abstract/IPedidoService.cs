using Proyecto.Models.Domain;

namespace Proyecto.Repositories.Abstract
{
    public interface IPedidoService
    {
        // Obtiene todos los pedidos para el listado general (con los datos del cliente)
        Task<IEnumerable<Pedido>> ObtenerTodosAsync();

        // Obtiene un pedido específico con todo su detalle (para imprimir el remito/factura)
        Task<Pedido?> ObtenerPorIdAsync(int id);

        // LA ESTRELLA DEL SISTEMA: Crea el pedido y descuenta el stock usando FEFO
        Task<bool> CrearPedidoTransaccionalAsync(Pedido pedido);

        // Permite avanzar el pedido (ej: de PENDIENTE a EN_PREPARACION)
        Task<bool> ActualizarEstadoAsync(int id, string nuevoEstado);
    }
}