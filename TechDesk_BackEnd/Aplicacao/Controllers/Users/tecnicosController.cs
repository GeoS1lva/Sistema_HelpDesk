using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sistema_HelpDesk.Desk.Application.UseCases.Users.DTOs;
using Sistema_HelpDesk.Desk.Application.UseCases.Users.Interface;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace Sistema_HelpDesk.Controllers.Users
{
    [ApiController]
    [Route("api/[controller]")]
    public class tecnicosController(ICriarUsuarioSistemaUseCase criarTecnico, IRemoverUsuarioSistemaUseCase removerTecnico, IRetornarInformacoesUseCase informacoesTecnicos, IAtualizarInformacoesUsuarioSistemaUseCase atualizarInformacoes) : ControllerBase
    {
        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Cadastrar([FromBody] UserCriar userTecnico)
        {
            var result = await criarTecnico.CriarUsuario(userTecnico);

            if(result.Error)
                return BadRequest(result.ErrorMessage);

            return Ok();
        }

        [HttpDelete("{userName}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Desativar([Required] string userName)
        {
            var userRequest = User.Identity.Name ?? User.FindFirstValue(ClaimTypes.Name);

            var result = await removerTecnico.RemoverUsuario(userName, userRequest);

            if (result.Error)
                return BadRequest(result.ErrorMessage);

            return Ok();
        }

        [HttpGet("{userName}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> RetornarInformacoesTecnico([Required] string userName)
        {
            var result = await informacoesTecnicos.RetornarInformacoesUsuario(userName);

            if (result.Error)
                return BadRequest(result.ErrorMessage);

            return Ok(result.Value);
        }
        
        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> RetornarInformacoesUsuarios()
        {
            var result = await informacoesTecnicos.RetornarInformacoesUsuariosCriado();

            if (result.Error)
                return BadRequest(result.ErrorMessage);

            return Ok(result.Value);
        }

        [HttpPatch("{userName}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> AtualizarDadosUsuarioSistema([FromBody] UserLoginAtualizar usuario)
        {
            var result = await atualizarInformacoes.AtualizarDadosUsuarioSistema(usuario);

            if (result.Error)
                return BadRequest(result.ErrorMessage);

            return Ok(result.Value);
        }
    }
}
