using Business.Validators;
using Models.DTOs;

namespace UnitTests
{
    public class AseguradoValidatorTests
    {
        private readonly AseguradoValidator _validator;

        public AseguradoValidatorTests()
        {
            _validator = new AseguradoValidator();
        }

        private CrearAseguradoDto CrearDtoValido()
        {
            return new CrearAseguradoDto
            {
                Cedula = "0102030405",
                NombreCompleto = "Juan Pérez",
                Telefono = "0987654321",
                Edad = 30
            };
        }

        [Fact]
        public void ValidarCreacion_DatosValidos_NoDebeRetornarErrores()
        {
            var dto = CrearDtoValido();

            var resultado = _validator.ValidarCreacion(dto);

            Assert.True(resultado.IsValid);

        }

        [Fact]
        public void ValidarCreacion_CedulaVacia_DeberiaRetornarError()
        {
            var dto = CrearDtoValido();
            dto.Cedula = "";

            var resultado = _validator.ValidarCreacion(dto);

            Assert.False(resultado.IsValid);
            Assert.Contains("La cédula es obligatoria", resultado.Errors.First().Message);
            Assert.Contains("Cedula", resultado.Errors.First().Field);
        }
    }
}
