using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Proyecto.Models.Emuns;

namespace Proyecto.Models.Domain
{
    public class BajaPorMerma
    {
        [Key]
        [Required]
        public int IdBaja { get; set; }
        public DateTime FechaBaja { get; set; }
        public int CantidadDescontada { get; set; }
        public MotivoBaja Motivo { get; set; }
        public string? Observaciones { get; set; }

        // Relación: De qué lote se tiró la mercadería
        public int IdLote {get;set;}
        public Lote? Lote {get;set;}
    }
}