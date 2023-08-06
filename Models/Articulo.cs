namespace ProyectoPrimerParcial.Models
{
    public class Articulo
    {
        public int? ArticuloID { get; set; }
        public string NombreArticulo { get; set; }
        public string Descripcion { get; set; }
        public string PrecioText { get; set; }

        public decimal Precio { get; set; }
        public int Stock { get; set; }
    }
}
