using Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CargaMasivaController : ControllerBase
    {
        private readonly ICargaMasivaService _cargaMasivaService;

        public CargaMasivaController(ICargaMasivaService cargaMasivaService)
        {
            _cargaMasivaService = cargaMasivaService;
        }

        /// Procesa un archivo de carga masiva de asegurados (.txt)
        /// Formato esperado: Cedula|NombreCompleto|Telefono|Edad
        [HttpPost("subir-archivo")]
        public async Task<IActionResult> SubirArchivo(IFormFile archivo)
        {
            if (archivo == null || archivo.Length == 0)
            {
                return BadRequest(new { Message = "No se proporcionó ningún archivo" });
            }

            // Valida extensión
            var extension = Path.GetExtension(archivo.FileName).ToLower();

            try
            {
                if (extension == ".txt")
                {
                    using var stream = archivo.OpenReadStream();
                    var response = await _cargaMasivaService.LeerArchivoTextoAsync(stream, archivo.FileName);

                    return response.Success ? Ok(response) : BadRequest(response);
                }
                else if (extension == ".xlsx" || extension == ".xls")
                {
                    using var stream = archivo.OpenReadStream();
                    var response = await _cargaMasivaService.LeerArchivoExcelAsync(stream, archivo.FileName);

                    return response.Success ? Ok(response) : BadRequest(response);
                }
                else
                {
                    return BadRequest(new { Message = "Solo se permiten archivos .txt" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"Error al procesar el archivo: {ex.Message}" });
            }
        }


    }
}
