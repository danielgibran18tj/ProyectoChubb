using Models.DTOs;
using Models.Responses;

namespace Business.Validators
{
    public class SeguroValidator
    {
        public ValidationResponse ValidarCreacion(CrearSeguroDto dto)
        {
            var validacion = new ValidationResponse();

            if (string.IsNullOrWhiteSpace(dto.CodigoSeguro))
                validacion.AddError("CodigoSeguro", "El código del seguro es obligatorio");
            else if (dto.CodigoSeguro.Length > 50)
                validacion.AddError("CodigoSeguro", "El código no puede exceder 50 caracteres");

            if (string.IsNullOrWhiteSpace(dto.NombreSeguro))
                validacion.AddError("NombreSeguro", "El nombre del seguro es obligatorio");
            else if (dto.NombreSeguro.Length > 200)
                validacion.AddError("NombreSeguro", "El nombre no puede exceder 200 caracteres");

            if (dto.SumaAsegurada <= 0)
                validacion.AddError("SumaAsegurada", "La suma asegurada debe ser mayor a 0");

            if (dto.Prima <= 0)
                validacion.AddError("Prima", "La prima debe ser mayor a 0");

            if (dto.Prima > dto.SumaAsegurada)
                validacion.AddError("Prima", "La prima no puede ser mayor a la suma asegurada");

            return validacion;
        }

        public ValidationResponse ValidarActualizacion(ActualizarSeguroDto dto)
        {
            var validacion = new ValidationResponse();

            if (dto.SeguroId <= 0)
                validacion.AddError("SeguroId", "El ID del seguro es inválido");

            if (string.IsNullOrWhiteSpace(dto.CodigoSeguro))
                validacion.AddError("CodigoSeguro", "El código del seguro es obligatorio");
            else if (dto.CodigoSeguro.Length > 50)
                validacion.AddError("CodigoSeguro", "El código no puede exceder 50 caracteres");

            if (string.IsNullOrWhiteSpace(dto.NombreSeguro))
                validacion.AddError("NombreSeguro", "El nombre del seguro es obligatorio");
            else if (dto.NombreSeguro.Length > 200)
                validacion.AddError("NombreSeguro", "El nombre no puede exceder 200 caracteres");

            if (dto.SumaAsegurada <= 0)
                validacion.AddError("SumaAsegurada", "La suma asegurada debe ser mayor a 0");

            if (dto.Prima <= 0)
                validacion.AddError("Prima", "La prima debe ser mayor a 0");

            if (dto.Prima > dto.SumaAsegurada)
                validacion.AddError("Prima", "La prima no puede ser mayor a la suma asegurada");

            return validacion;
        }
    }
}
