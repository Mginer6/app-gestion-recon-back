using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AppGestionReconocimientos.Api.Data;
using Microsoft.AspNetCore.Authorization;
using AppGestionReconocimientos.Api.Auth;

namespace AppGestionReconocimientos.Api.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsuarioController(AppDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        [Authorize(Roles = Roles.Administrador)]
        public async Task<IActionResult> ObtenerUsuarios()
        {
            var usuarios = await _context.Usuarios.ToListAsync();
            return Ok(usuarios);
        }
    }
}
