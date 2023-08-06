using Microsoft.AspNetCore.Mvc.Rendering;

namespace ProyectoPrimerParcial.Models
{
    public class FacturaViewModel
    {
        public string NITCliente { get; set; }
        public int? ArticuloID { get; set; }
        public decimal? PrecioUnitario { get; set; }
        public int? Cantidad { get; set; }
        public decimal? Subtotal { get; set; }


        public IEnumerable<Cliente>? Clientes { get; set; }
        public IEnumerable<Articulo>? Articulos { get; set; }

        public SelectList? ClientesSelectList { get; set; }
        public SelectList? ArticulosSelectList { get; set; }
        public List<DetalleFactura>? DetalleFacturas { get; set; }
    }
}
