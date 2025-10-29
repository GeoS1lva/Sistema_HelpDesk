using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sistema_HelpDesk.Desk.Application.UseCases.ServiceDesks.DTOs;
using Sistema_HelpDesk.Desk.Application.UseCases.ServiceDesks.Interface;

namespace Sistema_HelpDesk.Controllers.ServiceDesks
{
    [ApiController]
    [Route("api/[controller]")]
    public class mesasatendimentoController(ICriarMesaAtendimentoUseCase criarMesaAtendimento, IRemoverMesaAtendimentoUseCase removerMesaAtendimento, IRetornarInformacoesMesasAtendimentoUseCase retornarInformacoesMesasAtendimento) : ControllerBase
    {
        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> CadastrarMesaAtendimento([FromBody] MesaAtendimentoDTO mesa)
        {
            var result = await criarMesaAtendimento.CriarMesaAtendimento(mesa);

            if (result.Error)
                return BadRequest(result.ErrorMessage);

            return Ok();
        }
        
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> DeletarMesaAtendimento(int id)
        {
            var result = await removerMesaAtendimento.RemoverMesaAtendimento(id);

            if (result.Error)
                return BadRequest(result.ErrorMessage);

            return Ok();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> RetornarMesasAtendimento()
        {
            var result = await retornarInformacoesMesasAtendimento.RetornarMesasAtendimento();

            if (result.Error)
                return BadRequest(result.ErrorMessage);

            return Ok(result.Value);
        }
    }
}
