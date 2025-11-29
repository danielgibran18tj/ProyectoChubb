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

        /// <summary>
        /// Procesa un archivo de carga masiva de asegurados (.txt)
        /// Formato esperado: Cedula|NombreCompleto|Telefono|Edad
        /// </summary>
        [HttpPost("subir-archivo")]
        public async Task<IActionResult> SubirArchivo(IFormFile archivo)
        {
            if (archivo == null || archivo.Length == 0)
            {
                return BadRequest(new { Message = "No se proporcionó ningún archivo" });
            }

            // Validar extensión
            var extension = Path.GetExtension(archivo.FileName).ToLower();
            if (extension != ".txt")
            {
                return BadRequest(new { Message = "Solo se permiten archivos .txt" });
            }

            try
            {
                using var stream = archivo.OpenReadStream();
                var asegurados = await _cargaMasivaService.LeerArchivoTextoAsync(stream);

                if (!asegurados.Any())
                {
                    return BadRequest(new { Message = "El archivo no contiene registros válidos" });
                }

                var response = await _cargaMasivaService.ProcesarCargaMasivaAsync(asegurados, archivo.FileName);
                return response.Success ? Ok(response) : BadRequest(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"Error al procesar el archivo: {ex.Message}" });
            }
        }
    }
}
