using Models.DTOs;
using Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface ISeguroService
    {
        Task<ApiResponse<IEnumerable<SeguroDto>>> ObtenerTodosAsync();
        Task<ApiResponse<SeguroDto>> ObtenerPorIdAsync(int seguroId);
        Task<ApiResponse<SeguroDto>> CrearAsync(CrearSeguroDto dto);
        Task<ApiResponse<SeguroDto>> ActualizarAsync(ActualizarSeguroDto dto);
        Task<ApiResponse> EliminarAsync(int seguroId);
    }
}
