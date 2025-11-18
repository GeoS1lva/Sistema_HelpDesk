using Sistema_HelpDesk.Desk.Application.CommomResult;
using Sistema_HelpDesk.Desk.Application.Contracts.UnitOfWork;
using Sistema_HelpDesk.Desk.Application.UseCases.Tickets.DTOs;
using Sistema_HelpDesk.Desk.Application.UseCases.Tickets.Interface;
using Sistema_HelpDesk.Desk.Domain.Chamados.Enums;

namespace Sistema_HelpDesk.Desk.Application.UseCases.Tickets
{
    public class AtualizarInformacoesChamadosUseCase(IUnitOfWork unitOfWork) : IAtualizarInformacoesChamadoUseCase
    {
        public async Task<ResultModel> AlteracoesChamados(AlterarChamado alterarChamado)
        {
            var chamado = await unitOfWork.ChamadoRepository.RetornarChamado(alterarChamado.NumeroChamado);

            if (chamado == null)
                return ResultModel.Erro("Chamado não encontrado!");

            if (chamado.Status == Status.finalizado)
                return ResultModel.Erro("Não é possível realizar essa alteração pois o chamado já foi finalizado!");

            if(alterarChamado.CategoriaId != null && alterarChamado.SubCategoriaId != null)
            {
                var categoria = await unitOfWork.CategoriaRepository.RetornarCategoria(alterarChamado.CategoriaId);

                var subCategoria = await unitOfWork.SubCategoriasRepository.RetornarSubCategoria(alterarChamado.SubCategoriaId);

                if (subCategoria.CategoriaId != categoria.Id)
                    return ResultModel.Erro($"A SubCategoria {subCategoria.Nome} não pertence a categoria {categoria.Nome}");

                chamado.AlterarCategoriaSubCategoria(categoria, subCategoria);
            }

            if(alterarChamado.MesaAtendimentoId != null)
            {
                var mesaAtendimento = await unitOfWork.MesasAtendimentosRepository.RetornarMesaAtendimento(alterarChamado.MesaAtendimentoId);

                chamado.AlterarMesa(mesaAtendimento);
            }

            if(alterarChamado.TecnicoResponsavelUserName != null)
            {
                var userLogin = await unitOfWork.UserLoginRepository.RetornarLogin(alterarChamado.TecnicoResponsavelUserName);
                var usuario = await unitOfWork.UsuarioSistemaRepository.RetornarUsuario(userLogin.Id);

                chamado.AlterarTecnicoResponsavel(usuario);
            }

            await unitOfWork.SaveChangesAsync();

            return ResultModel.Sucesso();
        }
    }
}
