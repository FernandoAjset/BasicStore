using Dapper;
using Microsoft.Data.SqlClient;
using ProyectoPrimerParcial.Models;
using System.Data;

namespace ProyectoPrimerParcial.Servicios
{
    /// <summary>
    /// Interfaz que define los métodos para el servicio de gestión de artículos.
    /// </summary>
    public interface IArticulosService
    {
        /// <summary>
        /// Obtiene un artículo por su ID.
        /// </summary>
        /// <param name="idArticulo">ID del artículo a obtener.</param>
        /// <returns>El artículo correspondiente al ID proporcionado.</returns>
        public Task<Articulo> ObtenerArticulo(int idArticulo);

        /// <summary>
        /// Obtiene todos los artículos.
        /// </summary>
        /// <returns>Una colección de todos los artículos.</returns>
        public Task<IEnumerable<Articulo>> ObtenerTodosArticulos();

        /// <summary>
        /// Crea un nuevo artículo.
        /// </summary>
        /// <param name="articulo">El artículo a crear.</param>
        /// <returns>Un valor booleano que indica si la operación de creación fue exitosa.</returns>
        public Task<bool> CrearArticulo(Articulo articulo);

        /// <summary>
        /// Actualiza un artículo existente.
        /// </summary>
        /// <param name="articulo">El artículo con los datos actualizados.</param>
        /// <returns>Un valor booleano que indica si la operación de actualización fue exitosa.</returns>
        public Task<bool> ActualizarArticulo(Articulo articulo);

        /// <summary>
        /// Elimina un artículo por su ID.
        /// </summary>
        /// <param name="idArticulo">ID del artículo a eliminar.</param>
        /// <returns>Un valor booleano que indica si la operación de eliminación fue exitosa.</returns>
        public Task<bool> EliminarArticulo(int idArticulo);
    }

    /// <summary>
    /// Implementación del servicio de gestión de artículos utilizando Dapper para interactuar con la base de datos.
    /// </summary>
    public class ArticulosServiceWithDapper : IArticulosService
    {
        private readonly string connectionString;

        /// <summary>
        /// Constructor del servicio que recibe la configuración de conexión a la base de datos.
        /// </summary>
        /// <param name="configuration">La configuración de conexión a la base de datos.</param>
        public ArticulosServiceWithDapper(IConfiguration configuration)
        {
            this.connectionString = configuration.GetConnectionString("Default");
        }

        /// <inheritdoc/>
        public async Task<Articulo> ObtenerArticulo(int idArticulo)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();

            // Ejecutar el procedimiento almacenado para obtener un artículo por su ID
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

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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
