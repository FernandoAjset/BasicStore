using Dapper;
using Microsoft.Data.SqlClient;
using ProyectoPrimerParcial.Models;
using System.Security.Cryptography;
using System.Text;

namespace ProyectoPrimerParcial.Servicios
{
    public interface IUsuarioService
    {
        public Task<bool> AutenticarUsuario(Usuario usuario);
        public Task<Usuario> ObtenerUsuario(int UsuarioId);
        public Task<bool> CrearUsuario(Usuario usuario);
        public Task<bool> ActualizarUsuario(Usuario usuario);
        public Task<bool> EliminarUsuario(int UsuarioId);
        Task<Usuario> ObtenerUsuarioPorNombre(string nombreUsuario);
    }

    public class UsuarioServicioWithDapper : IUsuarioService
    {
        private readonly string connectionString;

        public UsuarioServicioWithDapper(
            IConfiguration configuration
            )
        {
            this.connectionString = configuration.GetConnectionString("Default");
        }

        public async Task<bool> CrearUsuario(Usuario usuario)
        {
            // Cifrar la contraseña utilizando el algoritmo SHA256
            byte[] passwordHash = GenerateHash(usuario.Contraseña);
            string passwordBase64 = Convert.ToBase64String(passwordHash);
            // Guardar el usuario
            using var connection = new SqlConnection(connectionString);
            var parameters = new { Operacion = "C", UsuarioId = 0, usuario.NombreUsuario, Contraseña = passwordBase64 };

            int usrId = await connection.ExecuteAsync("sp_Usuarios_CRUD", parameters, commandType: System.Data.CommandType.StoredProcedure);

            return usrId > 0;
        }

        public async Task<bool> AutenticarUsuario(Usuario usuario)
        {
            using var connection = new SqlConnection(connectionString);
            var parameters = new { Operacion = "A", UsuarioId = 0, usuario.NombreUsuario, usuario.Contraseña };

            var usuarios = await connection.QueryAsync<Usuario>("sp_Usuarios_CRUD", parameters, commandType: System.Data.CommandType.StoredProcedure);

            var usuarioEncontrado = usuarios.FirstOrDefault();

            if (usuarioEncontrado != null)
            {
                // Cifrar la contraseña enviada desde el frontend utilizando.
                byte[] passwordHashToCheck = GenerateHash(usuario.Contraseña);
                // Comparar los dos hashes cifrados
                bool autenticado = CompareHashes(passwordHashToCheck, Convert.FromBase64String(usuarioEncontrado.Contraseña));
                return autenticado;
            }
            return false;
        }

        public async Task<bool> ActualizarUsuario(Usuario usuario)
        {
            try
            {
                using var connection = new SqlConnection(connectionString);
                var parameters = new { Operacion = "U", usuario.UsuarioID, usuario.NombreUsuario, usuario.Contraseña };

                await connection.ExecuteAsync("sp_Usuarios_CRUD", parameters, commandType: System.Data.CommandType.StoredProcedure);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al actualizar el usuario: " + ex.Message);
                return false;
            }
        }

        public async Task<bool> EliminarUsuario(int UsuarioId)
        {
            try
            {
                using var connection = new SqlConnection(connectionString);
                var parameters = new { Operacion = "D", UsuarioID = UsuarioId };

                await connection.ExecuteAsync("sp_Usuarios_CRUD", parameters, commandType: System.Data.CommandType.StoredProcedure);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al eliminar el usuario: " + ex.Message);
                return false;
            }
        }

        public async Task<Usuario> ObtenerUsuario(int UsuarioId)
        {
            try
            {
                using var connection = new SqlConnection(connectionString);
                var parameters = new { Operacion = "R", UsuarioID = UsuarioId, NombreUsuario = (string)null, Contraseña = (string)null };

                return await connection.QueryFirstOrDefaultAsync<Usuario>("sp_Usuarios_CRUD", parameters, commandType: System.Data.CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al obtener el usuario: " + ex.Message);
                return null;
            }
        }

        public async Task<Usuario> ObtenerUsuarioPorNombre(string nombreUsuario)
        {
            using var connection = new SqlConnection(connectionString);
            var parameters = new { Operacion = "A", UsuarioID = 0, NombreUsuario = nombreUsuario, Contraseña = (string)null };

            return await connection.QueryFirstOrDefaultAsync<Usuario>("sp_Usuarios_CRUD", parameters, commandType: System.Data.CommandType.StoredProcedure);
        }

        // MÉTODOS ADICIONALES DEL SERVICIO PARA MANEJO DE CIFRADO DE CONTRASEÑA
        // Método para generar el hash de la contraseña usando el algoritmo SHA256 y el salt proporcionado
        private byte[] GenerateHash(string contraseña)
        {
            using var sha256 = SHA256.Create();
            // Concatenar la contraseña y el salt
            byte[] passwordBytes = Encoding.UTF8.GetBytes(contraseña);
            byte[] passwordWithSaltBytes = new byte[passwordBytes.Length];
            passwordBytes.CopyTo(passwordWithSaltBytes, 0);
            // Generar el hash utilizando SHA256
            byte[] hash = sha256.ComputeHash(passwordWithSaltBytes);

            return hash;
        }

        // Método para comparar dos hashes
        private static bool CompareHashes(byte[] hash1, byte[] hash2)
        {
            if (hash1.Length != hash2.Length)
                return false;

            for (int i = 0; i < hash1.Length; i++)
            {
                if (hash1[i] != hash2[i])
                    return false;
            }

            return true;
        }
    }
}
