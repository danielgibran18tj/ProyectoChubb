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

        /// <summary>
        /// Obtiene todos los asegurados activos
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var response = await _aseguradoService.ObtenerTodosAsync();
            return response.Success ? Ok(response) : BadRequest(response);
        }

        /// <summary>
        /// Obtiene un asegurado por ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var response = await _aseguradoService.ObtenerPorIdAsync(id);
            return response.Success ? Ok(response) : NotFound(response);
        }

        /// <summary>
        /// Crea un nuevo asegurado
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CrearAseguradoDto dto)
        {
            var response = await _aseguradoService.CrearAsync(dto);
            return response.Success ? CreatedAtAction(nameof(ObtenerPorId), new { id = response.Data?.AseguradoId }, response) : BadRequest(response);
        }

        /// <summary>
        /// Actualiza un asegurado existente
        /// </summary>
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

        /// <summary>
        /// Elimina un asegurado (lógico)
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var response = await _aseguradoService.EliminarAsync(id);
            return response.Success ? Ok(response) : BadRequest(response);
        }
    }
}
