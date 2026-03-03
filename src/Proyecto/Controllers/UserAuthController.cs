using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Proyecto.Models.DTO;
using Proyecto.Repositories.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity; // De acá viene el UserManager
using Proyecto.Models.Domain;        // De acá viene tu ApplicationUser
using Microsoft.EntityFrameworkCore; // Para poder usar .ToListAsync()


namespace Proyecto.Controllers
{
    public class UserAuthController : Controller 
    {
        private readonly IUserAuthService _authService;

        public UserAuthController(IUserAuthService authService)
        {
            _authService = authService;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel login)
        {
            if(!ModelState.IsValid)
            {
                return View(login);
            }

            var resultado = await _authService.LoginAsync(login);

            if (resultado.StatusCode == 1)
            {
                return RedirectToAction("Index", "Home");
            }

            else
            {
                TempData["Msg"] = resultado.Message;
                return View(login);
            }
        }

        public async Task<IActionResult> Logout()
        {
            await _authService.LogoutAsync();
            return RedirectToAction(nameof(Login));
        }


        [Authorize(Roles = "Admin")]
public async Task<IActionResult> Gestion()
{
    // Llamamos al servicio (no tocamos la base de datos directo desde acá)
    var usuarios = await _authService.ObtenerUsuariosAsync();
    return View(usuarios);
}

[HttpPost]
[Authorize(Roles = "Admin")]
public async Task<IActionResult> UpdateRole(string userId, string newRole)
{
    // Llamamos al servicio para que haga la magia
    var resultado = await _authService.CambiarRolAsync(userId, newRole);
    
    TempData["Msg"] = resultado.Message;
    return RedirectToAction(nameof(Gestion));
}

// 1. Este método es para que el link funcione y te MUESTRE el formulario vacío
[Authorize(Roles = "Admin")]
public IActionResult Registro()
{
    return View();
}

// 2. Este método es para cuando el Admin aprieta "Guardar Usuario"
[HttpPost]
[Authorize(Roles = "Admin")]
public async Task<IActionResult> Registro(RegistrationModel model)
{
    if (!ModelState.IsValid) 
        return View(model);

    var resultado = await _authService.RegistroAsync(model);
    
    if (resultado.StatusCode == 1)
    {
        TempData["Msg"] = "Empleado creado con éxito.";
        return RedirectToAction(nameof(Gestion)); 
    }
    
    ViewBag.Error = resultado.Message;
    return View(model);
}

        
    }
   
}