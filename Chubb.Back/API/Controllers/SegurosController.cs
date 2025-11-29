using Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SegurosController : ControllerBase
    {
        private readonly ISeguroService _seguroService;

        public SegurosController(ISeguroService seguroService)
        {
            _seguroService = seguroService;
        }

        // Obtiene todos los seguros activos
        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var response = await _seguroService.ObtenerTodosAsync();
            return response.Success ? Ok(response) : BadRequest(response);
        }

        // Obtiene un seguro por ID
        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var response = await _seguroService.ObtenerPorIdAsync(id);
            return response.Success ? Ok(response) : NotFound(response);
        }

        // Crea un nuevo seguro
        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CrearSeguroDto dto)
        {
            var response = await _seguroService.CrearAsync(dto);
            return response.Success ? CreatedAtAction(nameof(ObtenerPorId), new { id = response.Data?.SeguroId }, response) : BadRequest(response);
        }

        // Actualiza un seguro existente
        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] ActualizarSeguroDto dto)
        {
            if (id != dto.SeguroId)
            {
                return BadRequest(new { Message = "El ID no coincide" });
            }

            var response = await _seguroService.ActualizarAsync(dto);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        // Elimina un seguro (lógico)
        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var response = await _seguroService.EliminarAsync(id);
            return response.Success ? Ok(response) : BadRequest(response);
        }
    }
}
