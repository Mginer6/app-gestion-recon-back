using AppGestionReconocimientos.Api.Auth;
using AppGestionReconocimientos.Api.Data;
using AppGestionReconocimientos.Api.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AppGestionReconocimientos.Api.Controllers
{
    public record LoginRequest(string Email);

    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(AppDbContext db, IConfiguration config) : ControllerBase
    {
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest req)
        {
            var usuario = await db.Usuarios.FirstOrDefaultAsync(user => user.Email == req.Email);
            if (usuario is null) return Unauthorized();

            var rol = usuario.Rol.ToString();

            var claims = new[]
            {
                new Claim("sub", usuario.IdUsuario.ToString()),
                new Claim("name", usuario.Nombre),
                new Claim("role", rol),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!));
            var token = new JwtSecurityToken(
                issuer: config["Jwt:Issuer"],
                audience: config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(8),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                rol,
                nombre = usuario.Nombre
            });
        }
    }
}
