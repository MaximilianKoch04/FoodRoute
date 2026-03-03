using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto.Models.Domain
{
    public class LineaDePedido
    {
        [Key]
        public int Id { get; set; } // En tu diagrama es nro_linea

        [Required]
        public int CantidadSolicitada { get; set; } // cantidad_solicitada

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PrecioUnitarioHistorico { get; set; } // precio_unitario_historico

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal SubtotalLinea { get; set; } // subtotal_linea

        // --- RELACIONES (Foreign Keys) ---
        
        // Relación con el Pedido Cabecera
        public int PedidoId { get; set; }
        [ForeignKey("PedidoId")]
        public virtual Pedido? Pedido { get; set; }

        // Relación con el Producto
        public int IdProducto { get; set; }
        [ForeignKey("IdProducto")]
        public virtual Producto? Producto { get; set; }
    }
}