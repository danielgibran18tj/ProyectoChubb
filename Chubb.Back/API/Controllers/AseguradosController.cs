using Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AseguradosController : ControllerBase
    {
        private readonly IAseguradoService _aseguradoService;

        public AseguradosController(IAseguradoService aseguradoService)
        {
            _aseguradoService = aseguradoService;
        }

        // Obtiene todos los asegurados activos
        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var response = await _aseguradoService.ObtenerTodosAsync();
            return response.Success ? Ok(response) : BadRequest(response);
        }

        // Obtiene un asegurado por ID
        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var response = await _aseguradoService.ObtenerPorIdAsync(id);
            return response.Success ? Ok(response) : NotFound(response);
        }

        // Crea un nuevo asegurado
        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CrearAseguradoDto dto)
        {
            var response = await _aseguradoService.CrearAsync(dto);
            return response.Success ? CreatedAtAction(nameof(ObtenerPorId), new { id = response.Data?.AseguradoId }, response) : BadRequest(response);
        }

        // Actualiza un asegurado existente
        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] ActualizarAseguradoDto dto)
        {
            if (id != dto.AseguradoId)
            {
                return BadRequest(new { Message = "El ID no coincide" });
            }

            var response = await _aseguradoService.ActualizarAsync(dto);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        // Elimina un asegurado (lógico)
        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var response = await _aseguradoService.EliminarAsync(id);
            return response.Success ? Ok(response) : BadRequest(response);
        }
    }
}
