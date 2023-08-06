using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoPrimerParcial.Models;
using ProyectoPrimerParcial.Servicios;

namespace ProyectoPrimerParcial.Controllers
{
    [Authorize]
    public class ArticulosController : Controller
    {
        private readonly IArticulosService articulosService;

        /// <summary>
        /// Constructor del controlador de Artículos.
        /// </summary>
        /// <param name="articulosService">Servicio para la gestión de artículos.</param>
        public ArticulosController(IArticulosService articulosService)
        {
            this.articulosService = articulosService;
        }

        /// <summary>
        /// Endpoint para mostrar la lista de todos los artículos.
        /// </summary>
        /// <returns>Vista con la lista de todos los artículos.</returns>
        public async Task<IActionResult> Index()
        {
            IEnumerable<Articulo> articulos = await articulosService.ObtenerTodosArticulos();
            return View(articulos);
        }

        /// <summary>
        /// Endpoint para mostrar el formulario de creación de un nuevo artículo.
        /// </summary>
        /// <returns>Vista con el formulario de creación de artículo.</returns>
        public IActionResult CrearArticulo()
        {
            return View();
        }

        /// <summary>
        /// Endpoint para procesar la creación de un nuevo artículo.
        /// </summary>
        /// <param name="articulo">Datos del artículo a crear.</param>
        /// <returns>Redirecciona a la lista de artículos si se crea exitosamente, de lo contrario, muestra el formulario de creación con mensajes de error.</returns>
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

        /// <summary>
        /// Endpoint para mostrar el formulario de edición de un artículo específico.
        /// </summary>
        /// <param name="idArticulo">Identificador del artículo a editar.</param>
        /// <returns>Vista con el formulario de edición del artículo si se encuentra, de lo contrario, muestra una página de error.</returns>
        public async Task<IActionResult> EditarArticulo(int idArticulo)
        {
            try
            {
                // Obtener el artículo correspondiente al ID proporcionado utilizando el servicio de artículos.
                var articulo = await articulosService.ObtenerArticulo(idArticulo);

                // Si el artículo no se encuentra, retornar una respuesta 404 (Not Found).
                if (articulo == null)
                    return NotFound();

                // Mostrar la vista con el formulario de edición, pasando el artículo como modelo a la vista.
                return View(articulo);
            }
            catch (Exception ex)
            {
                // En caso de error, redireccionar a la página de error con el mensaje de error.
                return RedirectToAction("Error", "Home", new { error = ex.Message });
            }
        }

        /// <summary>
        /// Endpoint para procesar la edición de un artículo.
        /// </summary>
        /// <param name="articulo">Datos del artículo actualizado desde el formulario de edición.</param>
        /// <returns>Redirecciona a la lista de artículos si se actualiza exitosamente, de lo contrario, muestra el formulario de edición con mensajes de error.</returns>
        [HttpPost]
        public async Task<IActionResult> EditarArticulo(Articulo articulo)
        {
            try
            {
                // Verificar si el modelo del artículo recibido desde el formulario es válido.
                if (ModelState.IsValid)
                {
                    // Obtener el texto del precio del artículo.
                    string precioText = articulo.PrecioText;

                    // Convertir el precio del artículo a tipo decimal utilizando TryParse.
                    if (decimal.TryParse(precioText, out decimal precioConvert))
                    {
                        articulo.Precio = precioConvert;
                    }
                    else
                    {
                        // Si el precio no es válido, mostrar el formulario de edición con mensajes de error.
                        return View(articulo);
                    }

                    // Llamar al servicio de artículos para actualizar el artículo en la base de datos.
                    var result = await articulosService.ActualizarArticulo(articulo);

                    // Si la actualización es exitosa, redireccionar a la lista de artículos.
                    if (result)
                        return RedirectToAction("Index");
                    else
                        // Si la actualización falla, agregar un mensaje de error al modelo y mostrar el formulario de edición nuevamente.
                        ModelState.AddModelError("", "Error al actualizar el artículo.");
                }

                // Si el modelo no es válido, mostrar el formulario de edición con mensajes de error.
                return View(articulo);
            }
            catch (Exception ex)
            {
                // En caso de error, redireccionar a la página de error con el mensaje de error.
                return RedirectToAction("Error", "Home", new { error = ex.Message });
            }
        }

        /// <summary>
        /// Endpoint para mostrar el formulario de confirmación de eliminación de un artículo específico.
        /// </summary>
        /// <param name="idArticulo">Identificador del artículo a eliminar.</param>
        /// <returns>Vista con el formulario de confirmación de eliminación del artículo si se encuentra, de lo contrario, muestra una página de error.</returns>
        public async Task<IActionResult> EliminarArticulo(int idArticulo)
        {
            try
            {
                // Obtener el artículo correspondiente al ID proporcionado utilizando el servicio de artículos.
                var articulo = await articulosService.ObtenerArticulo(idArticulo);

                // Si el artículo no se encuentra, retornar una respuesta 404 (Not Found).
                if (articulo == null)
                    return NotFound();

                // Mostrar la vista con el formulario de confirmación de eliminación, pasando el artículo como modelo a la vista.
                return View(articulo);
            }
            catch (Exception ex)
            {
                // En caso de error, redireccionar a la página de error con el mensaje de error.
                return RedirectToAction("Error", "Home", new { error = ex.Message });
            }
        }

        /// <summary>
        /// Endpoint para procesar la eliminación de un artículo.
        /// </summary>
        /// <param name="articulo">Datos del artículo a eliminar.</param>
        /// <returns>Redirecciona a la lista de artículos si se elimina exitosamente, de lo contrario, muestra una página de error.</returns>
        [HttpPost]
        public async Task<IActionResult> EliminarArticulo(Articulo articulo)
        {
            try
            {
                // Llamar al servicio de artículos para eliminar el artículo de la base de datos utilizando su ID.
                await articulosService.EliminarArticulo(articulo.ArticuloID ?? 0);

                // Redireccionar a la lista de artículos después de eliminar exitosamente.
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // En caso de error, redireccionar a la página de error con el mensaje de error.
                return RedirectToAction("Error", "Home", new { error = ex.Message });
            }
        }
    }
}
