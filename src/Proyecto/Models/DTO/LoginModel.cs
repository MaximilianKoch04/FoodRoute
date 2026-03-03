using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Proyecto.Models.DTO
{
    public class LoginModel
    {
        [Required]
        public string?  Username { get; set; }
        [Required]
        public string? Password { get; set; }


    
    }
}