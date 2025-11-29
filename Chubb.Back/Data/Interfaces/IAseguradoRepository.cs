using Models.Entities;

namespace Data.Interfaces
{
    public interface IAseguradoRepository
    {
        Task<IEnumerable<Asegurado>> ObtenerTodosAsync();
        Task<Asegurado?> ObtenerPorIdAsync(int aseguradoId);
        Task<Asegurado?> ObtenerPorCedulaAsync(string cedula);
        Task<int> CrearAsync(Asegurado asegurado);
        Task<bool> ActualizarAsync(Asegurado asegurado);
        Task<bool> EliminarAsync(int aseguradoId);
        Task<bool> ExisteCedulaAsync(string cedula, int? aseguradoIdExcluir = null);
        Task<int> CrearMasivoAsync(List<Asegurado> asegurados);
    }
}
