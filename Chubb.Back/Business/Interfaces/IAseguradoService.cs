using Models.DTOs;
using Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IAseguradoService
    {
        Task<ApiResponse<IEnumerable<AseguradoDto>>> ObtenerTodosAsync();
        Task<ApiResponse<AseguradoDto>> ObtenerPorIdAsync(int aseguradoId);
        Task<ApiResponse<AseguradoDto>> CrearAsync(CrearAseguradoDto dto);
        Task<ApiResponse<AseguradoDto>> ActualizarAsync(ActualizarAseguradoDto dto);
        Task<ApiResponse> EliminarAsync(int aseguradoId);
    }
}
