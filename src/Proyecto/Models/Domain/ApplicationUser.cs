using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity; 

namespace Proyecto.Models.Domain
{
    public class ApplicationUser : IdentityUser
    {
        public string? Nombre_completo { get; set; }
    }
}