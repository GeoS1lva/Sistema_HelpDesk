using Sistema_HelpDesk.Desk.Application.CommomResult;
using Sistema_HelpDesk.Desk.Application.Contracts.UnitOfWork;
using Sistema_HelpDesk.Desk.Application.UseCases.Users.DTOs;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Net.Mail;
using Sistema_HelpDesk.Desk.Domain.Users.Entities;
using Sistema_HelpDesk.Desk.Domain.Mesa.Entities;
using Sistema_HelpDesk.Desk.Application.UseCases.ServiceDesks.DTOs;
using Sistema_HelpDesk.Desk.Application.UseCases.Users.Interface;
using Sistema_HelpDesk.Desk.Application.UseCases.Utils;

namespace Sistema_HelpDesk.Desk.Application.UseCases.Users
{
    public class CriarTecnicoUseCase(IUnitOfWork unitOfWork) : ICriarTecnicoUseCase
    {
        public async Task<ResultModel> CriarTecnico(UserCriar userTecnico)
        {
            if (!(await ValidarMesasAdicionadas(userTecnico.MesasAtendimentoId)))
                return ResultModel.Erro("Mesa de atendimento não encontrada!");

            if (!userTecnico.UserName.Contains("."))
                return ResultModel.Erro("UserName deve seguir o padrão nome.sobrenome");

            if (await unitOfWork.UserLoginRepository.ConfirmarUserNameCadastrado(userTecnico.UserName))
                return ResultModel.Erro("UserName já cadastrado");

            if (!ValidacaoEmail.ValidarEmail(userTecnico.Email))
                return ResultModel.Erro("Email Inválido!");

            await unitOfWork.UserLoginRepository.CriarLogin(new UserLogin { UserName = userTecnico.UserName, Email = userTecnico.Email, TipoPerfil = userTecnico.TipoPerfil }, userTecnico.Password, userTecnico.Role);

            var login = await unitOfWork.UserLoginRepository.RetornarLogin(userTecnico.UserName);

            unitOfWork.TecnicoRepository.AdicionarTecnico(new Tecnico(login.Id, userTecnico.Nome));

            await unitOfWork.SaveChangesAsync();

            var tecnico = await unitOfWork.TecnicoRepository.RetornarTecnico(login.Id);

            foreach (var mesaAtendimento in userTecnico.MesasAtendimentoId)
            {
                var mesa = await unitOfWork.MesasAtendimentosRepository.RetornarMesaAtendimento(mesaAtendimento);
                unitOfWork.MesasAtendimentosRepository.AdicionarTecnicoAMesa(new MesaTecnicos { TecnicoId = tecnico.Id, MesaAtendimentoId = mesa.Id });

                await unitOfWork.SaveChangesAsync();
            }

            return ResultModel.Sucesso();
        }

        private async Task<bool> ValidarMesasAdicionadas(List<int> listaMesas)
        {
            foreach(var mesa in listaMesas)
            {
                var result = await unitOfWork.MesasAtendimentosRepository.ConfirmarMesaCadastrada(mesa);
                 if(!result)
                    return false;

                break;
            }

            return true;
        }
    }
}
