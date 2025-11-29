using Models.DTOs;
using Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IAsignacionService
    {
        Task<ApiResponse<AsignacionDetalleDto>> AsignarSeguroAsync(AsignacionDto dto);
        Task<ApiResponse<ConsultaPorCedulaDto>> ConsultarPorCedulaAsync(string cedula);
        Task<ApiResponse<ConsultaPorCodigoSeguroDto>> ConsultarPorCodigoSeguroAsync(string codigoSeguro);
    }
}
