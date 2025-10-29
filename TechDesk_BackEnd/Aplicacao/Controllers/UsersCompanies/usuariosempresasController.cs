using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sistema_HelpDesk.Desk.Application.UseCases.UsersCompanies.DTOs;
using Sistema_HelpDesk.Desk.Application.UseCases.UsersCompanies.Interface;
using System.ComponentModel.DataAnnotations;

namespace Sistema_HelpDesk.Controllers.UsersCompanies
{
    [ApiController]
    [Route("api/[controller]")]
    public class usuariosempresasController(ICriarUsuarioEmpresaUseCase criarUsuarioEmpresa, IRetornarInformacoesUsuarioEmpresaUseCase retornarInformacoes) : ControllerBase
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
    }
}
