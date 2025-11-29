using Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConsultasController : ControllerBase
    {
        private readonly IAsignacionService _asignacionService;

        public ConsultasController(IAsignacionService asignacionService)
        {
            _asignacionService = asignacionService;
        }

        // Consulta los seguros de un asegurado por cédula
        [HttpGet("por-cedula/{cedula}")]
        public async Task<IActionResult> ConsultarPorCedula(string cedula)
        {
            var response = await _asignacionService.ConsultarPorCedulaAsync(cedula);
            return response.Success ? Ok(response) : NotFound(response);
        }

        // Consulta los asegurados de un seguro por código
        [HttpGet("por-codigo-seguro/{codigoSeguro}")]
        public async Task<IActionResult> ConsultarPorCodigoSeguro(string codigoSeguro)
        {
            var response = await _asignacionService.ConsultarPorCodigoSeguroAsync(codigoSeguro);
            return response.Success ? Ok(response) : NotFound(response);
        }
    }
}
