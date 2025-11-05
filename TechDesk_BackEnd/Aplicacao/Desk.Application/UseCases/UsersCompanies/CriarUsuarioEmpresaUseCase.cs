using Sistema_HelpDesk.Desk.Application.CommomResult;
using Sistema_HelpDesk.Desk.Application.Contracts.UnitOfWork;
using Sistema_HelpDesk.Desk.Application.UseCases.UsersCompanies.DTOs;
using Sistema_HelpDesk.Desk.Application.UseCases.UsersCompanies.Interface;
using Sistema_HelpDesk.Desk.Application.UseCases.Utils;
using Sistema_HelpDesk.Desk.Domain.Empresas.Entidades;
using Sistema_HelpDesk.Desk.Domain.Users.Entities;

namespace Sistema_HelpDesk.Desk.Application.UseCases.UsersCompanies
{
    public class CriarUsuarioEmpresaUseCase(IUnitOfWork unitOfWork) : ICriarUsuarioEmpresaUseCase
    {
        public async Task<ResultModel> CadastrarUsuarioEmpresa(UserEmpresaCriar userEmpresa)
        {
            if (await unitOfWork.UserLoginRepository.ConfirmarUserNameCadastrado(userEmpresa.UserName))
                return ResultModel.Erro("UserName já cadastrado");

            if (!userEmpresa.UserName.Contains("."))
                return ResultModel.Erro("UserName deve seguir o padrão nome.sobrenome");

            if (!ValidacaoEmail.ValidarEmail(userEmpresa.Email))
                return ResultModel.Erro("Email Inválido!");

            var result = ValidacaoSenha.Validador(userEmpresa.Password);

            if (result.Error)
                return ResultModel.Erro(result.ErrorMessage);

            await unitOfWork.UserLoginRepository.CriarLogin(new UserLogin { UserName = userEmpresa.UserName, Email = userEmpresa.Email, TipoPerfil = Domain.Users.Enum.TipoPerfil.usuario }, userEmpresa.Password, "Usuario");

            var login = await unitOfWork.UserLoginRepository.RetornarLogin(userEmpresa.UserName);

            unitOfWork.UsuarioEmpresaRepository.AdicionarUsuarioEmpresa(new UsuariosEmpresa(login.Id, userEmpresa.Nome, userEmpresa.EmpresaId));

            await unitOfWork.SaveChangesAsync();

            return ResultModel.Sucesso();
        }
    }
}
