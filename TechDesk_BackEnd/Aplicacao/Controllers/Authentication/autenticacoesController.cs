 using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sistema_HelpDesk.Desk.Application.UseCases.Autenticação;
using Sistema_HelpDesk.Desk.Application.UseCases.Autenticação.DTOs;
using Sistema_HelpDesk.Desk.Application.UseCases.Users.DTOs;
using Sistema_HelpDesk.Desk.Application.UseCases.Users.ResetPasswords;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace Sistema_HelpDesk.Controllers.Autenticação
{
    [ApiController]
    [Route("api/[controller]")]
    public class autenticacoesController(IAutheUseCase service, IResetarSenhaUseCase reset) : ControllerBase
    {
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginUserAcessar dto)
        {
            var result = await service.FazerLogin(dto);

            if(result.Error)
                return BadRequest(result.ErrorMessage);

            Response.Cookies.Append("jwt", result.Value, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddMinutes(480)
            });

            return Ok();
        }
        
        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> EncerarSessao()
        {
            Response.Cookies.Delete("jwt");

            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("/api/redefinicoes-senha")]
        public async Task<IActionResult> CriarSolicitacaoRedefinicaoSenha([FromBody] UserResetPasswords user)
        {
            var result = await reset.SolicitarRedefinicaoSenha(user);

            if(result.Error)
                return BadRequest(result.ErrorMessage);

            return Ok("Email Enviando com Sucesso!");
        }
    }
}
