using Sistema_HelpDesk.Desk.Application.CommomResult;

namespace Sistema_HelpDesk.Desk.Application.UseCases.Category.Interface
{
    public interface IDeletarAtivarCategoriaUseCase
    {
        public Task<ResultModel> DesativarCategoria(int id);
        public Task<ResultModel> AtivarCategoria(int id);
    }
}
