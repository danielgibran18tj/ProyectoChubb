using Data.Interfaces;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationTests
{
    public class FakeAseguradoRepository : IAseguradoRepository
    {

        private static int _id = 1;

        public Task<bool> ActualizarAsync(Asegurado asegurado)
        {
            throw new NotImplementedException();
        }

        public Task<int> CrearAsync(Asegurado asegurado)
        {
            return Task.FromResult(_id++);
        }

        public Task<int> CrearMasivoAsync(List<Asegurado> asegurados)
        {
            throw new NotImplementedException();
        }

        public Task<bool> EliminarAsync(int aseguradoId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExisteCedulaAsync(string cedula, int? aseguradoIdExcluir = null)
        {
            return Task.FromResult(false);
        }

        public Task<Asegurado?> ObtenerPorCedulaAsync(string cedula)
        {
            throw new NotImplementedException();
        }

        public Task<Asegurado?> ObtenerPorIdAsync(int aseguradoId)
        {
            var resp = new Asegurado
            {
                AseguradoId = 1,
                Cedula = "0102030405",
                NombreCompleto = "Juan Pérez",
                Telefono = "0987654321",
                Edad = 30,
                FechaCreacion = DateTime.Now,
                FechaModificacion = DateTime.Now,
                Activo = true

            };
            return Task.FromResult(resp);
        }


        public Task<IEnumerable<Asegurado>> ObtenerTodosAsync()
        {
            throw new NotImplementedException();
        }
    }
}
