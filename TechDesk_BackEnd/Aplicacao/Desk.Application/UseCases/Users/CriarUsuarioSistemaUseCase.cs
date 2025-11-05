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
using Sistema_HelpDesk.Desk.Domain.Users.Enum;

namespace Sistema_HelpDesk.Desk.Application.UseCases.Users
{
    public class CriarUsuarioSistemaUseCase(IUnitOfWork unitOfWork) : ICriarUsuarioSistemaUseCase
    {
        public async Task<ResultModel> CriarUsuario(UserCriar usuario)
        {
            if (!usuario.UserName.Contains("."))
                return ResultModel.Erro("UserName deve seguir o padrão nome.sobrenome");

            if (await unitOfWork.UserLoginRepository.ConfirmarUserNameCadastrado(usuario.UserName))
                return ResultModel.Erro("UserName já cadastrado");

            if (!ValidacaoEmail.ValidarEmail(usuario.Email))
                return ResultModel.Erro("Email Inválido!");

            var result = ValidacaoSenha.Validador(usuario.Password);

            if (result.Error)
                return ResultModel.Erro(result.ErrorMessage);

            switch (usuario.TipoPerfil)
            {
                case TipoPerfil.administrador:

                    await unitOfWork.UserLoginRepository.CriarLogin(new UserLogin { UserName = usuario.UserName, Email = usuario.Email, TipoPerfil = TipoPerfil.administrador }, usuario.Password, "Administrador");

                    var loginAdm = await unitOfWork.UserLoginRepository.RetornarLogin(usuario.UserName);

                    unitOfWork.UsuarioSistemaRepository.AdicionarUsuario(new UsuarioSistema(loginAdm.Id, usuario.Nome));

                    await unitOfWork.MesasAtendimentosRepository.AdicionarAdministradorMesas(loginAdm.Id);

                    await unitOfWork.SaveChangesAsync();
                    break;

                case TipoPerfil.tecnico:

                    await unitOfWork.UserLoginRepository.CriarLogin(new UserLogin { UserName = usuario.UserName, Email = usuario.Email, TipoPerfil = TipoPerfil.tecnico }, usuario.Password, "Tecnico");

                    var loginTec = await unitOfWork.UserLoginRepository.RetornarLogin(usuario.UserName);

                    unitOfWork.UsuarioSistemaRepository.AdicionarUsuario(new UsuarioSistema(loginTec.Id, usuario.Nome));

                    foreach (var mesaAtendimento in usuario.MesasAtendimentoId)
                    {
                        var mesa = await unitOfWork.MesasAtendimentosRepository.RetornarMesaAtendimento(mesaAtendimento);
                        unitOfWork.MesasAtendimentosRepository.AdicionarTecnicoAMesa(new MesaTecnicos { TecnicoId = loginTec.Id, MesaAtendimentoId = mesa.Id });
                    }

                    await unitOfWork.SaveChangesAsync();
                    break;

                default:
                    return ResultModel.Erro("Erro ao realizar operação!");
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
