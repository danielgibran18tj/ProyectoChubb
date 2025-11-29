using Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AsignacionesController : ControllerBase
    {
        private readonly IAsignacionService _asignacionService;

        public AsignacionesController(IAsignacionService asignacionService)
        {
            _asignacionService = asignacionService;
        }

        // Asigna un seguro a un asegurado
        [HttpPost]
        public async Task<IActionResult> AsignarSeguro([FromBody] AsignacionDto dto)
        {
            var response = await _asignacionService.AsignarSeguroAsync(dto);
            return response.Success ? Ok(response) : BadRequest(response);
        }
    }
}
