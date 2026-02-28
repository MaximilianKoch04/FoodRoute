using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Proyecto.Models.Domain
{
    public class Proveedor
    {
        [Key]
        [Required]
        public int IdProveedor { get; set; }

        public string? Nombre { get; set; }
        public string? RazonSocial { get; set; }
        public string? Cuit { get; set; }
        public string? Telefono { get; set; }

        public ICollection<RemitoIngreso> Remitos {get;set;} = new List<RemitoIngreso>();
        }
    }

