using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sistema_HelpDesk.Desk.Application.UseCases.Users.DTOs;
using Sistema_HelpDesk.Desk.Application.UseCases.UsersCompanies.DTOs;
using Sistema_HelpDesk.Desk.Application.UseCases.UsersCompanies.Interface;
using System.ComponentModel.DataAnnotations;

namespace Sistema_HelpDesk.Controllers.UsersCompanies
{
    [ApiController]
    [Route("api/[controller]")]
    public class usuariosempresasController(ICriarUsuarioEmpresaUseCase criarUsuarioEmpresa, IRetornarInformacoesUsuarioEmpresaUseCase retornarInformacoes, IRemoverAtivarUsuarioEmpresaUseCase removerAtivarUsuario, IAtualizarInformacoesUsuarioEmpresaUseCase atualizarInformacoes) : ControllerBase
    {
        [HttpPost]
        [Authorize(Roles = "Administrador,Tecnico")]
        public async Task<IActionResult> CadastrarUsuarioEmpresa([FromBody] UserEmpresaCriar user)
        {
            var result = await criarUsuarioEmpresa.CadastrarUsuarioEmpresa(user);

            if(result.Error)
                return BadRequest(result.ErrorMessage);

            return Ok();
        }

        [HttpGet("{userName}")]
        [Authorize(Roles = "Administrador,Tecnico")]
        public async Task<IActionResult> RetornarInformacoes([Required] string userName)
        {
            var result = await retornarInformacoes.RetornarInformacoesUsuarios(userName);

            if (result.Error)
                return BadRequest(result.ErrorMessage);

            return Ok(result.Value);
        }

        [HttpPatch("{id}/inativar")]
        [Authorize(Roles = "Administrador,Tecnico")]
        public async Task<IActionResult> DesativarUsuarioEmpresa([Required] int id)
        {
            var result = await removerAtivarUsuario.RemoverUsuarioEmpresa(id);

            if (result.Error)
                return BadRequest(result.ErrorMessage);

            return Ok();
        }

        [HttpPatch("{id}/ativar")]
        [Authorize(Roles = "Administrador,Tecnico")]
        public async Task<IActionResult> AtivarUsuarioEmpresa([Required] int id)
        {
            var result = await removerAtivarUsuario.AtivarUsuarioEmpresa(id);

            if (result.Error)
                return BadRequest(result.ErrorMessage);

            return Ok();
        }

        [HttpPatch("{userName}")]
        [Authorize(Roles = "Administrador,Tecnico")]
        public async Task<IActionResult> AtualizarInformacoes([FromBody] UserLoginAtualizar user)
        {
            var result = await atualizarInformacoes.AtualizarDadosUsuarioEmpresa(user);

            if (result.Error)
                return BadRequest(result.ErrorMessage);

            return Ok(result.Value);
        }
    }
}
