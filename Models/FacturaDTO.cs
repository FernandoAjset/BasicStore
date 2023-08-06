namespace ProyectoPrimerParcial.Models
{

    public class FacturaConsultaModel
    {
        public int FacturaID { get; set; }
        public string NombreCliente { get; set; }
        public DateTime FechaFactura { get; set; }
        public string NITCliente { get; set; }
        public decimal TotalFactura { get; set; }
        public List<DetalleFacturaConsultaModel> Detalles { get; set; }
        public List<DetalleFactura> DetallesPost { get; set; }

    }

    public class DetalleFacturaConsultaModel
    {
        public int DetalleID { get; set; }
        public string NombreCliente { get; set; }

        public int ArticuloID { get; set; }
        public string NombreArticulo { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get; set; }
    }
}
