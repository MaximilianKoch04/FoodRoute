using Microsoft.EntityFrameworkCore;
using Proyecto.Models.Domain;
using Proyecto.Repositories.Abstract;

namespace Proyecto.Repositories.Implementation
{
    public class ClienteService : IClienteService
    {
        private readonly DBContext _context;

        public ClienteService(DBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Cliente>> ObtenerTodosAsync()
        {
            return await _context.Clientes.ToListAsync();
        }

        public async Task<Cliente?> ObtenerPorIdAsync(int id)
        {
            return await _context.Clientes.FindAsync(id);
        }

        public async Task<bool> AgregarAsync(Cliente cliente)
        {
            try
            {
                _context.Clientes.Add(cliente);
                await _context.SaveChangesAsync();
                return true;
            }
            catch { return false; }
        }

        public async Task<bool> ActualizarAsync(Cliente cliente)
        {
            try
            {
                _context.Clientes.Update(cliente);
                await _context.SaveChangesAsync();
                return true;
            }
            catch { return false; }
        }

        public async Task<bool> EliminarAsync(int id)
        {
            try
            {
                var cliente = await ObtenerPorIdAsync(id);
                if (cliente == null) return false;

                _context.Clientes.Remove(cliente);
                await _context.SaveChangesAsync();
                return true;
            }
            catch { return false; }
        }
    }
}