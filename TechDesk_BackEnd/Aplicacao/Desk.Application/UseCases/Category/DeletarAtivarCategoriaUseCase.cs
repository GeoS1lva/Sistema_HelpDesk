using Sistema_HelpDesk.Desk.Application.CommomResult;
using Sistema_HelpDesk.Desk.Application.Contracts.UnitOfWork;
using Sistema_HelpDesk.Desk.Application.UseCases.Category.Interface;
using Sistema_HelpDesk.Desk.Domain.Chamados.Entidades;
using Sistema_HelpDesk.Desk.Infra.Persistence.UnitOfWork;

namespace Sistema_HelpDesk.Desk.Application.UseCases.Category
{
    public class DeletarAtivarCategoriaUseCase(IUnitOfWork unitOfWork) : IDeletarAtivarCategoriaUseCase
    {
        public async Task<ResultModel> DesativarCategoria(int id)
        {
            var categoria = await unitOfWork.CategoriaRepository.RetornarCategoria(id);

            if (categoria == null)
                return ResultModel.Erro("Não foi possível encontrar essa categoria");

            var subCategorias = await unitOfWork.SubCategoriasRepository.RetornarSubCategoriaPorCategoria(id);

            if(subCategorias.Count == 0)
            {
                categoria.Desativar();
                await unitOfWork.SaveChangesAsync();

                return ResultModel.Sucesso();
            }

            categoria.Desativar();
            foreach (var item in subCategorias)
            {
                item.Desativar();
            }

            await unitOfWork.SaveChangesAsync();
            return ResultModel.Sucesso();
        }

        public async Task<ResultModel> AtivarCategoria(int id)
        {
            var categoria = await unitOfWork.CategoriaRepository.RetornarCategoria(id);

            if (categoria == null)
                return ResultModel.Erro("Não foi possível encontrar essa categoria");

            var subCategorias = await unitOfWork.SubCategoriasRepository.RetornarSubCategoriaPorCategoria(id);

            if (subCategorias.Count == 0)
            {
                categoria.Ativar();
                await unitOfWork.SaveChangesAsync();

                return ResultModel.Sucesso();
            }

            categoria.Ativar();
            foreach (var item in subCategorias)
            {
                item.Ativar();
            }

            await unitOfWork.SaveChangesAsync();
            return ResultModel.Sucesso();
        }

        public async Task<ResultModel> DesativarSubCategoria(int subCategoriaId)
        {
            var subCategoria = await unitOfWork.SubCategoriasRepository.RetornarSubCategoria(subCategoriaId);

            if (subCategoria == null)
                return ResultModel.Erro("Não foi possível encontrar essa subCategoria");

            subCategoria.Desativar();
            await unitOfWork.SaveChangesAsync();

            return ResultModel.Sucesso();
        }

        public async Task<ResultModel> AtivarSubCatetgoria(int subCategoriaId)
        {
            var subCategoria = await unitOfWork.SubCategoriasRepository.RetornarSubCategoria(subCategoriaId);

            if (subCategoria == null)
                return ResultModel.Erro("Não foi possível encontrar essa subCategoria");

            var categoria = await unitOfWork.CategoriaRepository.RetornarCategoria(subCategoria.CategoriaId);

            if (categoria.Status == false)
                return ResultModel.Erro("Não foi possível realizar essa operação pois a categoria está desativada");

            subCategoria.Ativar();
            await unitOfWork.SaveChangesAsync();

            return ResultModel.Sucesso();
        }
    }
}
