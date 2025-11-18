using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sistema_HelpDesk.Desk.Application.UseCases.Tickets.DTOs;
using Sistema_HelpDesk.Desk.Application.UseCases.Tickets.Interface;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace Sistema_HelpDesk.Controllers.Tickets
{
    [ApiController]
    [Route("api/[controller]")]
    public class chamadosController(ICriarChamadoUseCase criarChamado, IAcoesChamadoUseCase acoesChamado, IRetornarInformacoesChamadosUseCase retornarInformacoes, IAtualizarInformacoesChamadoUseCase atualizarInformacoes) : Controller
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

        [HttpPost("play/{NumeroChamado}")]
        [Authorize(Roles = "Administrador,Tecnico")]
        public async Task<IActionResult> IniciarAtendimento([Required] string NumeroChamado)
        {
            var userRequest = User.Identity.Name ?? User.FindFirstValue(ClaimTypes.Name);

            var result = await acoesChamado.PlayChamado(NumeroChamado, userRequest);

            if (result.Error)
                return BadRequest(result.ErrorMessage);

            return Ok(result.Value);
        }

        [HttpPost("pausar/{NumeroChamado}")]
        [Authorize(Roles = "Administrador,Tecnico")]
        public async Task<IActionResult> PausarAtendimento([FromBody] PausaChamado chamado)
        {
            var userRequest = User.Identity.Name ?? User.FindFirstValue(ClaimTypes.Name);

            var result = await acoesChamado.PausarChamado(chamado, userRequest);

            if (result.Error)
                return BadRequest(result.ErrorMessage);

            return Ok(result.Value);
        }

        [HttpPost("finalizar/{NumeroChamado}")]
        [Authorize(Roles = "Administrador,Tecnico")]
        public async Task<IActionResult> FinalizarAtendimento([FromBody] FinalizarChamado chamado)
        {
            var userRequest = User.Identity.Name ?? User.FindFirstValue(ClaimTypes.Name);

            var result = await acoesChamado.FinalizarChamado(chamado, userRequest);

            if (result.Error)
                return BadRequest(result.ErrorMessage);

            return Ok(result.Value);
        }

        [HttpGet("dashboard")]
        [Authorize(Roles = "Administrador,Tecnico")]
        public async Task<IActionResult> RetornarInformacoesChamadoskanban()
        {
            var result = await retornarInformacoes.RetornarInformacoesKanban();

            if (result.Error)
                return BadRequest(result.ErrorMessage);

            return Ok(result.Value);
        }

        [HttpGet("painel-cliente/informativo")]
        [Authorize(Roles = "Usuario")]
        public async Task<IActionResult> RetornarInformacoesChamadosUsuarioEmpresa()
        {
            var userRequest = User.Identity.Name ?? User.FindFirstValue(ClaimTypes.Name);

            var result = await retornarInformacoes.RetornarInformacoesChamadosUsuarioEmpresa(userRequest);

            if (result.Error)
                return BadRequest(result.ErrorMessage);

            return Ok(result.Value);
        }

        [HttpGet("painel-cliente/{NumeroChamado}")]
        [Authorize(Roles = "Usuario")]
        public async Task<IActionResult> RetornarChamadoUsuarioEmpresa([Required] string NumeroChamado)
        {
            var result = await retornarInformacoes.RetornarChamadoUsuarioEmpresa(NumeroChamado);

            if (result.Error)
                return BadRequest(result.ErrorMessage);

            return Ok(result.Value);
        }

        [HttpPatch("alterar/dados/{NumeroChamado}")]
        [Authorize(Roles = "Administrador,Tecnico")]
        public async Task<IActionResult> AtualizarDadosChamado([FromBody] AlterarChamado chamado)
        {
            var result = await atualizarInformacoes.AlteracoesChamados(chamado);

            if (result.Error)
                return BadRequest(result.ErrorMessage);

            return Ok();
        }

        [HttpPost("adicionar-comentario/{NumeroChamado}")]
        [Authorize(Roles = "Administrador,Tecnico,Usuario")]
        public async Task<IActionResult> AdicionarComentarioChamado([FromBody] AdicionarComentario comentario)
        {
            var userRequest = User.Identity.Name ?? User.FindFirstValue(ClaimTypes.Name);

            var result = await acoesChamado.AdicionarComentarioChamado(comentario, userRequest);

            if (result.Error)
                return BadRequest(result.ErrorMessage);

            return Ok(result.Value);
        }

        [HttpGet("acoes/{NumeroChamado}")]
        [Authorize(Roles = "Administrador,Tecnico,Usuario")]
        public async Task<IActionResult> RetornarAcoesChamado([Required] string NumeroChamado)
        {
            var result = await acoesChamado.RetornoTodasAcoesChamado(NumeroChamado);

            return Ok(result);
        }

        [HttpGet("quantidade")]
        [Authorize(Roles = "Usuario")]
        public async Task<IActionResult> RetornarQuantidadeChamados()
        {
            var userRequest = User.Identity.Name ?? User.FindFirstValue(ClaimTypes.Name);

            var result = await retornarInformacoes.RetornarQuantidadeChamadoPorUsuarioEmpresa(userRequest);

            if (result.Error)
                return BadRequest(result.ErrorMessage);

            return Ok(result.Value);
        }
    }
}
