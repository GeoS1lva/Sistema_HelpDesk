using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sistema_HelpDesk.Desk.Application.CommomResult;
using Sistema_HelpDesk.Desk.Application.UseCases.Companies.DTOs;
using Sistema_HelpDesk.Desk.Application.UseCases.Companies.Interface;
using Sistema_HelpDesk.Desk.Application.UseCases.Empresas.DTOs;
using System.ComponentModel.DataAnnotations;

namespace Sistema_HelpDesk.Controllers.Companies
{
    [ApiController]
    [Route("api/[controller]")]
    public class empresasController(ICrirEmpresaUseCase criarEmpresa, IAtualizarEmpresaUseCase atualizarEmpresa, IRetornarInformacoesEmpresaUseCase retornarInformacoes, IRemoverAtivarEmpresaUseCase removerAtivarEmpresa) : ControllerBase
    {
        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> CadastrarEmpresa([FromBody] EmpresaCriar empresa)
        {
            var result = await criarEmpresa.CadastrarEmpresa(empresa);

            if(result.Error)
                return BadRequest(result.ErrorMessage);

            return Ok();
        }

        [HttpPatch("{id}/nome")]
        [Authorize(Roles = "Administrador,Tecnico")]
        public async Task<IActionResult> AtualizarNomeEmpresa(int id, [FromBody] EmpresaAtualizarNome empresa)
        {
            var result = await atualizarEmpresa.AtualizarNomeEmpresa(id, empresa);

            if (result.Error)
                return BadRequest(result.ErrorMessage);

            return Ok();
        }

        [HttpPatch("{id}/email")]
        [Authorize(Roles = "Administrador,Tecnico")]
        public async Task<IActionResult> AtualizarEmailEmpresa(int id, [FromBody] EmpresaAtualizarEmail empresa)
        {
            var result = await atualizarEmpresa.AtualizarEmailEmpresa(id, empresa);

            if (result.Error)
                return BadRequest(result.ErrorMessage);

            return Ok();
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> RetornarEmpresa([Required] int id)
        {
            var result = await retornarInformacoes.RetornarEmpresa(id);

            if (result.Error)
                return BadRequest(result.ErrorMessage);

            return Ok(result.Value);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> RetornarTodasEmpresa()
        {
            var result = await retornarInformacoes.RetornarTodasEmpresas();

            if (result.Error)
                return BadRequest(result.ErrorMessage);

            return Ok(result.Value);
        }

        [HttpGet("{id}/users")]
        [Authorize]
        public async Task<IActionResult> RetornarUsuariosPorEmpresa([Required] int id)
        {
            var result = await retornarInformacoes.RetornarUsuariosEmpresas(id);

            if (result.Error)
                return BadRequest(result.ErrorMessage);

            return Ok(result.Value);
        }

        [HttpPatch("{id}/inativar")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> DesativarEmpresa([Required] int id)
        {
            var result = await removerAtivarEmpresa.RemoverEmpresa(id);

            if (result.Error)
                return BadRequest(result.ErrorMessage);

            return Ok();
        }

        [HttpPatch("{id}/ativar")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> AtivarEmpresa([Required] int id)
        {
            var result = await removerAtivarEmpresa.AtivarEmpresa(id);

            if (result.Error)
                return BadRequest(result.ErrorMessage);

            return Ok();
        }
    }
}
