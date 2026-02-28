using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Proyecto.Models.Emuns;
using Proyecto.Models.Domain;
using Microsoft.EntityFrameworkCore;


namespace Proyecto.Models.Domain
{
    public class LoadDataBase
    {
        public static async Task CargarDatos(DBContext context, 
                                            UserManager<ApplicationUser> usuarioManager, 
                                            RoleManager<IdentityRole> roleManager)
        {
            
        if (!roleManager.Roles.Any())
        {
            await roleManager.CreateAsync(new IdentityRole("ADMIN"));

        }

        if (!usuarioManager.Users.Any())
        {
            var usuario = new ApplicationUser
            {
                Nombre_completo = "Maximiliano Koch",
                UserName = "Maxi.Koch",
                Email = "maxi.koch@gmail.com"
            };

            await usuarioManager.CreateAsync(usuario, "Admin2468$");
            await usuarioManager.AddToRoleAsync(usuario, "ADMIN");
        }

        if (!context.Proveedores.Any())
        {
            
            var proveedor1 = new Proveedor
            {
                Nombre = "Lautaro Berca",
                RazonSocial = "Huerta El Sol S.R.L.",
                Cuit = "20-12345678-9",
                Telefono = "3415453617"
            };

            var proveedor2 = new Proveedor
            {
                Nombre = "Juan Cruz Bocadi",
                RazonSocial = "Granja La Orgánica S.A.",
                Cuit = "20-98765432-1",
                Telefono = "3415894267"
            };

            
            var proveedor3 = new Proveedor
            {
                Nombre = "Pedro Lopez",
                RazonSocial = "Granja del Sol",
                Cuit = "20-73335367-1",
                Telefono = "3414623589"
            };

            var proveedor4 = new Proveedor
            {
                Nombre = "Carlos López",
                RazonSocial = "Verduras Del Sur",
                Cuit = "20-73335367-1",
                Telefono = "3414624589"
            };

            await context.Proveedores.AddRangeAsync(proveedor1, proveedor2, proveedor3, proveedor4);

            await context.SaveChangesAsync(); 
            
        }

        if (!context.Productos.Any())
        {
            var producto1 = new Producto
            {
                Nombre = "Lechuga",
                PrecioBase = 50.00m,
                StockFisicoTotal = 100,
                RequiereFrio = false
            };

            var producto2 = new Producto
            {
                Nombre = "Tomate",
                PrecioBase = 80.00m,
                StockFisicoTotal = 150,
                RequiereFrio = false
            };

            var producto3 = new Producto
            {
                Nombre = "Zanahoria",
                PrecioBase = 40.00m,
                StockFisicoTotal = 200,
                RequiereFrio = false
            };

            var producto4 = new Producto
            {
                Nombre = "Milanesas Soja",
                PrecioBase = 120.00m,
                StockFisicoTotal = 50,
                RequiereFrio = true
            };

            await context.Productos.AddRangeAsync(producto1, producto2, producto3, producto4);

            await context.SaveChangesAsync(); 
        }

        // ===== 3. CARGA DE LOTES (Segunda Pasada para el algoritmo FEFO) =====
            if (!context.Lotes.Any())
            {
                // 1. Vamos a buscar los productos que guardaste en la primera pasada
                var productoLechuga = context.Productos.FirstOrDefault(p => p.Nombre == "Lechuga");
                var productoTomate = context.Productos.FirstOrDefault(p => p.Nombre == "Tomate");
                var productoMilanesa = context.Productos.FirstOrDefault(p => p.Nombre == "Milanesas Soja");

                // 2. Verificamos que los haya encontrado para que no explote nada
                if (productoTomate != null && productoLechuga != null && productoMilanesa != null)
                {
                    var lote1 = new Lote
                    {
                        CodigoLote = "LOT-TOM-001",
                        Producto = productoTomate, 
                        StockActualLote = 150,
                        FechaIngreso = DateTime.Now.AddDays(-2),
                        // Fecha para que salga VERDE en tu Dashboard (Vigente)
                        FechaVencimiento = DateTime.Now.AddDays(15), 
                        Estado = EstadoLote.Disponible
                    };

                    var lote2 = new Lote
                    {
                        CodigoLote = "LOT-LEC-045",
                        Producto = productoLechuga,
                        StockActualLote = 50,
                        FechaIngreso = DateTime.Now.AddDays(-5),
                        // Fecha para que salga AMARILLO en tu Dashboard (Próximo a vencer)
                        FechaVencimiento = DateTime.Now.AddDays(2), 
                        Estado = EstadoLote.ProximoAVencer
                    };

                    var lote3 = new Lote
                    {
                        CodigoLote = "LOT-MIL-089",
                        Producto = productoMilanesa,
                        StockActualLote = 20,
                        FechaIngreso = DateTime.Now.AddDays(-30),
                        // Fecha para que salga ROJO en tu Dashboard (Vencido)
                        FechaVencimiento = DateTime.Now.AddDays(-1), 
                        Estado = EstadoLote.Vencido
                    };

                    context.Lotes.AddRange(lote1, lote2, lote3);
                    await context.SaveChangesAsync(); 
                }
            }



           
            
}   } }