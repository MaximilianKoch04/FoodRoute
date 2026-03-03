using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.Data;
using Proyecto.Models.DTO;
using Proyecto.Models.Domain;

namespace Proyecto.Repositories.Abstract
{
    public interface IUserAuthService
    {
        Task<Status>LoginAsync(LoginModel login);

        Task LogoutAsync();
        
        Task<IEnumerable<ApplicationUser>> ObtenerUsuariosAsync();
        Task<Status> CambiarRolAsync(string userId, string nuevoRol);

        Task<Status> RegistroAsync(RegistrationModel model);
       
    }
}