using Microsoft.EntityFrameworkCore;
using Proyecto.Models.Domain;
using Proyecto.Repositories.Abstract;

namespace Proyecto.Repositories.Implementation
{
    public class ProveedorService : IProveedorService
    {
        private readonly DBContext _context;

        public ProveedorService(DBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Proveedor>> ObtenerTodosAsync()
        {
            return await _context.Proveedores.ToListAsync();
        }

        public async Task<Proveedor?> ObtenerPorIdAsync(int id)
        {
            return await _context.Proveedores.FindAsync(id);
        }

        public async Task<bool> AgregarAsync(Proveedor proveedor)
        {
            try
            {
                _context.Proveedores.Add(proveedor);
                await _context.SaveChangesAsync();
                return true;
            }
            catch { return false; }
        }

        public async Task<bool> ActualizarAsync(Proveedor proveedor)
        {
            try
            {
                _context.Proveedores.Update(proveedor);
                await _context.SaveChangesAsync();
                return true;
            }
            catch { return false; }
        }

        public async Task<bool> EliminarAsync(int id)
        {
            try
            {
                var proveedor = await ObtenerPorIdAsync(id);
                if (proveedor == null) return false;

                _context.Proveedores.Remove(proveedor);
                await _context.SaveChangesAsync();
                return true;
            }
            catch { return false; }
        }
    }
}