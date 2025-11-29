using Models.Entities;

namespace Data.Interfaces
{
    public interface ISeguroRepository
    {
        Task<IEnumerable<Seguro>> ObtenerTodosAsync();
        Task<Seguro?> ObtenerPorIdAsync(int seguroId);
        Task<Seguro?> ObtenerPorCodigoAsync(string codigoSeguro);
        Task<int> CrearAsync(Seguro seguro);
        Task<bool> ActualizarAsync(Seguro seguro);
        Task<bool> EliminarAsync(int seguroId);
        Task<bool> ExisteCodigoAsync(string codigoSeguro, int? seguroIdExcluir = null);
    }
}
