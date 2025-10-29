using Sistema_HelpDesk.Desk.Application.CommomResult;
using Sistema_HelpDesk.Desk.Application.Contracts.UnitOfWork;
using Sistema_HelpDesk.Desk.Application.UseCases.Companies.DTOs;
using Sistema_HelpDesk.Desk.Application.UseCases.Companies.Interface;
using Sistema_HelpDesk.Desk.Application.UseCases.Empresas.DTOs;
using Sistema_HelpDesk.Desk.Application.UseCases.Utils;
using Sistema_HelpDesk.Desk.Domain.Empresas.Entidades;

namespace Sistema_HelpDesk.Desk.Application.UseCases.Companies
{
    public class AtualizarEmpresaUseCase(IUnitOfWork unitOfWork) : IAtualizarEmpresaUseCase
    {
        public async Task<ResultModel> AtualizarNomeEmpresa(int empresaId, EmpresaAtualizarNome novoNome)
        {
            var result = await unitOfWork.EmpresaRepository.AtualizarEmpresaNome(empresaId, novoNome);

            if (!result)
                return ResultModel.Erro("Não foi possível realizar essa alteração!");

            await unitOfWork.SaveChangesAsync();

            return ResultModel.Sucesso();
        }

        public async Task<ResultModel> AtualizarEmailEmpresa(int empresaId, EmpresaAtualizarEmail novoEmail)
        {
            if (!ValidacaoEmail.ValidarEmail(novoEmail.Email))
                return ResultModel.Erro("E-mail inválido!");

            var result = await unitOfWork.EmpresaRepository.AtualizarEmpresaEmail(empresaId, novoEmail);

            if (!result)
                return ResultModel.Erro("Não foi possível realizar essa alteração!");

            await unitOfWork.SaveChangesAsync();

            return ResultModel.Sucesso();
        }
    }
}
