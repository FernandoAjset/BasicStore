using Dapper;
using Microsoft.Data.SqlClient;
using ProyectoPrimerParcial.Models;
using System.Data;

namespace ProyectoPrimerParcial.Servicios
{
    public interface IClientesService
    {
        public Task<Cliente> ObtenerCliente(string NITCliente);
        public Task<IEnumerable<Cliente>> ObtenerTodosCliente();
        public Task<bool> CrearCliente(Cliente cliente);
        public Task<bool> ActualizarCliente(Cliente cliente);
        public Task<bool> EliminarCliente(string NITCliente);
    }
    public class ClientesServicioWithDapper : IClientesService
    {
        private readonly string connectionString;

        public ClientesServicioWithDapper(
                        IConfiguration configuration
            )
        {
            this.connectionString = configuration.GetConnectionString("Default");
        }

        public async Task<Cliente> ObtenerCliente(string NITCliente)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();

            // Ejecutar el procedimiento almacenado para obtener un cliente por su ID
            var cliente = await connection.QueryFirstOrDefaultAsync<Cliente>(
                "sp_Clientes_CRUD",
                new
                {
                    Operacion = 'R',
                    NITCliente,
                    NombreCliente = "",
                    Direccion = "",
                    Telefono = "",
                    Email = ""
                },
                commandType: CommandType.StoredProcedure
            );

            return cliente;
        }

        public async Task<IEnumerable<Cliente>> ObtenerTodosCliente()
        {
            using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();

            // Ejecutar el procedimiento almacenado para obtener un cliente por su ID
            var clientes = await connection.QueryAsync<Cliente>(
                "sp_Clientes_CRUD",
                new
                {
                    Operacion = 'R',
                    NITCliente = "",
                    NombreCliente = "",
                    Direccion = "",
                    Telefono = "",
                    Email = ""
                },
                commandType: CommandType.StoredProcedure
            );

            return clientes;
        }
        public async Task<bool> CrearCliente(Cliente cliente)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();

            // Ejecutar el procedimiento almacenado para crear un nuevo cliente
            var result = await connection.ExecuteAsync(
                "sp_Clientes_CRUD",
                new
                {
                    Operacion = 'C',
                    cliente.NITCliente,
                    cliente.NombreCliente,
                    cliente.Direccion,
                    cliente.Telefono,
                    cliente.Email
                },
                commandType: CommandType.StoredProcedure
            );

            return result > 0;
        }

        public async Task<bool> ActualizarCliente(Cliente cliente)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();

            // Ejecutar el procedimiento almacenado para actualizar un cliente
            await connection.ExecuteAsync(
                 "sp_Clientes_CRUD",
                 new
                 {
                     Operacion = 'U',
                     cliente.NITCliente,
                     cliente.NombreCliente,
                     cliente.Direccion,
                     cliente.Telefono,
                     cliente.Email
                 },
                 commandType: CommandType.StoredProcedure
             );

            return true;
        }

        public async Task<bool> EliminarCliente(string NITCliente)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();

            // Ejecutar el procedimiento almacenado para eliminar un cliente
            await connection.ExecuteAsync(
               "sp_Clientes_CRUD",
               new
               {
                   Operacion = 'D',
                   NITCliente = NITCliente,
                   NombreCliente = "",
                   Direccion = "",
                   Telefono = "",
                   Email = ""
               },
               commandType: CommandType.StoredProcedure
           );

            return true;
        }
    }
}
