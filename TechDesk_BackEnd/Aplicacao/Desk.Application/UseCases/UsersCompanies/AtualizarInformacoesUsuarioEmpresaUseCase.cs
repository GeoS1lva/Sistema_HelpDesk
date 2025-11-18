using Sistema_HelpDesk.Desk.Application.CommomResult;
using Sistema_HelpDesk.Desk.Application.Contracts.UnitOfWork;
using Sistema_HelpDesk.Desk.Application.UseCases.Users.DTOs;
using Sistema_HelpDesk.Desk.Application.UseCases.UsersCompanies.DTOs;
using Sistema_HelpDesk.Desk.Application.UseCases.UsersCompanies.Interface;
using Sistema_HelpDesk.Desk.Application.UseCases.Utils;
using Sistema_HelpDesk.Desk.Infra.Persistence.UnitOfWork;

namespace Sistema_HelpDesk.Desk.Application.UseCases.UsersCompanies
{
    public class AtualizarInformacoesUsuarioEmpresaUseCase(IUnitOfWork unitOfWork) : IAtualizarInformacoesUsuarioEmpresaUseCase
    {
        public async Task<ResultModel<UserEmpresaInformacoes>> AtualizarDadosUsuarioEmpresa(UserLoginAtualizar user)
        {
            var usuarioLogin = await unitOfWork.UserLoginRepository.RetornarLogin(user.AntigoUserName);

            if (usuarioLogin == null)
                return ResultModel<UserEmpresaInformacoes>.Erro("Usuario não encontrado!");

            var usuario = await unitOfWork.UsuarioEmpresaRepository.RetornarUsuario(usuarioLogin.Id);

            if (user.NovoUserName != null)
            {
                if (!user.NovoUserName.Contains("."))
                    return ResultModel<UserEmpresaInformacoes>.Erro("UserName deve seguir o padrão nome.sobrenome");

                if (await unitOfWork.UserLoginRepository.ConfirmarUserNameCadastrado(user.NovoUserName))
                    return ResultModel<UserEmpresaInformacoes>.Erro("UserName já cadastrado");

                usuarioLogin.UserName = user.NovoUserName;
            }

            if (user.Email != null)
            {
                if (!ValidacaoEmail.ValidarEmail(user.Email))
                    return ResultModel<UserEmpresaInformacoes>.Erro("Email Inválido!");

                usuarioLogin.Email = user.Email;
            }

            if (user.Password != null)
            {
                var result = ValidacaoSenha.Validador(user.Password);

                if (result.Error)
                    return ResultModel<UserEmpresaInformacoes>.Erro(result.ErrorMessage);

                await unitOfWork.UserLoginRepository.TrocarSenhaUserLogin(usuarioLogin, user.Password);
            }

            if (user.Nome != null)
                usuario.Nome = user.Nome;

            var resultado = await unitOfWork.UserLoginRepository.AtualizarUserLogin(usuarioLogin);
            await unitOfWork.SaveChangesAsync();

            return ResultModel<UserEmpresaInformacoes>.Sucesso(new UserEmpresaInformacoes
            {
                Nome = usuario.Nome,
                UserName = usuarioLogin.UserName,
                Email = usuarioLogin.Email,
                EmpresaId = usuario.EmpresaId,
                EmpresaNome = usuario.Empresa.Nome,
                Status = usuario.Status
            });
        }
    }
}
