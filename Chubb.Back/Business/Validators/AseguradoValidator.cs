using Models.DTOs;
using Models.Responses;
using System.Text.RegularExpressions;

namespace Business.Validators
{
    public class AseguradoValidator
    {
        public ValidationResponse ValidarCreacion(CrearAseguradoDto dto)
        {
            var validacion = new ValidationResponse();

            if (string.IsNullOrWhiteSpace(dto.Cedula))
                validacion.AddError("Cedula", "La cédula es obligatoria");
            else if (!EsCedulaValida(dto.Cedula))
                validacion.AddError("Cedula", "La cédula tiene un formato inválido (debe contener 10 dígitos)");

            if (string.IsNullOrWhiteSpace(dto.NombreCompleto))
                validacion.AddError("NombreCompleto", "El nombre completo es obligatorio");
            else if (dto.NombreCompleto.Length > 200)
                validacion.AddError("NombreCompleto", "El nombre no puede exceder 200 caracteres");

            if (string.IsNullOrWhiteSpace(dto.Telefono))
                validacion.AddError("Telefono", "El teléfono es obligatorio");
            else if (!EsTelefonoValido(dto.Telefono))
                validacion.AddError("Telefono", "El teléfono tiene un formato inválido");

            if (dto.Edad < 0 || dto.Edad > 120)
                validacion.AddError("Edad", "La edad debe estar entre 0 y 120 años");

            return validacion;
        }

        public ValidationResponse ValidarActualizacion(ActualizarAseguradoDto dto)
        {
            var validacion = new ValidationResponse();

            if (dto.AseguradoId <= 0)
                validacion.AddError("AseguradoId", "El ID del asegurado es inválido");

            if (string.IsNullOrWhiteSpace(dto.Cedula))
                validacion.AddError("Cedula", "La cédula es obligatoria");
            else if (!EsCedulaValida(dto.Cedula))
                validacion.AddError("Cedula", "La cédula tiene un formato inválido (debe contener 10 dígitos)");

            if (string.IsNullOrWhiteSpace(dto.NombreCompleto))
                validacion.AddError("NombreCompleto", "El nombre completo es obligatorio");
            else if (dto.NombreCompleto.Length > 200)
                validacion.AddError("NombreCompleto", "El nombre no puede exceder 200 caracteres");

            if (string.IsNullOrWhiteSpace(dto.Telefono))
                validacion.AddError("Telefono", "El teléfono es obligatorio");
            else if (!EsTelefonoValido(dto.Telefono))
                validacion.AddError("Telefono", "El teléfono tiene un formato inválido");

            if (dto.Edad < 0 || dto.Edad > 120)
                validacion.AddError("Edad", "La edad debe estar entre 0 y 120 años");

            return validacion;
        }

        private bool EsCedulaValida(string cedula)
        {
            // Validación básica: 10 dígitos
            return Regex.IsMatch(cedula, @"^\d{10}$");
        }

        private bool EsTelefonoValido(string telefono)
        {
            // Validación básica: 10 dígitos (puede incluir +593)
            return Regex.IsMatch(telefono, @"^(\+593)?\d{9,10}$");
        }
    }
}
