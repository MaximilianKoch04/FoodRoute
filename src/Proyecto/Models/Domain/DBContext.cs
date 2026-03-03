using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Proyecto.Models.Domain;
using Microsoft.EntityFrameworkCore;


namespace Proyecto.Models.Domain
{
    public class DBContext: IdentityDbContext<ApplicationUser>

    {

        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<RemitoIngreso>()
                .Property(r => r.CostoTotal)
                .HasColumnType("decimal(18,2)");

            builder.Entity<DetalleIngreso>()
                .Property(d => d.PrecioUnitario)
                .HasColumnType("decimal(18,2)");

            builder.Entity<DetalleIngreso>()
                .HasOne(d => d.Remito)
                .WithMany(r => r.Detalles)
                .HasForeignKey(d => d.IdRemito)
                .OnDelete(DeleteBehavior.Restrict);

            // Proteger los Remitos: Si intento borrar un Proveedor que ya tiene remitos, la BD me lo impide.
            builder.Entity<RemitoIngreso>()
                .HasOne(r => r.Proveedor)
                .WithMany(p => p.Remitos)
                .HasForeignKey(r => r.IdProveedor)
                .OnDelete(DeleteBehavior.Restrict);

            // Proteger los Productos: No puedo borrar un producto si ya tiene lotes ingresados.
            builder.Entity<Lote>()
                .HasOne(l => l.Producto)
                .WithMany(p => p.Lotes)
                .HasForeignKey(l => l.IdProducto)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Producto>()
                .Property(p => p.PrecioBase)
                .HasColumnType("decimal(18,2)");
        }





        public DbSet<Lote> Lotes { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Proveedor> Proveedores { get; set; }
        public DbSet<RemitoIngreso> RemitosIngreso { get; set; }
        public DbSet<DetalleIngreso> DetallesIngreso { get; set; }
        public DbSet<BajaPorMerma> BajasPorMerma { get; set; }

        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<LineaDePedido> LineasDePedido { get; set; }

        public DbSet<Cliente> Clientes { get; set; }
    }
}