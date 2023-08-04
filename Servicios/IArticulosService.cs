using ProyectoPrimerParcial.Models;

namespace ProyectoPrimerParcial.Servicios
{
    public interface IArticulosService
    {
        public Task<Articulo> ObtenerArticulo(string NITCliente);
        public Task<IEnumerable<Articulo>> ObtenerTodosArticulos();
        public Task<bool> CrearArticulo(Articulo cliente);
        public Task<bool> ActualizarArticulo(Articulo cliente);
        public Task<bool> EliminarArticulo(string NITCliente);
    }

}
