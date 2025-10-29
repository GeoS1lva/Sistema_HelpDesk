using Sistema_HelpDesk.Desk.Application.CommomResult;
using Sistema_HelpDesk.Desk.Application.Contracts.UnitOfWork;
using Sistema_HelpDesk.Desk.Application.UseCases.Companies.DTOs;
using Sistema_HelpDesk.Desk.Application.UseCases.Companies.Interface;
using Sistema_HelpDesk.Desk.Application.UseCases.UsersCompanies.DTOs;
using Sistema_HelpDesk.Desk.Application.UseCases.Utils;

namespace Sistema_HelpDesk.Desk.Application.UseCases.Companies
{
    public class RetornarInformacoesEmpresaUseCase(IUnitOfWork unitOfWork) : IRetornarInformacoesEmpresaUseCase
    {
        public async Task<ResultModel<EmpresaInformacoes>> RetornarEmpresa(int id)
        {
            var empresa = await unitOfWork.EmpresaRepository.RetornarEmpresa(id);

            return ResultModel<EmpresaInformacoes>.Sucesso(new EmpresaInformacoes { Id = id, Nome = empresa.Nome, Cnpj = empresa.Cnpj, Email = empresa.Email, DataCriacao = empresa.DataCriacao, Status = empresa.Status });
        }

        public async Task<ResultModel<List<EmpresaInformacoes>>> RetornarTodasEmpresas()
        {
             var listaEmpresas = await unitOfWork.EmpresaRepository.RetornarListaEmpresas();

            if (listaEmpresas is null)
                return ResultModel<List<EmpresaInformacoes>>.Erro("Sem Empresas Cadastradas");

            return ResultModel<List<EmpresaInformacoes>>.Sucesso(listaEmpresas);
        }

        public async Task<ResultModel<List<UserEmpresaInformacoes>>> RetornarUsuariosEmpresas(int id)
        {
            var empresa = await unitOfWork.EmpresaRepository.RetornarEmpresa(id);

            if (empresa == null)
                return ResultModel<List<UserEmpresaInformacoes>>.Erro("Empresa Não Encontrada");

            var listarUserLogin = await unitOfWork.UserLoginRepository.RetornarLoginsCriados();
            var users = await unitOfWork.UsuarioEmpresaRepository.RetornarUsuariosPorEmpresa(id);

            if (users == null)
                return ResultModel<List<UserEmpresaInformacoes>>.Erro("Nenhum usuário cadastrada");

            List<UserEmpresaInformacoes> listaUsers = [];

            var pesquisaLogin = listarUserLogin.ToDictionary(x => x.Id, x => x);

            foreach (var user in users)
            {
                if(pesquisaLogin.TryGetValue(user.Id, out var login))
                {
                    listaUsers.Add(new UserEmpresaInformacoes
                    {
                        Nome = user.Nome,
                        UserName = login.UserName,
                        Email = login.Email,
                        EmpresaId = user.EmpresaId,
                        EmpresaNome = empresa.Nome,
                        Status = user.Status
                    });
                }
            }

            return ResultModel<List<UserEmpresaInformacoes>>.Sucesso(listaUsers);
        }
    }
}