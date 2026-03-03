using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Proyecto.Models.Enums;

namespace Proyecto.Models.Domain
{
    public class Pedido
    {
        [Key]
        public int Idpedido { get; set; } // En tu diagrama es nro_pedido

        [Required]
        public DateTime FechaEmision { get; set; } = DateTime.Now; // fecha_emision

        public DateTime? FechaEntregaEstimada { get; set; } // fecha_entrega_estimada

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal SubtotalBruto { get; set; } // subtotal_bruto

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalFinal { get; set; } // total_final

        public EstadoPedido EstadoActual { get; set; } = EstadoPedido.PENDIENTE; // estado_actual
        
        public MediosDePago FormaPago { get; set; } // forma_pago

        // --- RELACIONES (Foreign Keys) ---

        // Relación con el Cliente (1 Pedido pertenece a 1 Cliente)
        [Required(ErrorMessage = "Debe seleccionar un cliente validado.")]
        public int ClienteId { get; set; }
        [ForeignKey("ClienteId")]
        public virtual Cliente? Cliente { get; set; }

        // Relación con el Vendedor (Usuario de Identity)
        public string? VendedorId { get; set; }
        [ForeignKey("VendedorId")]
        public virtual ApplicationUser? Vendedor { get; set; }

        // Relación con el Detalle (1 Pedido tiene N Líneas)
        public virtual List<LineaDePedido> Lineas { get; set; } = new List<LineaDePedido>();
    }
}