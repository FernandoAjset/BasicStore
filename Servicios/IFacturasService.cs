using Dapper;
using Microsoft.Data.SqlClient;
using ProyectoPrimerParcial.Models;
using System.Data;

namespace ProyectoPrimerParcial.Servicios
{
    public interface IFacturasService
    {
        public Task<FacturaConsultaModel> ObtenerFactura(int idFactura);
        public Task<IEnumerable<FacturaConsultaModel>> ObtenerTodasFacturas();
        public Task<bool> CrearFactura(FacturaConsultaModel articulo);
        public Task<bool> ActualizarFactura(FacturaConsultaModel articulo);
        public Task<bool> EliminarFactura(int idArticulo);
    }

    public class FacturasServiceWithDapper : IFacturasService
    {
        private readonly string connectionString;

        public FacturasServiceWithDapper(
                        IConfiguration configuration
            )
        {
            this.connectionString = configuration.GetConnectionString("Default");
        }
        public async Task<FacturaConsultaModel> ObtenerFactura(int idFactura)
        {

            using var connection = new SqlConnection(connectionString);
            connection.Open();

            var parameters = new
            {
                Operacion = "R",
                FacturaID = idFactura,
                FechaFactura = "",
                ClienteID = default(int),
                NITCliente = default(string)
            };

            var facturaDictionary = new Dictionary<int, FacturaConsultaModel>();

            var result = await connection.QueryAsync<FacturaConsultaModel, DetalleFacturaConsultaModel, FacturaConsultaModel>(
                "sp_EncabezadoFactura",
                (factura, detalle) =>
                {
                    if (!facturaDictionary.TryGetValue(factura.FacturaID, out var facturaEntry))
                    {
                        facturaEntry = factura;
                        facturaEntry.Detalles = new List<DetalleFacturaConsultaModel>();
                        facturaDictionary.Add(facturaEntry.FacturaID, facturaEntry);
                    }

                    if (detalle != null)
                    {
                        facturaEntry.Detalles.Add(detalle);
                    }

                    return facturaEntry;
                },
                parameters,
                splitOn: "DetalleID",
                commandType: CommandType.StoredProcedure
            );

            return result.FirstOrDefault();
        }

        public async Task<IEnumerable<FacturaConsultaModel>> ObtenerTodasFacturas()
        {
            using var connection = new SqlConnection(connectionString);
            connection.Open();

            var parameters = new
            {
                Operacion = "R",
                FacturaID = 0,
                FechaFactura = "",
                ClienteID = default(int),
                NITCliente = default(string)
            };

            var facturaDictionary = new Dictionary<int, FacturaConsultaModel>();

            var result = await connection.QueryAsync<FacturaConsultaModel, DetalleFacturaConsultaModel, FacturaConsultaModel>(
                "sp_EncabezadoFactura",
                (factura, detalle) =>
                {
                    if (!facturaDictionary.TryGetValue(factura.FacturaID, out var facturaEntry))
                    {
                        facturaEntry = factura;
                        facturaEntry.Detalles = new List<DetalleFacturaConsultaModel>();
                        facturaDictionary.Add(facturaEntry.FacturaID, facturaEntry);
                    }

                    if (detalle != null)
                    {
                        facturaEntry.Detalles.Add(detalle);
                    }

                    return facturaEntry;
                },
                parameters,
                splitOn: "DetalleID",
                commandType: CommandType.StoredProcedure
            );

            return result.Distinct() ;
        }

        public async Task<bool> CrearFactura(FacturaConsultaModel factura)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();

            using var transaction = connection.BeginTransaction(); // Iniciar la transacción

            try
            {
                var parameters = new
                {
                    Operacion = "C",
                    FacturaID = default(int),
                    FechaFactura = factura.FechaFactura,
                    ClienteID = default(int),
                    NITCliente = factura.NITCliente
                };

                // Ejecutar el SP para crear una nueva factura dentro de la transacción
                var idFactura = await connection.QueryFirstAsync<int>(
                    "sp_EncabezadoFactura",
                    parameters,
                    commandType: CommandType.StoredProcedure,
                    transaction: transaction // Asociar la transacción con la ejecución del SP
                );

                if (idFactura != 0)
                {
                    foreach (var detalle in factura.DetallesPost)
                    {
                        var parametersDetalle = new
                        {
                            Operacion = "C",
                            DetalleID = default(int),
                            FacturaID = idFactura,
                            ArticuloID = detalle.ArticuloID,
                            Cantidad = detalle.Cantidad,
                            PrecioUnitario = detalle.PrecioUnitario,
                            Subtotal = detalle.Subtotal
                        };

                        // Ejecutar el SP para crear el detalle de la factura dentro de la transacción
                        await connection.ExecuteAsync(
                            "sp_DetalleFactura",
                            parametersDetalle,
                            commandType: CommandType.StoredProcedure,
                            transaction: transaction // Asociar la transacción con la ejecución del SP
                        );
                    }

                    transaction.Commit(); // Confirmar la transacción si todo ha ido bien
                }
                else
                {
                    transaction.Rollback(); // Deshacer la transacción si no se pudo crear la factura
                }

                return idFactura != 0;
            }
            catch (Exception ex)
            {
                // En caso de error, deshacer la transacción
                transaction.Rollback();
                throw; // Lanzar la excepción para que se maneje en la capa superior
            }
        }


        public async Task<bool> ActualizarFactura(FacturaConsultaModel factura)
        {
            using var connection = new SqlConnection(connectionString);
            connection.Open();

            var parameters = new
            {
                Operacion = "U",
                factura.FacturaID,
                FechaFactura = default(DateTime),
                ClienteID = default(int),
                NITCliente = factura.NITCliente
            };

            // Ejecutar el SP para actualizar el NIT del cliente en la factura
            var rowsAffected = await connection.ExecuteAsync(
                "sp_EncabezadoFactura",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return rowsAffected > 0;
        }

        public async Task<bool> EliminarFactura(int idFactura)
        {
            using var connection = new SqlConnection(connectionString);
            connection.Open();

            var parameters = new
            {
                Operacion = "D",
                FacturaID = idFactura,
                FechaFactura = default(DateTime),
                ClienteID = default(int),
                NITCliente = default(string)
            };

            // Ejecutar el SP para eliminar la factura y sus detalles asociados
            var rowsAffected = await connection.ExecuteAsync(
                "sp_EncabezadoFactura",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return rowsAffected > 0;
        }
    }
}
