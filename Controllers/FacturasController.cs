using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProyectoPrimerParcial.Models;
using ProyectoPrimerParcial.Servicios;

namespace ProyectoPrimerParcial.Controllers
{
    [Authorize]
    public class FacturasController : Controller
    {
        private readonly IFacturasService facturasService;
        private readonly IClientesService clientesService;
        private readonly IArticulosService articulosService;

        public FacturasController(
            IFacturasService facturasService,
            IClientesService clientesService,
            IArticulosService articulosService
            )
        {
            this.facturasService = facturasService;
            this.clientesService = clientesService;
            this.articulosService = articulosService;
        }
        public async Task<IActionResult> CrearFactura()
        {
            var model = new FacturaViewModel
            {
                Clientes = await ObtenerListaClientes(),
                Articulos = await ObtenerListaArticulos(),
            };

            model.ClientesSelectList = new SelectList(model.Clientes, "NITCliente", "NombreCliente");
            model.ArticulosSelectList = new SelectList(model.Articulos, "ArticuloID", "NombreArticulo");

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] FacturaViewModel facturaViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    // Ejemplo de cómo obtener los detalles de la tabla:
                    var detalles = facturaViewModel.DetalleFacturas;

                    // Ejemplo de cómo obtener el cliente seleccionado:
                    var clienteNIT = facturaViewModel.NITCliente;
                    FacturaConsultaModel facturaConsulta = new();
                    facturaConsulta.FechaFactura = DateTime.Now;
                    facturaConsulta.NITCliente = facturaViewModel.NITCliente;
                    facturaConsulta.TotalFactura = 0;
                    facturaConsulta.DetallesPost = facturaViewModel.DetalleFacturas;
                    var operacion = await facturasService.CrearFactura(facturaConsulta);
                    if (!operacion)
                    {
                        return RedirectToAction("Error", "Home", new { error = "No se pudo crear la factura" });
                    }
                    else
                    {
                        return RedirectToAction("Index");
                    }
                }

                // Si el ModelState no es válido, regresa a la misma vista con el modelo y los mensajes de validación.
                var model = new FacturaViewModel
                {
                    Clientes = await ObtenerListaClientes(),
                    Articulos = await ObtenerListaArticulos(),
                };

                model.ClientesSelectList = new SelectList(model.Clientes, "NITCliente", "NombreCliente");
                model.ArticulosSelectList = new SelectList(model.Articulos, "ArticuloID", "NombreArticulo");

                return View(model);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { error = ex.Message });
            }
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var facturas = await facturasService.ObtenerTodasFacturas();
                return View(facturas);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { error = ex.Message });
            }
        }
        public async Task<IActionResult> ImprimirFactura(int id)
        {
            try
            {
                var factura = await facturasService.ObtenerFactura(id);
                if (factura == null)
                {
                    return RedirectToAction("Error", "Home", new { error = "La factura no existe" });
                }

                return View("ImprimirFactura", factura);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { error = ex.Message });
            }
        }

        // Método para simular la obtención de la lista de clientes (reemplazar con la lógica real)
        private async Task<IEnumerable<Cliente>> ObtenerListaClientes()
        {
            IEnumerable<Cliente> clientes = await clientesService.ObtenerTodosCliente();

            return clientes;
        }

        // Método para simular la obtención de la lista de artículos (reemplazar con la lógica real)
        private async Task<IEnumerable<Articulo>> ObtenerListaArticulos()
        {
            IEnumerable<Articulo> articulos = await articulosService.ObtenerTodosArticulos();

            return articulos;
        }

    }
}
