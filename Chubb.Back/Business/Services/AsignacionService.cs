using Business.Interfaces;
using Data.Interfaces;
using Models.DTOs;
using Models.Responses;

namespace Business.Services
{
    public class AsignacionService : IAsignacionService
    {
        private readonly IAseguradoSeguroRepository _aseguradoSeguroRepository;
        private readonly IAseguradoRepository _aseguradoRepository;
        private readonly ISeguroRepository _seguroRepository;

        public AsignacionService(
            IAseguradoSeguroRepository aseguradoSeguroRepository,
            IAseguradoRepository aseguradoRepository,
            ISeguroRepository seguroRepository)
        {
            _aseguradoSeguroRepository = aseguradoSeguroRepository;
            _aseguradoRepository = aseguradoRepository;
            _seguroRepository = seguroRepository;
        }

        public async Task<ApiResponse<AsignacionDetalleDto>> AsignarSeguroAsync(AsignacionDto dto)
        {
            try
            {
                var asegurado = await _aseguradoRepository.ObtenerPorIdAsync(dto.AseguradoId);
                if (asegurado == null)
                {
                    return ApiResponse<AsignacionDetalleDto>.ErrorResponse("Asegurado no encontrado");
                }

                var seguro = await _seguroRepository.ObtenerPorIdAsync(dto.SeguroId);
                if (seguro == null)
                {
                    return ApiResponse<AsignacionDetalleDto>.ErrorResponse("Seguro no encontrado");
                }

                if (await _aseguradoSeguroRepository.ExisteAsignacionAsync(dto.AseguradoId, dto.SeguroId))
                {
                    return ApiResponse<AsignacionDetalleDto>.ErrorResponse("La asignación ya existe");
                }

                var asignacionId = await _aseguradoSeguroRepository.AsignarSeguroAsync(dto.AseguradoId, dto.SeguroId);

                var resultado = new AsignacionDetalleDto
                {
                    AseguradoSeguroId = asignacionId,
                    AseguradoId = asegurado.AseguradoId,
                    CedulaAsegurado = asegurado.Cedula,
                    NombreAsegurado = asegurado.NombreCompleto,
                    SeguroId = seguro.SeguroId,
                    CodigoSeguro = seguro.CodigoSeguro,
                    NombreSeguro = seguro.NombreSeguro,
                    FechaAsignacion = DateTime.Now
                };

                return ApiResponse<AsignacionDetalleDto>.SuccessResponse(resultado, "Seguro asignado exitosamente");
            }
            catch (Exception ex)
            {
                return ApiResponse<AsignacionDetalleDto>.ErrorResponse($"Error al asignar el seguro: {ex.Message}");
            }
        }

        public async Task<ApiResponse<ConsultaPorCedulaDto>> ConsultarPorCedulaAsync(string cedula)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(cedula))
                {
                    return ApiResponse<ConsultaPorCedulaDto>.ErrorResponse("La cédula es requerida");
                }

                var resultado = await _aseguradoSeguroRepository.ConsultarPorCedulaAsync(cedula);
                if (resultado == null)
                {
                    return ApiResponse<ConsultaPorCedulaDto>.ErrorResponse("No se encontró información para la cédula proporcionada");
                }

                return ApiResponse<ConsultaPorCedulaDto>.SuccessResponse(resultado, "Consulta realizada exitosamente");
            }
            catch (Exception ex)
            {
                return ApiResponse<ConsultaPorCedulaDto>.ErrorResponse($"Error al consultar: {ex.Message}");
            }
        }

        public async Task<ApiResponse<ConsultaPorCodigoSeguroDto>> ConsultarPorCodigoSeguroAsync(string codigoSeguro)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(codigoSeguro))
                {
                    return ApiResponse<ConsultaPorCodigoSeguroDto>.ErrorResponse("El código de seguro es requerido");
                }

                var resultado = await _aseguradoSeguroRepository.ConsultarPorCodigoSeguroAsync(codigoSeguro);
                if (resultado == null)
                {
                    return ApiResponse<ConsultaPorCodigoSeguroDto>.ErrorResponse("No se encontró información para el código de seguro proporcionado");
                }

                return ApiResponse<ConsultaPorCodigoSeguroDto>.SuccessResponse(resultado, "Consulta realizada exitosamente");
            }
            catch (Exception ex)
            {
                return ApiResponse<ConsultaPorCodigoSeguroDto>.ErrorResponse($"Error al consultar: {ex.Message}");
            }
        }
    }
}
