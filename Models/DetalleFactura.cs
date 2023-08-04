namespace ProyectoPrimerParcial.Models
{
    public class DetalleFactura
    {
        public int DetalleID { get; set; }
        public int FacturaID { get; set; }
        public int ArticuloID { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get; set; }
    }

}
