using System.ComponentModel.DataAnnotations;

namespace Proyecto.Models.DTO
{
    public class RegistrationModel
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string? NombreCompleto { get; set; }

        [Required(ErrorMessage = "El nombre de usuario es obligatorio")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "El email es obligatorio")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [MinLength(6, ErrorMessage = "Mínimo 6 caracteres")]
        public string? Password { get; set; }

        public string? Role { get; set; } // El rol que elegirá el Admin
    }
}