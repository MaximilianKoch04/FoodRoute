using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Proyecto.Models.Domain
{
    public class Producto
    {
        [Key]
        [Required]
        public int IdProducto {get;set;}
        public string? Nombre {get;set;}
        public decimal PrecioBase { get; set; }
        public int StockFisicoTotal { get; set; }
        public bool RequiereFrio { get; set; }

        

        public ICollection<Lote> Lotes { get; set; } = new List<Lote>();
    }
}