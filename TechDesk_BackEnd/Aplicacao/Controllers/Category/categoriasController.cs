using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sistema_HelpDesk.Desk.Application.UseCases.Category.DTOs;
using Sistema_HelpDesk.Desk.Application.UseCases.Category.Interface;
using System.ComponentModel.DataAnnotations;

namespace Sistema_HelpDesk.Controllers.Category
{
    [ApiController]
    [Route("api/[controller]")]
    public class categoriasController(ICriarCategoriaUseCase criarCategoria, IDeletarAtivarCategoriaUseCase deletarAtivar, IRetornarInformacoesCategoriaUseCase retornarInformacoes) : Controller
    {
        [HttpPost]
        [Authorize(Roles = "Administrador,Tecnico")]
        public async Task<IActionResult> CadastrarCategoria([FromBody] CategoriaCriar categoriaCriar)
        {
            var result = await criarCategoria.AdicionarCategoria(categoriaCriar);

            if(result.Error)
                return BadRequest(result.ErrorMessage);

            return Ok();
        }

        [HttpPost("subCategoria")]
        [Authorize(Roles = "Administrador,Tecnico")]
        public async Task<IActionResult> CadastrarSubCategoria([Required] List<SubCategoriaCriar> subCategorias, [Required] int categoriaId)
        {
            var result = await criarCategoria.AdicionarSubCategorias(subCategorias, categoriaId);

            if (result.Error)
                return BadRequest(result.ErrorMessage);

            return Ok();
        }

        [HttpPatch("{id}/inativar")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> DesativarCategoria([Required] int id)
        {
            var result = await deletarAtivar.DesativarCategoria(id);

            if (result.Error)
                return BadRequest(result.ErrorMessage);

            return Ok();
        }

        [HttpPatch("{id}/ativar")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> AtivarCategoria([Required] int id)
        {
            var result = await deletarAtivar.AtivarCategoria(id);

            if (result.Error)
                return BadRequest(result.ErrorMessage);

            return Ok();
        }

        [HttpPatch("subCategoria/{id}/inativar")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> DesativarSubCategoria([Required] int id)
        {
            var result = await deletarAtivar.DesativarSubCategoria(id);

            if (result.Error)
                return BadRequest(result.ErrorMessage);

            return Ok();
        }

        [HttpPatch("subCategoria/{id}/ativar")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> AtivarSubCategoria([Required] int id)
        {
            var result = await deletarAtivar.AtivarSubCatetgoria(id);

            if (result.Error)
                return BadRequest(result.ErrorMessage);

            return Ok();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> RetornarCategorias()
        {
            var result = await retornarInformacoes.RetornarCategorias();

            if (result.Error)
                return BadRequest(result.ErrorMessage);

            return Ok(result.Value);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> RetornarCategoria([Required] int id)
        {
            var result = await retornarInformacoes.RetornarCategoria(id);

            if (result.Error)
                return BadRequest(result.ErrorMessage);

            return Ok(result.Value);
        }
    }
}
