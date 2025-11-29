using Business.Interfaces;
using Business.Validators;
using Data.Interfaces;
using Models.DTOs;
using Models.Entities;
using Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class SeguroService : ISeguroService
    {
        private readonly ISeguroRepository _seguroRepository;
        private readonly SeguroValidator _validator;

        public SeguroService(ISeguroRepository seguroRepository)
        {
            _seguroRepository = seguroRepository;
            _validator = new SeguroValidator();
        }

        public async Task<ApiResponse<IEnumerable<SeguroDto>>> ObtenerTodosAsync()
        {
            try
            {
                var seguros = await _seguroRepository.ObtenerTodosAsync();
                var segurosDto = seguros.Select(s => new SeguroDto
                {
                    SeguroId = s.SeguroId,
                    CodigoSeguro = s.CodigoSeguro,
                    NombreSeguro = s.NombreSeguro,
                    SumaAsegurada = s.SumaAsegurada,
                    Prima = s.Prima
                });

                return ApiResponse<IEnumerable<SeguroDto>>.SuccessResponse(segurosDto, "Seguros obtenidos exitosamente");
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<SeguroDto>>.ErrorResponse($"Error al obtener seguros: {ex.Message}");
            }
        }

        public async Task<ApiResponse<SeguroDto>> ObtenerPorIdAsync(int seguroId)
        {
            try
            {
                var seguro = await _seguroRepository.ObtenerPorIdAsync(seguroId);
                if (seguro == null)
                {
                    return ApiResponse<SeguroDto>.ErrorResponse("Seguro no encontrado");
                }

                var seguroDto = new SeguroDto
                {
                    SeguroId = seguro.SeguroId,
                    CodigoSeguro = seguro.CodigoSeguro,
                    NombreSeguro = seguro.NombreSeguro,
                    SumaAsegurada = seguro.SumaAsegurada,
                    Prima = seguro.Prima
                };

                return ApiResponse<SeguroDto>.SuccessResponse(seguroDto);
            }
            catch (Exception ex)
            {
                return ApiResponse<SeguroDto>.ErrorResponse($"Error al obtener el seguro: {ex.Message}");
            }
        }

        public async Task<ApiResponse<SeguroDto>> CrearAsync(CrearSeguroDto dto)
        {
            try
            {
                var validacion = _validator.ValidarCreacion(dto);
                if (!validacion.IsValid)
                {
                    return ApiResponse<SeguroDto>.ErrorResponse(validacion.GetErrorMessages());
                }

                if (await _seguroRepository.ExisteCodigoAsync(dto.CodigoSeguro))
                {
                    return ApiResponse<SeguroDto>.ErrorResponse("El código de seguro ya existe");
                }

                var seguro = new Seguro
                {
                    CodigoSeguro = dto.CodigoSeguro,
                    NombreSeguro = dto.NombreSeguro,
                    SumaAsegurada = dto.SumaAsegurada,
                    Prima = dto.Prima
                };

                var seguroId = await _seguroRepository.CrearAsync(seguro);
                seguro.SeguroId = seguroId;

                var seguroDto = new SeguroDto
                {
                    SeguroId = seguro.SeguroId,
                    CodigoSeguro = seguro.CodigoSeguro,
                    NombreSeguro = seguro.NombreSeguro,
                    SumaAsegurada = seguro.SumaAsegurada,
                    Prima = seguro.Prima
                };

                return ApiResponse<SeguroDto>.SuccessResponse(seguroDto, "Seguro creado exitosamente");
            }
            catch (Exception ex)
            {
                return ApiResponse<SeguroDto>.ErrorResponse($"Error al crear el seguro: {ex.Message}");
            }
        }

        public async Task<ApiResponse<SeguroDto>> ActualizarAsync(ActualizarSeguroDto dto)
        {
            try
            {
                var validacion = _validator.ValidarActualizacion(dto);
                if (!validacion.IsValid)
                {
                    return ApiResponse<SeguroDto>.ErrorResponse(validacion.GetErrorMessages());
                }

                var seguroExistente = await _seguroRepository.ObtenerPorIdAsync(dto.SeguroId);
                if (seguroExistente == null)
                {
                    return ApiResponse<SeguroDto>.ErrorResponse("Seguro no encontrado");
                }

                if (await _seguroRepository.ExisteCodigoAsync(dto.CodigoSeguro, dto.SeguroId))
                {
                    return ApiResponse<SeguroDto>.ErrorResponse("El código de seguro ya existe en otro registro");
                }

                var seguro = new Seguro
                {
                    SeguroId = dto.SeguroId,
                    CodigoSeguro = dto.CodigoSeguro,
                    NombreSeguro = dto.NombreSeguro,
                    SumaAsegurada = dto.SumaAsegurada,
                    Prima = dto.Prima
                };

                await _seguroRepository.ActualizarAsync(seguro);

                var seguroDto = new SeguroDto
                {
                    SeguroId = seguro.SeguroId,
                    CodigoSeguro = seguro.CodigoSeguro,
                    NombreSeguro = seguro.NombreSeguro,
                    SumaAsegurada = seguro.SumaAsegurada,
                    Prima = seguro.Prima
                };

                return ApiResponse<SeguroDto>.SuccessResponse(seguroDto, "Seguro actualizado exitosamente");
            }
            catch (Exception ex)
            {
                return ApiResponse<SeguroDto>.ErrorResponse($"Error al actualizar el seguro: {ex.Message}");
            }
        }

        public async Task<ApiResponse> EliminarAsync(int seguroId)
        {
            try
            {
                var seguro = await _seguroRepository.ObtenerPorIdAsync(seguroId);
                if (seguro == null)
                {
                    return new ApiResponse
                    {
                        Success = false,
                        Message = "Seguro no encontrado",
                        Errors = new List<string> { "Seguro no encontrado" }
                    };
                }

                await _seguroRepository.EliminarAsync(seguroId);
                return new ApiResponse
                {
                    Success = true,
                    Message = "Seguro eliminado exitosamente"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse
                {
                    Success = false,
                    Message = "Error al eliminar el seguro",
                    Errors = new List<string> { ex.Message }
                };
            }
        }


    }
}
