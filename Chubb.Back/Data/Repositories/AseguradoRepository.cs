using Data.Connection;
using Data.Interfaces;
using Microsoft.Data.SqlClient;
using Models.Entities;
using System.Data;

namespace Data.Repositories
{
    public class AseguradoRepository : IAseguradoRepository
    {
        private readonly SqlConnectionFactory _connectionFactory;

        public AseguradoRepository(SqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<Asegurado>> ObtenerTodosAsync()
        {
            var asegurados = new List<Asegurado>();

            using var connection = _connectionFactory.CreateConnection();
            using var command = new SqlCommand(
                "SELECT AseguradoId, Cedula, NombreCompleto, Telefono, Edad, FechaCreacion, FechaModificacion, Activo " +
                "FROM Asegurados WHERE Activo = 1 ORDER BY FechaCreacion DESC",
                connection);

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                asegurados.Add(MapearAsegurado(reader));
            }

            return asegurados;
        }

        public async Task<Asegurado?> ObtenerPorIdAsync(int aseguradoId)
        {
            using var connection = _connectionFactory.CreateConnection();
            using var command = new SqlCommand(
                "SELECT AseguradoId, Cedula, NombreCompleto, Telefono, Edad, FechaCreacion, FechaModificacion, Activo " +
                "FROM Asegurados WHERE AseguradoId = @AseguradoId AND Activo = 1",
                connection);

            command.Parameters.Add("@AseguradoId", SqlDbType.Int).Value = aseguradoId;

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return MapearAsegurado(reader);
            }

            return null;
        }

        public async Task<Asegurado?> ObtenerPorCedulaAsync(string cedula)
        {
            using var connection = _connectionFactory.CreateConnection();
            using var command = new SqlCommand(
                "SELECT AseguradoId, Cedula, NombreCompleto, Telefono, Edad, FechaCreacion, FechaModificacion, Activo " +
                "FROM Asegurados WHERE Cedula = @Cedula AND Activo = 1",
                connection);

            command.Parameters.Add("@Cedula", SqlDbType.NVarChar, 20).Value = cedula;

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return MapearAsegurado(reader);
            }

            return null;
        }

        public async Task<int> CrearAsync(Asegurado asegurado)
        {
            using var connection = _connectionFactory.CreateConnection();
            using var command = new SqlCommand("sp_CrearAsegurado", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.Add("@Cedula", SqlDbType.NVarChar, 20).Value = asegurado.Cedula;
            command.Parameters.Add("@NombreCompleto", SqlDbType.NVarChar, 200).Value = asegurado.NombreCompleto;
            command.Parameters.Add("@Telefono", SqlDbType.NVarChar, 20).Value = asegurado.Telefono;
            command.Parameters.Add("@Edad", SqlDbType.Int).Value = asegurado.Edad;

            await connection.OpenAsync();
            var result = await command.ExecuteScalarAsync();
            return Convert.ToInt32(result);
        }

        public async Task<bool> ActualizarAsync(Asegurado asegurado)
        {
            using var connection = _connectionFactory.CreateConnection();
            using var command = new SqlCommand("sp_ActualizarAsegurado", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.Add("@AseguradoId", SqlDbType.Int).Value = asegurado.AseguradoId;
            command.Parameters.Add("@Cedula", SqlDbType.NVarChar, 20).Value = asegurado.Cedula;
            command.Parameters.Add("@NombreCompleto", SqlDbType.NVarChar, 200).Value = asegurado.NombreCompleto;
            command.Parameters.Add("@Telefono", SqlDbType.NVarChar, 20).Value = asegurado.Telefono;
            command.Parameters.Add("@Edad", SqlDbType.Int).Value = asegurado.Edad;

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
            return true;
        }

        public async Task<bool> EliminarAsync(int aseguradoId)
        {
            using var connection = _connectionFactory.CreateConnection();
            using var command = new SqlCommand("sp_EliminarAsegurado", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.Add("@AseguradoId", SqlDbType.Int).Value = aseguradoId;

            await connection.OpenAsync();
            var rowsAffected = await command.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }

        public async Task<bool> ExisteCedulaAsync(string cedula, int? aseguradoIdExcluir = null)
        {
            using var connection = _connectionFactory.CreateConnection();

            var query = "SELECT COUNT(1) FROM Asegurados WHERE Cedula = @Cedula AND Activo = 1";
            if (aseguradoIdExcluir.HasValue)
            {
                query += " AND AseguradoId != @AseguradoIdExcluir";
            }

            using var command = new SqlCommand(query, connection);
            command.Parameters.Add("@Cedula", SqlDbType.NVarChar, 20).Value = cedula;

            if (aseguradoIdExcluir.HasValue)
            {
                command.Parameters.Add("@AseguradoIdExcluir", SqlDbType.Int).Value = aseguradoIdExcluir.Value;
            }

            await connection.OpenAsync();
            var count = (int)await command.ExecuteScalarAsync();
            return count > 0;
        }

        public async Task<int> CrearMasivoAsync(List<Asegurado> asegurados)
        {
            var registrosCreados = 0;

            using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();

            using var transaction = connection.BeginTransaction();
            try
            {
                foreach (var asegurado in asegurados)
                {
                    using var command = new SqlCommand("sp_CrearAsegurado", connection, transaction)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    command.Parameters.Add("@Cedula", SqlDbType.NVarChar, 20).Value = asegurado.Cedula;
                    command.Parameters.Add("@NombreCompleto", SqlDbType.NVarChar, 200).Value = asegurado.NombreCompleto;
                    command.Parameters.Add("@Telefono", SqlDbType.NVarChar, 20).Value = asegurado.Telefono;
                    command.Parameters.Add("@Edad", SqlDbType.Int).Value = asegurado.Edad;

                    await command.ExecuteNonQueryAsync();
                    registrosCreados++;
                }

                transaction.Commit();
                return registrosCreados;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        private Asegurado MapearAsegurado(SqlDataReader reader)
        {
            return new Asegurado
            {
                AseguradoId = reader.GetInt32(reader.GetOrdinal("AseguradoId")),
                Cedula = reader.GetString(reader.GetOrdinal("Cedula")),
                NombreCompleto = reader.GetString(reader.GetOrdinal("NombreCompleto")),
                Telefono = reader.GetString(reader.GetOrdinal("Telefono")),
                Edad = reader.GetInt32(reader.GetOrdinal("Edad")),
                FechaCreacion = reader.GetDateTime(reader.GetOrdinal("FechaCreacion")),
                FechaModificacion = reader.GetDateTime(reader.GetOrdinal("FechaModificacion")),
                Activo = reader.GetBoolean(reader.GetOrdinal("Activo"))
            };
        }
    }
}
