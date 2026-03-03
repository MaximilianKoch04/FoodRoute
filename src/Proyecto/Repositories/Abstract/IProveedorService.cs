using Proyecto.Models.Domain;

namespace Proyecto.Repositories.Abstract
{
    public interface IProveedorService
    {
        Task<IEnumerable<Proveedor>> ObtenerTodosAsync();
        Task<Proveedor?> ObtenerPorIdAsync(int id);
        Task<bool> AgregarAsync(Proveedor proveedor);
        Task<bool> ActualizarAsync(Proveedor proveedor);
        Task<bool> EliminarAsync(int id);
    }
}