using Proyecto.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace Proyecto.Models.Domain
{
    public class Cliente
    {
        [Key]
        public int Idcliente { get; set; } // id_cliente

        [Required(ErrorMessage = "La razón social es obligatoria")]
        public string? RazonSocial { get; set; }

        [Required(ErrorMessage = "El CUIT es obligatorio")]
        public string? Cuit { get; set; }

        public string? Telefono { get; set; }

        [Required(ErrorMessage = "La dirección de entrega es vital para la logística")]
        public string? DireccionEntrega { get; set; }

        

        // Por defecto, cuando lo creamos, arranca ACTIVO
        public EstadoCliente EstadoCuenta { get; set; } = EstadoCliente.ACTIVO;
        // Propiedad de navegación (Un cliente puede tener muchos pedidos)
        public virtual ICollection<Pedido>? Pedidos { get; set; }

        // Propiedad de navegación (Un cliente puede tener muchos pedidos)
        // public virtual ICollection<Pedido>? Pedidos { get; set; } // Lo descomentamos cuando hagamos Pedido
    }
}