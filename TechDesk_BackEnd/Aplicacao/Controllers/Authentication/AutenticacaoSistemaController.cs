using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sistema_HelpDesk.Desk.Application.UseCases.Autenticação;
using Sistema_HelpDesk.Desk.Application.UseCases.Autenticação.DTOs;
using Sistema_HelpDesk.Desk.Application.UseCases.Users.DTOs;
using Sistema_HelpDesk.Desk.Application.UseCases.Users.ResetPasswords;
using System.ComponentModel.DataAnnotations;

namespace Sistema_HelpDesk.Controllers.Autenticação
{
    [ApiController]
    [Route("api/[controller]")]
    public class AutenticacaoSistemaController(IAutheUseCase service, IResetarSenhaUseCase reset) : ControllerBase
    {
        /*[HttpPost("CadastrarLogin")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> CriarLogin([FromBody][Required] UserLoginCriar dto)
        {
            var result = await service.CriarLogin(dto);

            if(result.Error)
                return BadRequest(result.ErrorMessage);

            return Ok();
        }*/

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody][Required] LoginUserAcessar dto)
        {
            var result = await service.FazerLogin(dto);

            if(result.Error)
                return BadRequest(result.ErrorMessage);

            Response.Cookies.Append("jwt", result.Value, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddMinutes(480)
            });
            
            return Ok();
        }
        
        [HttpGet("Logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            Response.Cookies.Delete("jwt");

            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> SolicResetSenha([FromBody] UserResetPasswords user)
        {
            var result = await reset.SolicitarRedefinicaoSenha(user);

            if(result.Error)
                return BadRequest(result.ErrorMessage);

            return Ok("Email Enviando com Sucesso!");
        }
    }
}
