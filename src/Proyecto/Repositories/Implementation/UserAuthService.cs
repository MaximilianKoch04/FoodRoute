using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Identity.Client;
using Proyecto.Repositories.Abstract;
using Proyecto.Models.DTO;
using Microsoft.AspNetCore.Identity;
using Proyecto.Models.Domain;
using Microsoft.EntityFrameworkCore;


namespace Proyecto.Repositories.Implementation
{
    public class UserAuthService : IUserAuthService 
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public UserAuthService(UserManager<ApplicationUser> userManager, 
                                SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        public async Task<Status> LoginAsync(LoginModel login)
        {
            var status = new Status();
            var user = await userManager.FindByNameAsync(login.Username!);

            if (user == null)
            {
                status.StatusCode = 0;
                status.Message = "Usuario no encontrado.";
                return status;
            }

           if (!await userManager.CheckPasswordAsync(user, login.Password!))
            {
                status.StatusCode = 0;
                status.Message = "Contraseña incorrecta.";
                return status;
            }

            var resultado = await signInManager.PasswordSignInAsync(user, login.Password!, true , false);

            if (!resultado.Succeeded)
            {
                status.StatusCode = 0;
                status.Message = "Las credenciales son incorrectas.";
        
            }

            status.StatusCode = 1;
            status.Message = "Inicio de sesión exitoso.";

            return status;  

            

        }

        public async Task LogoutAsync()
        {
            await signInManager.SignOutAsync();
        }

        public async Task<IEnumerable<ApplicationUser>> ObtenerUsuariosAsync()
        {
            // Busca a todos los usuarios en la base de datos
            return await userManager.Users.ToListAsync();
        }

        public async Task<Status> CambiarRolAsync(string userId, string nuevoRol)
{
    var status = new Status();
    var user = await userManager.FindByIdAsync(userId);
    
    if (user == null)
    {
        status.StatusCode = 0;
        status.Message = "Usuario no encontrado.";
        return status;
    }

    // Limpiamos los roles actuales para evitar conflictos
    var rolesActuales = await userManager.GetRolesAsync(user);
    await userManager.RemoveFromRolesAsync(user, rolesActuales);

    // USAMOS EL NOMBRE DEL ROL TAL CUAL VIENE
    // Si falla acá, es porque el 'nuevoRol' no existe en la tabla AspNetRoles
    var resultado = await userManager.AddToRoleAsync(user, nuevoRol);

    if (!resultado.Succeeded)
    {
        status.StatusCode = 0;
        status.Message = "Error: " + string.Join(", ", resultado.Errors.Select(x => x.Description));
        return status;
    }

    status.StatusCode = 1;
    status.Message = "Rol actualizado correctamente.";
    return status;
}

public async Task<Status> RegistroAsync(RegistrationModel model)
{
    var status = new Status();
    
    // 1. Verificamos si el usuario ya existe
    var userExists = await userManager.FindByNameAsync(model.Username!);
    if (userExists != null)
    {
        status.StatusCode = 0;
        status.Message = "El nombre de usuario ya está en uso.";
        return status;
    }

    // 2. Creamos el objeto de usuario (ApplicationUser)
    var user = new ApplicationUser
    {
        UserName = model.Username,
        Email = model.Email,
        Nombre_completo = model.NombreCompleto,
        EmailConfirmed = true // Para que pueda entrar directo
    };

    // 3. Lo guardamos en la base con su clave hasheada
    var result = await userManager.CreateAsync(user, model.Password!);
    
    if (result.Succeeded)
    {
        // 4. Le asignamos el rol que eligió el Admin
        if (!string.IsNullOrEmpty(model.Role))
        {
            await userManager.AddToRoleAsync(user, model.Role);
        }
        
        status.StatusCode = 1;
        status.Message = "Usuario creado exitosamente.";
    }
    else
    {
        status.StatusCode = 0;
        status.Message = "Error: " + string.Join(", ", result.Errors.Select(x => x.Description));
    }

    return status;
}
    }
}