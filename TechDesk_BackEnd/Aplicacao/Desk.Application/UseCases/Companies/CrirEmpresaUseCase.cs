using Sistema_HelpDesk.Desk.Application.CommomResult;
using Sistema_HelpDesk.Desk.Application.Contracts.UnitOfWork;
using Sistema_HelpDesk.Desk.Application.UseCases.Companies.DTOs;
using Sistema_HelpDesk.Desk.Application.UseCases.Companies.Interface;
using Sistema_HelpDesk.Desk.Application.UseCases.Utils;
using Sistema_HelpDesk.Desk.Domain.Empresas.Entidades;

namespace Sistema_HelpDesk.Desk.Application.UseCases.Companies
{
    public class CrirEmpresaUseCase(IUnitOfWork unitOfWork) : ICrirEmpresaUseCase
    {
        public async Task<ResultModel> CadastrarEmpresa(EmpresaCriar empresa)
        {
            if (!ValidacaoEmail.ValidarEmail(empresa.Email))
                return ResultModel.Erro("E-mail com nomeclatura incorreta!");

            string cnpj = new string(empresa.Cnpj.Where(char.IsDigit).ToArray());

            if (await unitOfWork.EmpresaRepository.ConsultarCnpj(cnpj))
                return ResultModel.Erro("CNPJ já cadastrado!");

            if (!ValidacaoCnpj.ValidarCnpj(cnpj))
                return ResultModel.Erro("CNPJ inválido!");

            unitOfWork.EmpresaRepository.AdicionarEmpresa(new Empresa(empresa.Nome, cnpj, empresa.Email));

            await unitOfWork.SaveChangesAsync();

            return ResultModel.Sucesso();
        }
    }
}
