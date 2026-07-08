using Microsoft.AspNetCore.Mvc;
using TupApi.DTOs;
using TupApi.Services;

namespace TupApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _service;

        public AuthController(AuthService service)
        {
            _service = service;
        }

        /// <summary>
        /// POST /api/auth/registro
        /// Registra un nuevo usuario y devuelve un JWT.
        /// </summary>
        [HttpPost("registro")]
        public async Task<ActionResult<AuthResponseDto>> Registro([FromBody] RegistroDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _service.RegistrarAsync(dto);
            return StatusCode(201, response);
        }

        /// <summary>
        /// POST /api/auth/login
        /// Autentica un usuario y devuelve un JWT.
        /// </summary>
        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _service.LoginAsync(dto);
            return Ok(response);
        }
    }
}