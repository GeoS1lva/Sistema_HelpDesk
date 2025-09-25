using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sistema_HelpDesk.Desk.Application.Autenticação.Interface;
using Sistema_HelpDesk.Desk.Application.Users.Interface;
using Sistema_HelpDesk.Desk.Domain.Models.DTOs.Autenticação;
using Sistema_HelpDesk.Desk.Domain.Users.ResetarSenhaUser;
using System.ComponentModel.DataAnnotations;

namespace Sistema_HelpDesk.Controllers.Autenticação
{
    [ApiController]
    [Route("api/[controller]")]
    public class AutenticacaoController(IAutheService service, IResetarSenhaService reset) : ControllerBase
    {
        [HttpPost("CadastrarLogin")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> CriarLogin([FromBody][Required] CadastrarUserDto dto)
        {
            var result = await service.CriarLogin(dto);

            if(result.Error)
                return BadRequest(result.ErrorMessage);

            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody][Required] LoginUserDto dto)
        {
            var result = await service.FazerLogin(dto);

            Response.Cookies.Append("jwt", result, new CookieOptions
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
        public async Task<IActionResult> SolicResetSenha([FromBody] UserResetSenha user)
        {
            var result = await reset.SolicitarRedefinicaoSenha(user);

            if(result.Error)
                return BadRequest(result.ErrorMessage);

            return Ok("Email Enviando com Sucesso!");
        }
    }
}
