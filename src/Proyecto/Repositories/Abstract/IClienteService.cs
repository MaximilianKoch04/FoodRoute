using Proyecto.Models.Domain;

namespace Proyecto.Repositories.Abstract
{
    public interface IClienteService
    {
        Task<IEnumerable<Cliente>> ObtenerTodosAsync();
        Task<Cliente?> ObtenerPorIdAsync(int id);
        Task<bool> AgregarAsync(Cliente cliente);
        Task<bool> ActualizarAsync(Cliente cliente);
        Task<bool> EliminarAsync(int id);
    }
}