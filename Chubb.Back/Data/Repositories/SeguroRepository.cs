using Data.Connection;
using Data.Interfaces;
using Microsoft.Data.SqlClient;
using Models.Entities;
using System.Data;

namespace Data.Repositories
{
    public class SeguroRepository : ISeguroRepository
    {
        private readonly SqlConnectionFactory _connectionFactory;

        public SeguroRepository(SqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<Seguro>> ObtenerTodosAsync()
        {
            var seguros = new List<Seguro>();

            using var connection = _connectionFactory.CreateConnection();
            using var command = new SqlCommand(
                "SELECT SeguroId, CodigoSeguro, NombreSeguro, SumaAsegurada, Prima, FechaCreacion, FechaModificacion, Activo " +
                "FROM Seguros WHERE Activo = 1 ORDER BY FechaCreacion DESC",
                connection);

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                seguros.Add(MapearSeguro(reader));
            }

            return seguros;
        }

        public async Task<Seguro?> ObtenerPorIdAsync(int seguroId)
        {
            using var connection = _connectionFactory.CreateConnection();
            using var command = new SqlCommand(
                "SELECT SeguroId, CodigoSeguro, NombreSeguro, SumaAsegurada, Prima, FechaCreacion, FechaModificacion, Activo " +
                "FROM Seguros WHERE SeguroId = @SeguroId AND Activo = 1",
                connection);

            command.Parameters.Add("@SeguroId", SqlDbType.Int).Value = seguroId;

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return MapearSeguro(reader);
            }

            return null;
        }

        public async Task<Seguro?> ObtenerPorCodigoAsync(string codigoSeguro)
        {
            using var connection = _connectionFactory.CreateConnection();
            using var command = new SqlCommand(
                "SELECT SeguroId, CodigoSeguro, NombreSeguro, SumaAsegurada, Prima, FechaCreacion, FechaModificacion, Activo " +
                "FROM Seguros WHERE CodigoSeguro = @CodigoSeguro AND Activo = 1",
                connection);

            command.Parameters.Add("@CodigoSeguro", SqlDbType.NVarChar, 50).Value = codigoSeguro;

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return MapearSeguro(reader);
            }

            return null;
        }

        public async Task<int> CrearAsync(Seguro seguro)
        {
            using var connection = _connectionFactory.CreateConnection();
            using var command = new SqlCommand("sp_CrearSeguro", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.Add("@CodigoSeguro", SqlDbType.NVarChar, 50).Value = seguro.CodigoSeguro;
            command.Parameters.Add("@NombreSeguro", SqlDbType.NVarChar, 200).Value = seguro.NombreSeguro;
            command.Parameters.Add("@SumaAsegurada", SqlDbType.Decimal).Value = seguro.SumaAsegurada;
            command.Parameters.Add("@Prima", SqlDbType.Decimal).Value = seguro.Prima;

            await connection.OpenAsync();
            var result = await command.ExecuteScalarAsync();
            return Convert.ToInt32(result);
        }

        public async Task<bool> ActualizarAsync(Seguro seguro)
        {
            using var connection = _connectionFactory.CreateConnection();
            using var command = new SqlCommand("sp_ActualizarSeguro", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.Add("@SeguroId", SqlDbType.Int).Value = seguro.SeguroId;
            command.Parameters.Add("@CodigoSeguro", SqlDbType.NVarChar, 50).Value = seguro.CodigoSeguro;
            command.Parameters.Add("@NombreSeguro", SqlDbType.NVarChar, 200).Value = seguro.NombreSeguro;
            command.Parameters.Add("@SumaAsegurada", SqlDbType.Decimal).Value = seguro.SumaAsegurada;
            command.Parameters.Add("@Prima", SqlDbType.Decimal).Value = seguro.Prima;

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
            return true;
        }

        public async Task<bool> EliminarAsync(int seguroId)
        {
            using var connection = _connectionFactory.CreateConnection();
            using var command = new SqlCommand("sp_EliminarSeguro", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.Add("@SeguroId", SqlDbType.Int).Value = seguroId;

            await connection.OpenAsync();
            var rowsAffected = await command.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }

        public async Task<bool> ExisteCodigoAsync(string codigoSeguro, int? seguroIdExcluir = null)
        {
            using var connection = _connectionFactory.CreateConnection();

            var query = "SELECT COUNT(1) FROM Seguros WHERE CodigoSeguro = @CodigoSeguro AND Activo = 1";
            if (seguroIdExcluir.HasValue)
            {
                query += " AND SeguroId != @SeguroIdExcluir";
            }

            using var command = new SqlCommand(query, connection);
            command.Parameters.Add("@CodigoSeguro", SqlDbType.NVarChar, 50).Value = codigoSeguro;

            if (seguroIdExcluir.HasValue)
            {
                command.Parameters.Add("@SeguroIdExcluir", SqlDbType.Int).Value = seguroIdExcluir.Value;
            }

            await connection.OpenAsync();
            var count = (int)await command.ExecuteScalarAsync();
            return count > 0;
        }

        private Seguro MapearSeguro(SqlDataReader reader)
        {
            return new Seguro
            {
                SeguroId = reader.GetInt32(reader.GetOrdinal("SeguroId")),
                CodigoSeguro = reader.GetString(reader.GetOrdinal("CodigoSeguro")),
                NombreSeguro = reader.GetString(reader.GetOrdinal("NombreSeguro")),
                SumaAsegurada = reader.GetDecimal(reader.GetOrdinal("SumaAsegurada")),
                Prima = reader.GetDecimal(reader.GetOrdinal("Prima")),
                FechaCreacion = reader.GetDateTime(reader.GetOrdinal("FechaCreacion")),
                FechaModificacion = reader.GetDateTime(reader.GetOrdinal("FechaModificacion")),
                Activo = reader.GetBoolean(reader.GetOrdinal("Activo"))
            };
        }

    }
}
