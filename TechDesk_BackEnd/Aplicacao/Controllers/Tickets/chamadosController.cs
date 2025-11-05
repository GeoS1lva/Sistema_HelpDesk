using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sistema_HelpDesk.Desk.Application.UseCases.Tickets.DTOs;
using Sistema_HelpDesk.Desk.Application.UseCases.Tickets.Interface;
using System.Security.Claims;

namespace Sistema_HelpDesk.Controllers.Tickets
{
    [ApiController]
    [Route("api/[controller]")]
    public class chamadosController(ICriarChamadoUseCase criarChamado) : Controller
    {
        [HttpPost]
        [Authorize(Roles = "Administrador,Tecnico")]
        public async Task<IActionResult> AbrirChamadoUsuarioSistema([FromBody] AbrirChamadoUsuarioSistema solicitacao)
        {
            var userRequest = User.Identity.Name ?? User.FindFirstValue(ClaimTypes.Name);

            var result = await criarChamado.AbrirChamadoSistema(solicitacao, userRequest);

            if (result.Error)
                return BadRequest(result.ErrorMessage);

            return Ok(result.Value);
        }

        [HttpPost("painelChamados")]
        [Authorize(Roles = "Usuario")]
        public async Task<IActionResult> AbrirChamadoUsuarioEmpresa([FromBody] AbrirChamadoUsuarioEmpresa solicitacao)
        {
            var userRequest = User.Identity.Name ?? User.FindFirstValue(ClaimTypes.Name);

            var result = await criarChamado.AbrirChamadoPainel(solicitacao, userRequest);

            if (result.Error)
                return BadRequest(result.ErrorMessage);

            return Ok(result.Value);
        }
    }
}
