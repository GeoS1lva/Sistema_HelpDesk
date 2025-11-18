using Microsoft.AspNetCore.Identity;
using Sistema_HelpDesk.Desk.Application.CommomResult;
using Sistema_HelpDesk.Desk.Application.Contracts.Security;
using Sistema_HelpDesk.Desk.Application.Contracts.UnitOfWork;
using Sistema_HelpDesk.Desk.Application.UseCases.Autenticação.DTOs;
using Sistema_HelpDesk.Desk.Application.UseCases.Authentication.DTOs;
using Sistema_HelpDesk.Desk.Application.UseCases.Users.DTOs;
using Sistema_HelpDesk.Desk.Domain.Users.Entities;
using Sistema_HelpDesk.Desk.Domain.Users.Enum;

namespace Sistema_HelpDesk.Desk.Application.UseCases.Autenticação
{
    public class AutheUseCase(IUnitOfWork unitOfWork, IJwtGerador jwt) : IAutheUseCase
    {
        public async Task<ResultModel<RetornoAutenticacao>> FazerLoginSistema(LoginUserAcessar dto)
        {
            var user = await unitOfWork.UserLoginRepository.RetornarLogin(dto.UserName);

            if (user is null)
                return ResultModel<RetornoAutenticacao>.Erro("Credenciais Inválidas.");

            if (await unitOfWork.UserLoginRepository.ConfirmarSituacaoUsuario(user))
                return ResultModel<RetornoAutenticacao>.Erro("Usuário Bloqueado!");

            if (user.TipoPerfil == TipoPerfil.usuario)
                return ResultModel<RetornoAutenticacao>.Erro("Credenciais Inválidas. Acesse o Portal do Cliente para abertura de chamado!");

            var result = await unitOfWork.UserLoginRepository.ConfirmarSenhaLogin(user, dto.Password);

            if (!result)
                return ResultModel<RetornoAutenticacao>.Erro("Credenciais Inválidas."); ;

            var roles = await unitOfWork.UserLoginRepository.RetornarPapeisUser(user.UserName);
            var token = jwt.GeradorJwt(user, roles);

            IdentidadeInformacoes identidade = new IdentidadeInformacoes
            {
                UserName = user.UserName,
                Email = user.Email,
                Role = roles[0]
            };

            return ResultModel<RetornoAutenticacao>.Sucesso(new RetornoAutenticacao
            {
                Token = token,
                identidadeInformacoes = identidade
            });
        }

        public async Task<ResultModel<RetornoAutenticacao>> FazerLoginPainel(LoginUserAcessar dto)
        {
            var user = await unitOfWork.UserLoginRepository.RetornarLogin(dto.UserName);

            if (user is null)
                return ResultModel<RetornoAutenticacao>.Erro("Credenciais Inválidas.");

            if (await unitOfWork.UserLoginRepository.ConfirmarSituacaoUsuario(user))
                return ResultModel<RetornoAutenticacao>.Erro("Usuário Bloqueado!");

            if (user.TipoPerfil != TipoPerfil.usuario)
                return ResultModel<RetornoAutenticacao>.Erro("Credenciais Inválidas.");

            var result = await unitOfWork.UserLoginRepository.ConfirmarSenhaLogin(user, dto.Password);

            if (!result)
                return ResultModel<RetornoAutenticacao>.Erro("Credenciais Inválidas."); ;

            var roles = await unitOfWork.UserLoginRepository.RetornarPapeisUser(user.UserName);
            var token = jwt.GeradorJwt(user, roles);

            IdentidadeInformacoes identidade = new IdentidadeInformacoes
            {
                UserName = user.UserName,
                Email = user.Email,
                Role = roles[0]
            };

            return ResultModel<RetornoAutenticacao>.Sucesso(new RetornoAutenticacao
            {
                Token = token,
                identidadeInformacoes = identidade
            });
        }
    }
}
