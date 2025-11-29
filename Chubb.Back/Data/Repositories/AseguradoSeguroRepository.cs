using Data.Connection;
using Data.Interfaces;
using Microsoft.Data.SqlClient;
using Models.DTOs;
using Models.Entities;
using System.Data;

namespace Data.Repositories
{
    public class AseguradoSeguroRepository : IAseguradoSeguroRepository
    {
        private readonly SqlConnectionFactory _connectionFactory;

        public AseguradoSeguroRepository(SqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<int> AsignarSeguroAsync(int aseguradoId, int seguroId)
        {
            using var connection = _connectionFactory.CreateConnection();
            using var command = new SqlCommand("sp_AsignarSeguroAsegurado", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.Add("@AseguradoId", SqlDbType.Int).Value = aseguradoId;
            command.Parameters.Add("@SeguroId", SqlDbType.Int).Value = seguroId;

            await connection.OpenAsync();
            var result = await command.ExecuteScalarAsync();
            return Convert.ToInt32(result);
        }

        public async Task<bool> ExisteAsignacionAsync(int aseguradoId, int seguroId)
        {
            using var connection = _connectionFactory.CreateConnection();
            using var command = new SqlCommand(
                "SELECT COUNT(1) FROM AseguradosSeguros " +
                "WHERE AseguradoId = @AseguradoId AND SeguroId = @SeguroId AND Activo = 1",
                connection);

            command.Parameters.Add("@AseguradoId", SqlDbType.Int).Value = aseguradoId;
            command.Parameters.Add("@SeguroId", SqlDbType.Int).Value = seguroId;

            await connection.OpenAsync();
            var count = (int)await command.ExecuteScalarAsync();
            return count > 0;
        }

        public async Task<ConsultaPorCedulaDto?> ConsultarPorCedulaAsync(string cedula)
        {
            using var connection = _connectionFactory.CreateConnection();
            using var command = new SqlCommand("sp_ConsultarSegurosPorCedula", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.Add("@Cedula", SqlDbType.NVarChar, 20).Value = cedula;

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            ConsultaPorCedulaDto? resultado = null;

            while (await reader.ReadAsync())
            {
                if (resultado == null)
                {
                    resultado = new ConsultaPorCedulaDto
                    {
                        Asegurado = new AseguradoDto
                        {
                            AseguradoId = reader.GetInt32(reader.GetOrdinal("AseguradoId")),
                            Cedula = reader.GetString(reader.GetOrdinal("Cedula")),
                            NombreCompleto = reader.GetString(reader.GetOrdinal("NombreCompleto")),
                            Telefono = reader.GetString(reader.GetOrdinal("Telefono")),
                            Edad = reader.GetInt32(reader.GetOrdinal("Edad"))
                        }
                    };
                }

                resultado.Seguros.Add(new SeguroAsignadoDto
                {
                    SeguroId = reader.GetInt32(reader.GetOrdinal("SeguroId")),
                    CodigoSeguro = reader.GetString(reader.GetOrdinal("CodigoSeguro")),
                    NombreSeguro = reader.GetString(reader.GetOrdinal("NombreSeguro")),
                    SumaAsegurada = reader.GetDecimal(reader.GetOrdinal("SumaAsegurada")),
                    Prima = reader.GetDecimal(reader.GetOrdinal("Prima")),
                    FechaAsignacion = reader.GetDateTime(reader.GetOrdinal("FechaAsignacion"))
                });
            }

            return resultado;
        }

        public async Task<ConsultaPorCodigoSeguroDto?> ConsultarPorCodigoSeguroAsync(string codigoSeguro)
        {
            using var connection = _connectionFactory.CreateConnection();
            using var command = new SqlCommand("sp_ConsultarAseguradosPorCodigoSeguro", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.Add("@CodigoSeguro", SqlDbType.NVarChar, 50).Value = codigoSeguro;

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            ConsultaPorCodigoSeguroDto? resultado = null;

            while (await reader.ReadAsync())
            {
                if (resultado == null)
                {
                    resultado = new ConsultaPorCodigoSeguroDto
                    {
                        Seguro = new SeguroDto
                        {
                            SeguroId = reader.GetInt32(reader.GetOrdinal("SeguroId")),
                            CodigoSeguro = reader.GetString(reader.GetOrdinal("CodigoSeguro")),
                            NombreSeguro = reader.GetString(reader.GetOrdinal("NombreSeguro")),
                            SumaAsegurada = reader.GetDecimal(reader.GetOrdinal("SumaAsegurada")),
                            Prima = reader.GetDecimal(reader.GetOrdinal("Prima"))
                        }
                    };
                }

                resultado.Asegurados.Add(new AseguradoAsignadoDto
                {
                    AseguradoId = reader.GetInt32(reader.GetOrdinal("AseguradoId")),
                    Cedula = reader.GetString(reader.GetOrdinal("Cedula")),
                    NombreCompleto = reader.GetString(reader.GetOrdinal("NombreCompleto")),
                    Telefono = reader.GetString(reader.GetOrdinal("Telefono")),
                    Edad = reader.GetInt32(reader.GetOrdinal("Edad")),
                    FechaAsignacion = reader.GetDateTime(reader.GetOrdinal("FechaAsignacion"))
                });
            }

            return resultado;
        }

        public async Task<IEnumerable<AseguradoSeguro>> ObtenerPorAseguradoAsync(int aseguradoId)
        {
            var asignaciones = new List<AseguradoSeguro>();

            using var connection = _connectionFactory.CreateConnection();
            using var command = new SqlCommand(
                "SELECT AseguradoSeguroId, AseguradoId, SeguroId, FechaAsignacion, Activo " +
                "FROM AseguradosSeguros WHERE AseguradoId = @AseguradoId AND Activo = 1",
                connection);

            command.Parameters.Add("@AseguradoId", SqlDbType.Int).Value = aseguradoId;

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                asignaciones.Add(MapearAseguradoSeguro(reader));
            }

            return asignaciones;
        }

        public async Task<IEnumerable<AseguradoSeguro>> ObtenerPorSeguroAsync(int seguroId)
        {
            var asignaciones = new List<AseguradoSeguro>();

            using var connection = _connectionFactory.CreateConnection();
            using var command = new SqlCommand(
                "SELECT AseguradoSeguroId, AseguradoId, SeguroId, FechaAsignacion, Activo " +
                "FROM AseguradosSeguros WHERE SeguroId = @SeguroId AND Activo = 1",
                connection);

            command.Parameters.Add("@SeguroId", SqlDbType.Int).Value = seguroId;

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                asignaciones.Add(MapearAseguradoSeguro(reader));
            }

            return asignaciones;
        }

        private AseguradoSeguro MapearAseguradoSeguro(SqlDataReader reader)
        {
            return new AseguradoSeguro
            {
                AseguradoSeguroId = reader.GetInt32(reader.GetOrdinal("AseguradoSeguroId")),
                AseguradoId = reader.GetInt32(reader.GetOrdinal("AseguradoId")),
                SeguroId = reader.GetInt32(reader.GetOrdinal("SeguroId")),
                FechaAsignacion = reader.GetDateTime(reader.GetOrdinal("FechaAsignacion")),
                Activo = reader.GetBoolean(reader.GetOrdinal("Activo"))
            };
        }

    }
}
