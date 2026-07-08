using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using TupApi.DTOs;
using TupApi.Models;
using TupApi.Repositories;

namespace TupApi.Services
{
    public class AuthService
    {
        private readonly IUsuarioRepository _repo;
        private readonly IConfiguration _config;

        public AuthService(IUsuarioRepository repo, IConfiguration config)
        {
            _repo = repo;
            _config = config;
        }

        public async Task<AuthResponseDto> RegistrarAsync(RegistroDto dto)
        {
            // Validar email único
            if (await _repo.ExisteEmailAsync(dto.Email.Trim().ToLower()))
                throw new InvalidOperationException(
                    "Ya existe una cuenta registrada con ese email.");

            // Hash de la contraseña con BCrypt
            var hash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            var usuario = new Usuario
            {
                Nombre        = dto.Nombre.Trim(),
                Email         = dto.Email.Trim().ToLower(),
                PasswordHash  = hash,
                Rol           = "usuario",
                FechaRegistro = DateTime.UtcNow
            };

            await _repo.CreateAsync(usuario);
            return GenerarToken(usuario);
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
        {
            var usuario = await _repo.GetByEmailAsync(dto.Email.Trim().ToLower());

            if (usuario == null || !BCrypt.Net.BCrypt.Verify(dto.Password, usuario.PasswordHash))
                throw new UnauthorizedAccessException(
                    "Email o contraseña incorrectos.");

            return GenerarToken(usuario);
        }

        private AuthResponseDto GenerarToken(Usuario usuario)
        {
            var jwtSettings = _config.GetSection("JwtSettings");
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!));

            var expira = DateTime.UtcNow.AddHours(
                double.Parse(jwtSettings["ExpirationHours"]!));

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id!),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim(ClaimTypes.Name, usuario.Nombre),
                new Claim(ClaimTypes.Role, usuario.Rol),
            };

            var token = new JwtSecurityToken(
                issuer:   jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims:   claims,
                expires:  expira,
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            return new AuthResponseDto
            {
                Token  = new JwtSecurityTokenHandler().WriteToken(token),
                Nombre = usuario.Nombre,
                Email  = usuario.Email,
                Rol    = usuario.Rol,
                Expira = expira
            };
        }
    }
}