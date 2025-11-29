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

        /// <summary>
        /// Obtiene todos los seguros activos
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var response = await _seguroService.ObtenerTodosAsync();
            return response.Success ? Ok(response) : BadRequest(response);
        }

        /// <summary>
        /// Obtiene un seguro por ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var response = await _seguroService.ObtenerPorIdAsync(id);
            return response.Success ? Ok(response) : NotFound(response);
        }

        /// <summary>
        /// Crea un nuevo seguro
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CrearSeguroDto dto)
        {
            var response = await _seguroService.CrearAsync(dto);
            return response.Success ? CreatedAtAction(nameof(ObtenerPorId), new { id = response.Data?.SeguroId }, response) : BadRequest(response);
        }

        /// <summary>
        /// Actualiza un seguro existente
        /// </summary>
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

        /// <summary>
        /// Elimina un seguro (lógico)
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var response = await _seguroService.EliminarAsync(id);
            return response.Success ? Ok(response) : BadRequest(response);
        }
    }
}
