using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using Sistema_HelpDesk.Desk.Application.CommomResult;
using Sistema_HelpDesk.Desk.Application.Contracts.UnitOfWork;
using Sistema_HelpDesk.Desk.Application.UseCases.Users.DTOs;
using Sistema_HelpDesk.Desk.Domain.Users.Entities;
using Sistema_HelpDesk.Desk.Infra.Email;

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
            var conc = new Dictionary<string, string>
            {
                {"token", tokenReset },
                {"email", userReset.Email }
            };

            var mensagem = QueryHelpers.AddQueryString("http://localhost:7181/", conc);
            await emailSender.SendEmailAsync(user.Email, "Resete de senha", mensagem);

            return ResultModel.Sucesso();
        }
    }
}
