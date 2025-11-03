using Sistema_HelpDesk.Desk.Application.CommomResult;
using Sistema_HelpDesk.Desk.Application.Contracts.UnitOfWork;
using Sistema_HelpDesk.Desk.Application.UseCases.Users.DTOs;
using Sistema_HelpDesk.Desk.Application.UseCases.Users.Interface;
using Sistema_HelpDesk.Desk.Domain.Users.Enum;
using System.Collections.Generic;

namespace Sistema_HelpDesk.Desk.Application.UseCases.Users
{
    public class RetornarInformacoesUseCase(IUnitOfWork unitOfWork) : IRetornarInformacoesUseCase
    {
        public async Task<ResultModel<UserInformacoes>> RetornarInformacoesUsuario(string UserName)
        {
            var infoLogin = await unitOfWork.UserLoginRepository.RetornarLogin(UserName);
            if (infoLogin == null)
                return ResultModel<UserInformacoes>.Erro("Usuário não encontrado!");

            var infoTecnico = await unitOfWork.UsuarioEmpresaRepository.RetornarUsuario(infoLogin.Id);

            if (infoTecnico == null && infoLogin.TipoPerfil != TipoPerfil.administrador)
                return ResultModel<UserInformacoes>.Erro("Perfil Tecnico do Usuario não encontrado!");

            return ResultModel<UserInformacoes>.Sucesso(new UserInformacoes
            {
                Nome = infoTecnico.Nome,
                Email = infoLogin.Email,
                UserName = infoLogin.UserName,
                Role = await unitOfWork.UserLoginRepository.RetornarPapeisUser(infoLogin.UserName),
                Mesas = await unitOfWork.MesasAtendimentosRepository.RetornarListaMesaTecnico(infoTecnico.Id),
                Status = infoTecnico.Status
            });
        }
        
        public async Task<ResultModel<List<UserInformacoes>>> RetornarInformacoesUsuariosCriado()
        {
            var listarUserLogin = await unitOfWork.UserLoginRepository.RetornarLoginsCriados();
            var tecnicosInformacoes = await unitOfWork.UsuarioSistemaRepository.RetornarUsuarios();

            if (listarUserLogin == null || tecnicosInformacoes == null)
                return ResultModel<List<UserInformacoes>>.Erro("Nenhum Usuário Cadastrado!");

            List<UserInformacoes> userInformacoes = [];
            var pesquisarLogin = listarUserLogin.ToDictionary(x => x.Id, x => x);

            foreach (var tecnico in tecnicosInformacoes)
            {
                if (pesquisarLogin.TryGetValue(tecnico.Id, out var login))
                {
                    var role = await unitOfWork.UserLoginRepository.RetornarPapeisUser(login.UserName);
                    var mesas = await unitOfWork.MesasAtendimentosRepository.RetornarListaMesaTecnico(tecnico.Id);

                    userInformacoes.Add(new UserInformacoes
                    {
                        Nome = tecnico.Nome,
                        Email = login.Email,
                        UserName = login.UserName,
                        Role = role,
                        Mesas = mesas,
                        Status = tecnico.Status
                    });
                }
            }

            return ResultModel<List<UserInformacoes>>.Sucesso(userInformacoes);
        }
    }
}
