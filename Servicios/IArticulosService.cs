using Dapper;
using Microsoft.Data.SqlClient;
using ProyectoPrimerParcial.Models;
using System.Data;

namespace ProyectoPrimerParcial.Servicios
{
    public interface IArticulosService
    {
        public Task<Articulo> ObtenerArticulo(int idArticulo);
        public Task<IEnumerable<Articulo>> ObtenerTodosArticulos();
        public Task<bool> CrearArticulo(Articulo articulo);
        public Task<bool> ActualizarArticulo(Articulo articulo);
        public Task<bool> EliminarArticulo(int idArticulo);
    }
    public class ArticulosServiceWithDapper : IArticulosService
    {
        private readonly string connectionString;

        public ArticulosServiceWithDapper(
                        IConfiguration configuration
            )
        {
            this.connectionString = configuration.GetConnectionString("Default");
        }

        public async Task<Articulo> ObtenerArticulo(int idArticulo)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();

            // Ejecutar el procedimiento almacenado para obtener un artículo por su NITCliente
            var articulo = await connection.QueryFirstOrDefaultAsync<Articulo>(
                "sp_Articulos_CRUD",
                new
                {
                    Operacion = 'R',
                    ArticuloID = idArticulo,
                    NombreArticulo = "",
                    Descripcion = "",
                    Precio = 0,
                    Stock = 0
                },
                commandType: CommandType.StoredProcedure
            );

            return articulo;
        }

        public async Task<IEnumerable<Articulo>> ObtenerTodosArticulos()
        {
            using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();

            // Ejecutar el procedimiento almacenado para obtener todos los artículos
            var articulos = await connection.QueryAsync<Articulo>(
                "sp_Articulos_CRUD",
                new
                {
                    Operacion = 'R',
                    ArticuloID = (int?)null,
                    NombreArticulo = "",
                    Descripcion = "",
                    Precio = 0,
                    Stock = 0
                },
                commandType: CommandType.StoredProcedure
            );

            return articulos;
        }

        public async Task<bool> CrearArticulo(Articulo articulo)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();

            // Ejecutar el procedimiento almacenado para crear un nuevo artículo
            var result = await connection.ExecuteAsync(
                "sp_Articulos_CRUD",
                new
                {
                    Operacion = 'C',
                    ArticuloID = 0,
                    articulo.NombreArticulo,
                    articulo.Descripcion,
                    articulo.Precio,
                    articulo.Stock
                },
                commandType: CommandType.StoredProcedure
            );

            return result > 0;
        }

        public async Task<bool> ActualizarArticulo(Articulo articulo)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();

            // Ejecutar el procedimiento almacenado para actualizar un artículo
            await connection.ExecuteAsync(
                "sp_Articulos_CRUD",
                new
                {
                    Operacion = 'U',
                    articulo.ArticuloID,
                    articulo.NombreArticulo,
                    articulo.Descripcion,
                    articulo.Precio,
                    articulo.Stock
                },
                commandType: CommandType.StoredProcedure
            );

            return true;
        }

        public async Task<bool> EliminarArticulo(int idArticulo)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();

            // Ejecutar el procedimiento almacenado para eliminar un artículo
            await connection.ExecuteAsync(
               "sp_Articulos_CRUD",
               new
               {
                   Operacion = 'D',
                   ArticuloID = idArticulo,
                   NombreArticulo = "",
                   Descripcion = "",
                   Precio = 0,
                   Stock = 0
               },
               commandType: CommandType.StoredProcedure
           );

            return true;
        }
    }
}
