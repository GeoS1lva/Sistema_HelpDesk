using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using Sistema_HelpDesk.Desk.Application.Users.Interface;
using Sistema_HelpDesk.Desk.Domain.Models;
using Sistema_HelpDesk.Desk.Domain.Users;
using Sistema_HelpDesk.Desk.Domain.Users.ResetarSenhaUser;
using Sistema_HelpDesk.Desk.Infra.Email;

namespace Sistema_HelpDesk.Desk.Application.Users.Service.RedefinirSenha
{
    public class ResetarSenhaService(UserManager<UserLogin> repository, IEmailSender emailSender) : IResetarSenhaService
    {
        private readonly UserManager<UserLogin> Repository = repository;
        private readonly IEmailSender _emailSender = emailSender;

        public async Task<ResultModel> SolicitarRedefinicaoSenha(UserResetSenha userReset)
        {
            var user = await Repository.FindByNameAsync(userReset.UserName);

            if(user==null)
                return ResultModel.Erro("Usuário não encontrado!");

            if(user.Email!=userReset.Email)
                return ResultModel.Erro("E-mail Incorreto!");

            var tokenReset = await Repository.GeneratePasswordResetTokenAsync(user);
            var conc = new Dictionary<string, string>
            {
                {"token", tokenReset },
                {"email", userReset.Email }
            };

            var mensagem = QueryHelpers.AddQueryString("http://localhost:7181/", conc);
            await _emailSender.SendEmailAsync(user.Email, "Resete de senha", mensagem);

            return ResultModel.Sucesso();
        }
    }
}
