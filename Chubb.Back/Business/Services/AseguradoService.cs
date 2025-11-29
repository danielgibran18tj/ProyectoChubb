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
    public class AseguradoService : IAseguradoService
    {
        private readonly IAseguradoRepository _aseguradoRepository;
        private readonly AseguradoValidator _validator;

        public AseguradoService(IAseguradoRepository aseguradoRepository)
        {
            _aseguradoRepository = aseguradoRepository;
            _validator = new AseguradoValidator();
        }

        public async Task<ApiResponse<IEnumerable<AseguradoDto>>> ObtenerTodosAsync()
        {
            try
            {
                var asegurados = await _aseguradoRepository.ObtenerTodosAsync();
                var aseguradosDto = asegurados.Select(a => new AseguradoDto
                {
                    AseguradoId = a.AseguradoId,
                    Cedula = a.Cedula,
                    NombreCompleto = a.NombreCompleto,
                    Telefono = a.Telefono,
                    Edad = a.Edad
                });

                return ApiResponse<IEnumerable<AseguradoDto>>.SuccessResponse(aseguradosDto, "Asegurados obtenidos exitosamente");
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<AseguradoDto>>.ErrorResponse($"Error al obtener asegurados: {ex.Message}");
            }
        }

        public async Task<ApiResponse<AseguradoDto>> ObtenerPorIdAsync(int aseguradoId)
        {
            try
            {
                var asegurado = await _aseguradoRepository.ObtenerPorIdAsync(aseguradoId);
                if (asegurado == null)
                {
                    return ApiResponse<AseguradoDto>.ErrorResponse("Asegurado no encontrado");
                }

                var aseguradoDto = new AseguradoDto
                {
                    AseguradoId = asegurado.AseguradoId,
                    Cedula = asegurado.Cedula,
                    NombreCompleto = asegurado.NombreCompleto,
                    Telefono = asegurado.Telefono,
                    Edad = asegurado.Edad
                };

                return ApiResponse<AseguradoDto>.SuccessResponse(aseguradoDto);
            }
            catch (Exception ex)
            {
                return ApiResponse<AseguradoDto>.ErrorResponse($"Error al obtener el asegurado: {ex.Message}");
            }
        }

        public async Task<ApiResponse<AseguradoDto>> CrearAsync(CrearAseguradoDto dto)
        {
            try
            {
                var validacion = _validator.ValidarCreacion(dto);
                if (!validacion.IsValid)
                {
                    return ApiResponse<AseguradoDto>.ErrorResponse(validacion.GetErrorMessages());
                }

                if (await _aseguradoRepository.ExisteCedulaAsync(dto.Cedula))
                {
                    return ApiResponse<AseguradoDto>.ErrorResponse("La cédula ya está registrada");
                }

                var asegurado = new Asegurado
                {
                    Cedula = dto.Cedula,
                    NombreCompleto = dto.NombreCompleto,
                    Telefono = dto.Telefono,
                    Edad = dto.Edad
                };

                var aseguradoId = await _aseguradoRepository.CrearAsync(asegurado);
                asegurado.AseguradoId = aseguradoId;

                var aseguradoDto = new AseguradoDto
                {
                    AseguradoId = asegurado.AseguradoId,
                    Cedula = asegurado.Cedula,
                    NombreCompleto = asegurado.NombreCompleto,
                    Telefono = asegurado.Telefono,
                    Edad = asegurado.Edad
                };

                return ApiResponse<AseguradoDto>.SuccessResponse(aseguradoDto, "Asegurado creado exitosamente");
            }
            catch (Exception ex)
            {
                return ApiResponse<AseguradoDto>.ErrorResponse($"Error al crear el asegurado: {ex.Message}");
            }
        }

        public async Task<ApiResponse<AseguradoDto>> ActualizarAsync(ActualizarAseguradoDto dto)
        {
            try
            {
                var validacion = _validator.ValidarActualizacion(dto);
                if (!validacion.IsValid)
                {
                    return ApiResponse<AseguradoDto>.ErrorResponse(validacion.GetErrorMessages());
                }

                var aseguradoExistente = await _aseguradoRepository.ObtenerPorIdAsync(dto.AseguradoId);
                if (aseguradoExistente == null)
                {
                    return ApiResponse<AseguradoDto>.ErrorResponse("Asegurado no encontrado");
                }

                if (await _aseguradoRepository.ExisteCedulaAsync(dto.Cedula, dto.AseguradoId))
                {
                    return ApiResponse<AseguradoDto>.ErrorResponse("La cédula ya está registrada en otro asegurado");
                }

                var asegurado = new Asegurado
                {
                    AseguradoId = dto.AseguradoId,
                    Cedula = dto.Cedula,
                    NombreCompleto = dto.NombreCompleto,
                    Telefono = dto.Telefono,
                    Edad = dto.Edad
                };

                await _aseguradoRepository.ActualizarAsync(asegurado);

                var aseguradoDto = new AseguradoDto
                {
                    AseguradoId = asegurado.AseguradoId,
                    Cedula = asegurado.Cedula,
                    NombreCompleto = asegurado.NombreCompleto,
                    Telefono = asegurado.Telefono,
                    Edad = asegurado.Edad
                };

                return ApiResponse<AseguradoDto>.SuccessResponse(aseguradoDto, "Asegurado actualizado exitosamente");
            }
            catch (Exception ex)
            {
                return ApiResponse<AseguradoDto>.ErrorResponse($"Error al actualizar el asegurado: {ex.Message}");
            }
        }

        public async Task<ApiResponse> EliminarAsync(int aseguradoId)
        {
            try
            {
                var asegurado = await _aseguradoRepository.ObtenerPorIdAsync(aseguradoId);
                if (asegurado == null)
                {
                    return new ApiResponse
                    {
                        Success = false,
                        Message = "Asegurado no encontrado",
                        Errors = new List<string> { "Asegurado no encontrado" }
                    };
                }

                await _aseguradoRepository.EliminarAsync(aseguradoId);
                return new ApiResponse
                {
                    Success = true,
                    Message = "Asegurado eliminado exitosamente"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse
                {
                    Success = false,
                    Message = "Error al eliminar el asegurado",
                    Errors = new List<string> { ex.Message }
                };
            }
        }


    }
}
