using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Proyecto.Models.Domain;
namespace Proyecto.Models.Domain
{
    public class RemitoIngreso
    {
      [Key]
      [Required]
        public int IdRemito { get; set; }
        public string? NumeroRemito { get; set; } 
        public DateTime FechaRecepcion { get; set; }
        public decimal CostoTotal { get; set; }


        // 1. Relación con el Proveedor (Quién trajo la mercadería)
        [Required]
        public int IdProveedor { get; set; }
        
        [ForeignKey("IdProveedor")]
        public Proveedor? Proveedor { get; set; }

        // 2. Relación con el Detalle (Los renglones del remito)
        // Inicializamos la lista para que no tire error de "Referencia Nula" al usarla
        public ICollection<DetalleIngreso> Detalles { get; set; } = new List<DetalleIngreso>();
    }
}