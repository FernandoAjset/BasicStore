using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.SqlServer.Server;
using ProyectoPrimerParcial.Models;
using ProyectoPrimerParcial.Servicios;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ProyectoPrimerParcial.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly IUsuarioService usuarioService;

        public UsuariosController(IUsuarioService usuarioService)
        {
            this.usuarioService = usuarioService;
        }
        public IActionResult Login()
        {
            // Variable de sesión "IsLoggedIn" que indica si el usuario ha iniciado sesión.
            ViewData["IsLoggedIn"] = false;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Usuario usuario)
        {
            try
            {
                bool exito = await usuarioService.AutenticarUsuario(usuario);
                if (exito)
                {
                    // Convertir el objeto Usuario a formato JSON
                    string usuarioJson = JsonSerializer.Serialize(usuario);

                    // Guardar el JSON en la sesión
                    HttpContext.Session.SetString("IsLoggedIn", usuarioJson);
                    ViewData["IsLoggedIn"] = true;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewData["IsLoggedIn"] = false;
                    TempData["MostrarAlerta"] = true;
                    return View();
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { error = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LogOut()
        {
            // Quitar el JSON en la sesión
            HttpContext.Session.Clear();
            ViewData["IsLoggedIn"] = false;
            return RedirectToAction("Login", "Usuarios");
        }

        [HttpPost]
        public async Task<IActionResult> Registro(Usuario usuario)
        {
            try
            {
                // Obtener si existe un usuario con el mismo nombre
                Usuario usuarioPorNombre = await usuarioService.ObtenerUsuarioPorNombre(usuario.NombreUsuario);
                if (usuarioPorNombre is not null)
                {
                    TempData["MostrarAlerta"] = true;
                    return View();
                }
                await usuarioService.CrearUsuario(usuario);
                return View();
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { error = ex.Message });
            }
        }
    }
}
