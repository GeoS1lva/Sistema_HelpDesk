using Sistema_HelpDesk.Desk.Application.CommomResult;
using Sistema_HelpDesk.Desk.Application.Contracts.UnitOfWork;
using Sistema_HelpDesk.Desk.Application.UseCases.Category.DTOs;
using Sistema_HelpDesk.Desk.Application.UseCases.Category.Interface;
using Sistema_HelpDesk.Desk.Domain.Chamados.Entidades;
using Sistema_HelpDesk.Desk.Domain.Chamados.Entities;
using Sistema_HelpDesk.Desk.Domain.Chamados.Enums;

namespace Sistema_HelpDesk.Desk.Application.UseCases.Category
{
    public class CriarCategoriaUseCase(IUnitOfWork unitOfWork) : ICriarCategoriaUseCase
    {
        public async Task<ResultModel> AdicionarCategoria(CategoriaCriar categoria)
        {
            if (await unitOfWork.CategoriaRepository.ConsultarCategoria(categoria.Nome))
                return ResultModel.Erro("Essa Categoria já existe!");

            unitOfWork.CategoriaRepository.AdicionarCategoria(new Categoria(categoria.Nome));

            await unitOfWork.SaveChangesAsync();

            return ResultModel.Sucesso();
        }

        public async Task<ResultModel> AdicionarSubCategorias(List<SubCategoriaCriar> subCategorias, int categoriaId)
        {
            if (subCategorias.Any(x => x.SLA < 3600))
                return ResultModel.Erro("O Valor mínimo do SLA é 1 hora");

            var categoria = await unitOfWork.CategoriaRepository.RetornarCategoria(categoriaId);

            foreach(var subCategoria in subCategorias)
            {
                if (categoria.SubCategorias.Any(x => x.Nome == subCategoria.Nome))
                    return ResultModel.Erro($"A Categoria {categoria.Nome} já possui a subCategoria {subCategoria.Nome}");

                var mesaAtendimento = await unitOfWork.MesasAtendimentosRepository.RetornarMesaAtendimento(subCategoria.MesaAtendimentoId);

                if (mesaAtendimento == null)
                    return ResultModel.Erro($"MesaAtendimento não encontrada para a subCategoria {subCategoria.Nome}!");

                var novaSubCategoria = new SubCategoria(subCategoria.Nome, subCategoria.Prioridade, subCategoria.SLA, mesaAtendimento.Id, categoria.Id);

                categoria.SubCategorias.Add(novaSubCategoria);
                unitOfWork.SubCategoriasRepository.AdicionarSubCategoria(novaSubCategoria);
            }

            await unitOfWork.SaveChangesAsync();

            return ResultModel.Sucesso();
        }
    }
}
