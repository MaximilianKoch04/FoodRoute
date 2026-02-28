using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Proyecto.Models.Emuns;

namespace Proyecto.Models.Domain
{
    public class Lote
    {
        [Key]
        [Required]
        public int IdLote { get; set; }
        
        [Required]
        public string? CodigoLote { get; set; }
        public DateTime FechaIngreso { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public int StockActualLote { get; set; }
        public EstadoLote Estado { get; set; }

        // Relaciones (Foreign Keys en el futuro)
        public int IdProducto {get;set;}

        public Producto Producto { get; set; } = null!;

        public void VerificarCaducidad(DateTime fechaActual)
        {
            if (FechaVencimiento < fechaActual)
            {
                Estado = EstadoLote.Vencido;
            }
            else if ((FechaVencimiento - fechaActual).TotalDays <= 7)
            {
                Estado = EstadoLote.ProximoAVencer;
            }
        }

        public bool DescontarStock(int cantidad)
        {
            if (cantidad > StockActualLote) return false;
            
            StockActualLote -= cantidad;
            if (StockActualLote == 0)
            {
                Estado = EstadoLote.Agotado;
            }
            return true;
        }

        
    }
}