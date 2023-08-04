namespace ProyectoPrimerParcial.Models
{
    public class EncabezadoFactura
    {
        public int FacturaID { get; set; }
        public DateTime FechaFactura { get; set; }
        public string NITCliente { get; set; }
        public decimal TotalFactura { get; set; }
    }

}
