using Sistema_HelpDesk.Desk.Application.CommomResult;
using Sistema_HelpDesk.Desk.Application.Contracts.UnitOfWork;
using Sistema_HelpDesk.Desk.Application.UseCases.Users.DTOs;
using Sistema_HelpDesk.Desk.Application.UseCases.Users.Interface;
using Sistema_HelpDesk.Desk.Application.UseCases.Utils;

namespace Sistema_HelpDesk.Desk.Application.UseCases.Users
{
    public class AtualizarInformacoesUsuarioSistemaUseCase(IUnitOfWork unitOfWork) : IAtualizarInformacoesUsuarioSistemaUseCase
    {
        public async Task<ResultModel<UserInformacoes>> AtualizarDadosUsuarioSistema(UserLoginAtualizar user)
        {
            var usuarioLogin = await unitOfWork.UserLoginRepository.RetornarLogin(user.AntigoUserName);

            if (usuarioLogin == null)
                return ResultModel<UserInformacoes>.Erro("Usuario não encontrado!");

            var usuario = await unitOfWork.UsuarioSistemaRepository.RetornarUsuario(usuarioLogin.Id);

            if(user.NovoUserName != null)
            {
                if (!user.NovoUserName.Contains("."))
                    return ResultModel<UserInformacoes>.Erro("UserName deve seguir o padrão nome.sobrenome");

                if (await unitOfWork.UserLoginRepository.ConfirmarUserNameCadastrado(user.NovoUserName))
                    return ResultModel<UserInformacoes>.Erro("UserName já cadastrado");

                usuarioLogin.UserName = user.NovoUserName;
            }

            if(user.Email != null)
            {
                if (!ValidacaoEmail.ValidarEmail(user.Email))
                    return ResultModel<UserInformacoes>.Erro("Email Inválido!");

                usuarioLogin.Email = user.Email;
            }

            if(user.Password != null)
            {
                var result = ValidacaoSenha.Validador(user.Password);

                if (result.Error)
                    return ResultModel<UserInformacoes>.Erro(result.ErrorMessage);

                await unitOfWork.UserLoginRepository.TrocarSenhaUserLogin(usuarioLogin, user.Password);
            }

            if(user.Nome != null)
                usuario.Nome = user.Nome;

            var resultado = await unitOfWork.UserLoginRepository.AtualizarUserLogin(usuarioLogin);
            await unitOfWork.SaveChangesAsync();

            return ResultModel<UserInformacoes>.Sucesso(new UserInformacoes
            {
                Nome = usuario.Nome,
                Email = usuarioLogin.Email,
                UserName = usuarioLogin.UserName,
                Role = await unitOfWork.UserLoginRepository.RetornarPapeisUser(usuarioLogin.UserName),
                Mesas = await unitOfWork.MesasAtendimentosRepository.RetornarListaMesaTecnico(usuarioLogin.Id),
                Status = usuario.Status
            });
        }
    }
}
