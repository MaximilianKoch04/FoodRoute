using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Proyecto.Models.Domain;

namespace Proyecto.Models
{
    public class DetalleIngreso
    {
        public int Id { get; set; }
        public string? Descripcion { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public int IdRemito { get; set; }

        [ForeignKey("IdRemito")]
        public RemitoIngreso? Remito { get; set; }

        // 2. Relación hacia el Catálogo: ¿Qué producto físico es?
        [Required]
        public int IdProducto { get; set; }
        
        [ForeignKey("IdProducto")]
        public Producto? Producto { get; set; }

        // 3. Relación hacia el Inventario: El Lote que se va a crear a partir de este renglón
        // (Puede ser null inicialmente hasta que el Encargado confirme el ingreso)
        public int? IdLoteGenerado { get; set; }
        
        [ForeignKey("IdLoteGenerado")]
        public Lote? Lote { get; set; }
    }
}