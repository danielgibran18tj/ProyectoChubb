using Models.DTOs;
using Models.Entities;

namespace Data.Interfaces
{
    public interface IAseguradoSeguroRepository
    {
        Task<int> AsignarSeguroAsync(int aseguradoId, int seguroId);
        Task<bool> ExisteAsignacionAsync(int aseguradoId, int seguroId);
        Task<ConsultaPorCedulaDto?> ConsultarPorCedulaAsync(string cedula);
        Task<ConsultaPorCodigoSeguroDto?> ConsultarPorCodigoSeguroAsync(string codigoSeguro);
        Task<IEnumerable<AseguradoSeguro>> ObtenerPorAseguradoAsync(int aseguradoId);
        Task<IEnumerable<AseguradoSeguro>> ObtenerPorSeguroAsync(int seguroId);
    }
}
