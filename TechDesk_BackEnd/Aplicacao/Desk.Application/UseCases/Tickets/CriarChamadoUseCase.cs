using Sistema_HelpDesk.Desk.Application.CommomResult;
using Sistema_HelpDesk.Desk.Application.Contracts.UnitOfWork;
using Sistema_HelpDesk.Desk.Application.UseCases.Companies.DTOs;
using Sistema_HelpDesk.Desk.Application.UseCases.Tickets.DTOs;
using Sistema_HelpDesk.Desk.Application.UseCases.Tickets.Interface;
using Sistema_HelpDesk.Desk.Domain.Chamados.Entidades;
using Sistema_HelpDesk.Desk.Domain.Empresas.Entidades;
using Sistema_HelpDesk.Desk.Domain.Users.Entities;

namespace Sistema_HelpDesk.Desk.Application.UseCases.Tickets
{
    public class CriarChamadoUseCase(IUnitOfWork unitOfWork) : ICriarChamadoUseCase
    {
        public async Task<ResultModel<RetornarChamadoUsuarioEmpresa>> AbrirChamadoPainel(AbrirChamadoUsuarioEmpresa solicitacao, string userNameRequest)
        {
            var login = await unitOfWork.UserLoginRepository.RetornarLogin(userNameRequest);
            var usuarioEmpresa = await unitOfWork.UsuarioEmpresaRepository.RetornarUsuario(login.Id);
            var empresa = await unitOfWork.EmpresaRepository.RetornarEmpresa(usuarioEmpresa.EmpresaId);

            var categoria = await unitOfWork.CategoriaRepository.RetornarCategoria(solicitacao.CategoriaId);

            if (categoria == null)
                return ResultModel<RetornarChamadoUsuarioEmpresa>.Erro("Categoria não encontrada!");

            var novoChamado = new Chamado(solicitacao.Assunto, solicitacao.Descricao, categoria, usuarioEmpresa.EmpresaId, usuarioEmpresa.Id);

            unitOfWork.ChamadoRepository.AdicionarChamado(novoChamado);

            await unitOfWork.SaveChangesAsync();

            var retornarChamado = new RetornarChamadoUsuarioEmpresa
            {
                NumeroChamado = novoChamado.NumeroChamado,
                Assunto = solicitacao.Assunto,
                Descricao = solicitacao.Descricao,
                Categoria = categoria.Nome,
                Prioridade = categoria.Prioridade,
                DataCriacao = novoChamado.DataCriacao,
                Status = novoChamado.Status,
                Empresa = MapearEmpresa(empresa),
                Usuario = MapearUsuario(login, usuarioEmpresa)
            };

            return ResultModel<RetornarChamadoUsuarioEmpresa>.Sucesso(retornarChamado);
        }

        public async Task<ResultModel<RetornarChamadoUsuarioSistema>> AbrirChamadoSistema(AbrirChamadoUsuarioSistema solicitacao, string userNameRequest)
        {
            var login = await unitOfWork.UserLoginRepository.RetornarLogin(userNameRequest);
            var usuario = await unitOfWork.UsuarioSistemaRepository.RetornarUsuario(login.Id);

            var empresa = await unitOfWork.EmpresaRepository.RetornarEmpresa(solicitacao.EmpresaId);

            var categoria = await unitOfWork.CategoriaRepository.RetornarCategoria(solicitacao.CategoriaId);

            if (categoria == null)
                return ResultModel<RetornarChamadoUsuarioSistema>.Erro("Categoria não encontrada!");

            var novoChamado = new Chamado(solicitacao.Assunto, solicitacao.Descricao, categoria, solicitacao.EmpresaId, solicitacao.UsuarioEmpresaId, login.Id);

            unitOfWork.ChamadoRepository.AdicionarChamado(novoChamado);

            await unitOfWork.SaveChangesAsync();

            var usuarioEmpresa = await unitOfWork.UsuarioEmpresaRepository.RetornarUsuario(solicitacao.UsuarioEmpresaId);
            var loginEmpresa = await unitOfWork.UserLoginRepository.RetornarLogin(usuarioEmpresa.Id);

            var retornarChamado = new RetornarChamadoUsuarioSistema
            {
                NumeroChamado = novoChamado.NumeroChamado,
                Assunto = solicitacao.Assunto,
                Descricao = solicitacao.Descricao,
                Categoria = categoria.Nome,
                Prioridade = categoria.Prioridade,
                DataCriacao = novoChamado.DataCriacao,
                Status = novoChamado.Status,
                Empresa = MapearEmpresa(empresa),
                Usuario = MapearUsuario(loginEmpresa, usuarioEmpresa),
                NomeTecnicoAberturaChamado = usuario.Nome
            };

            return ResultModel<RetornarChamadoUsuarioSistema>.Sucesso(retornarChamado);
        }

        private static EmpresaInformacoes MapearEmpresa(Empresa empresa) => new()
        {
            Id = empresa.Id,
            Nome = empresa.Nome,
            Cnpj = empresa.Cnpj,
            Email = empresa.Email,
            DataCriacao = empresa.DataCriacao,
            Status = empresa.Status
        };

        private static UsuarioEmpresaChamadoDTO MapearUsuario(UserLogin login, UsuariosEmpresa usuario) => new()
        {
            Nome = usuario.Nome,
            Email = login.Email,
            UserName = login.UserName
        };
    }
}
