using Humanizer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using Sistema_HelpDesk.Desk.Application.CommomResult;
using Sistema_HelpDesk.Desk.Application.Contracts.UnitOfWork;
using Sistema_HelpDesk.Desk.Application.UseCases.Users.DTOs;
using Sistema_HelpDesk.Desk.Application.UseCases.Utils;
using Sistema_HelpDesk.Desk.Domain.Users.Entities;
using Sistema_HelpDesk.Desk.Infra.Email;
using System.Text;

namespace Sistema_HelpDesk.Desk.Application.UseCases.Users.ResetPasswords
{
    public class ResetarSenhaUseCase(IUnitOfWork unitOfWork, IEmailSender emailSender) : IResetarSenhaUseCase
    {
        public async Task<ResultModel> SolicitarRedefinicaoSenha(UserResetPasswords userReset)
        {
            var user = await unitOfWork.UserLoginRepository.RetornarLogin(userReset.UserName);

            if (user == null)
                return ResultModel.Erro("Usuário não encontrado!");

            if (user.Email != userReset.Email)
                return ResultModel.Erro("E-mail Incorreto!");

            var tokenReset = await unitOfWork.UserLoginRepository.TokenResetSenha(user);
            var tokenEncoded = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(tokenReset));

            var conc = new Dictionary<string, string>
            {
                {"token", tokenEncoded },
                {"userName", userReset.UserName }
            };

            var mensagem = QueryHelpers.AddQueryString("http://localhost:7181/", conc);
            await emailSender.SendEmailAsync(user.Email, "Resete de senha", mensagem);

            return ResultModel.Sucesso();
        }

        public async Task<ResultModel<string>> ResetarSenha(UserTokenResetPassword userRequest)
        {
            var userLogin = await unitOfWork.UserLoginRepository.RetornarLogin(userRequest.UserName);

            if (userLogin == null)
                return ResultModel<string>.Erro("Usuário não encontrado!");

            var tokenBytes = WebEncoders.Base64UrlDecode(userRequest.Token);
            var tokenNormal = Encoding.UTF8.GetString(tokenBytes);

            var result = ValidacaoSenha.Validador(userRequest.Password);

            if (result.Error)
                return ResultModel<string>.Erro(result.ErrorMessage);

            var resultAlteracao = await unitOfWork.UserLoginRepository.ResetarSenha(userLogin, tokenNormal, userRequest.Password);

            if (!resultAlteracao.Succeeded)
            {
                var erros = string.Join(" | ", resultAlteracao.Errors.Select(e => e.Description));
                return ResultModel<string>.Erro("Erro ao redefinir senha: " + erros);
            }

            return ResultModel<string>.Sucesso("A senha foi alterada corretamente!");
        }
    }
}
