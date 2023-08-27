using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using NuGet.Versioning;
using System.Runtime.InteropServices;
using Xela_api_pariss.Migrations;
using Xela_api_pariss.Models;
//using Xela_api_pariss.Models.Svportalreal;
using Xela_api_pariss.Servicios;

namespace Xela_api_pariss.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly ApplicacionDbContext context;

        public UsuariosController(UserManager<IdentityUser> userManager, 
            SignInManager<IdentityUser> signInManager, ApplicacionDbContext context)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.context = context; 
        }
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }
        [AllowAnonymous]
        public IActionResult PagePrincipal()
        {
            return View();
        }
        [AllowAnonymous]
        public IActionResult Registro()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Registro(RegistroViewModel modelo)
        {
            if (!ModelState.IsValid)
            {
                return View(modelo);
            }
            var usuario = new IdentityUser() { Email = modelo.UserName, UserName = modelo.Email ,
                PhoneNumber = modelo.Cedula };

            var resultado = await userManager.CreateAsync(usuario, password : modelo.Password);

            if (resultado.Succeeded)
            {
                await signInManager.SignInAsync(usuario, isPersistent: true);
                //    return RedirectToAction("Index", "Home");
                return RedirectToAction("PrincipalPage", "Home");
            }
            else {
                foreach (var error in resultado.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(modelo);
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel modelo) {
            if (!ModelState.IsValid)
            {
                return View(modelo);
            }

            var  resultado = await signInManager.PasswordSignInAsync(
                modelo.Email,modelo.Password,modelo.Recuerdame, lockoutOnFailure: false);
           
            if (resultado.Succeeded)
            {
                //return RedirectToAction("Index", "Home");
                return RedirectToAction("PrincipalPage", "Home");
            }
            else
            {
                    ModelState.AddModelError(string.Empty, "Nombre o password  incorrectos");
            }
            return View(modelo);
        }

        [HttpPost]
        public async Task<IActionResult> Logout() 
        {
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
            return RedirectToAction("Login", "Usuarios");
        }

        [HttpGet]
        public async Task<IActionResult> Listado(string mensaje = null)
        { 
            var usuarios = await context.Users.Where(u => u.AccessFailedCount == 0).Select(u => new UsuariosViewModel
            {
                Email = u.UserName,    
                UserName = u.Email,  
            }).ToListAsync();

            var modelo = new UsuariosListViewModel();
            modelo.Usuarios = usuarios;
            modelo.Mensaje = mensaje;
            return View(modelo);
        }
        public async Task<IActionResult> HacerAdmin(string email)
        {
            var usuario = await context.Users.Where(u => u.UserName == email).FirstOrDefaultAsync();

            if (usuario is null)
            { 
                return NotFound();  
            }
            await userManager.AddToRoleAsync(usuario, Constantes.RolAdmin);
            return RedirectToAction("Listado",
                routeValues: new { mensaje = "Rol Asignado correctamente a " + email });
        }
    

        [HttpPost]
        public async Task<IActionResult> Sp_execActualizUser([FromBody] Sp_execActualizUser modelo)
        {
            if (!ModelState.IsValid)
            {
                return Json("No se actualizo");
            }
            await context.Database.ExecuteSqlInterpolatedAsync($@"
                exec Sp_execActualizUser @AccessFailedCount= {modelo.AccessFailedCount}");

            return Ok("Se actualizo Correctamente");

        }
       
    }
}
