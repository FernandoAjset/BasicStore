using Microsoft.AspNetCore.Mvc;
using ProyectoPrimerParcial.Models;
using ProyectoPrimerParcial.Servicios;

namespace ProyectoPrimerParcial.Controllers
{
    public class ArticulosController : Controller
    {
        private readonly IArticulosService articulosService;
        public ArticulosController(IArticulosService articulosService)
        {
            this.articulosService = articulosService;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Articulo> articulos = await articulosService.ObtenerTodosArticulos();
            return View(articulos);
        }
        public IActionResult CrearArticulo()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CrearArticulo(Articulo articulo)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string precioText = articulo.PrecioText;

                    if (decimal.TryParse(precioText, out decimal precioConvert))
                    {
                        articulo.Precio = precioConvert;
                    }
                    else
                    {
                        return View(articulo);
                    }
                    var exito = await articulosService.CrearArticulo(articulo);
                    if (exito)
                    {
                        return RedirectToAction("Index");
                    }
                }

                return View(articulo);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { error = ex.Message });
            }
        }
        public async Task<IActionResult> EditarArticulo(int idArticulo)
        {
            try
            {
                var articulo = await articulosService.ObtenerArticulo(idArticulo);
                if (articulo == null)
                    return NotFound();

                return View(articulo);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditarArticulo(Articulo articulo)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string precioText = articulo.PrecioText;

                    if (decimal.TryParse(precioText, out decimal precioConvert))
                    {
                        articulo.Precio = precioConvert;
                    }
                    else
                    {
                        return View(articulo);
                    }
                    var result = await articulosService.ActualizarArticulo(articulo);
                    if (result)
                        return RedirectToAction("Index");
                    else
                        ModelState.AddModelError("", "Error al actualizar el artículo.");
                }
                return View(articulo);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { error = ex.Message });
            }
        }

        public async Task<IActionResult> EliminarArticulo(int idArticulo)
        {
            try
            {
                var articulo = await articulosService.ObtenerArticulo(idArticulo);
                if (articulo == null)
                    return NotFound();

                return View(articulo);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> EliminarArticulo(Articulo articulo)
        {
            try
            {
                await articulosService.EliminarArticulo(articulo.ArticuloID ?? 0);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { error = ex.Message });
            }
        }
    }
}
