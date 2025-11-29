using Models.DTOs;
using Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface ICargaMasivaService
    {
        Task<ApiResponse<ResultadoCargaMasivaDto>> ProcesarCargaMasivaAsync(List<AseguradoCargaDto> asegurados, string nombreArchivo);
        Task<List<AseguradoCargaDto>> LeerArchivoTextoAsync(Stream fileStream);
    }
}
