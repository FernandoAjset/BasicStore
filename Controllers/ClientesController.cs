using Microsoft.AspNetCore.Mvc;
using ProyectoPrimerParcial.Models;
using ProyectoPrimerParcial.Servicios;

namespace ProyectoPrimerParcial.Controllers
{
    public class ClientesController : Controller
    {
        private readonly IClientesService clientesService;

        public ClientesController(IClientesService clientesService)
        {
            this.clientesService = clientesService;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                IEnumerable<Cliente> clientes = await clientesService.ObtenerTodosCliente();
                return View(clientes);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { error = ex.Message });
            }
        }

        public IActionResult CrearCliente()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CrearCliente(Cliente cliente)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var exito = await clientesService.CrearCliente(cliente);
                    if (exito)
                    {
                        return RedirectToAction("Index");
                    }
                }

                return View(cliente);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { error = ex.Message });
            }
        }

        public async Task<IActionResult> EditarCliente(string nitCliente)
        {
            try
            {
                var cliente = await clientesService.ObtenerCliente(nitCliente);
                if (cliente == null)
                {
                    return NotFound();
                }

                return View(cliente);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditarCliente(Cliente cliente)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var exito = await clientesService.ActualizarCliente(cliente);
                    if (exito)
                    {
                        return RedirectToAction("Index");
                    }
                }

                return View(cliente);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> EliminarCliente(string NITCliente)
        {
            try
            {
                var cliente = await clientesService.ObtenerCliente(NITCliente);
                if (cliente == null)
                {
                    return NotFound();
                }

                return View(cliente);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> EliminarCliente(Cliente cliente)
        {
            try
            {
                var exito = await clientesService.EliminarCliente(cliente.NITCliente);
                if (exito)
                {
                    return RedirectToAction("Index");
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { error = ex.Message });
            }
        }
    }
}
