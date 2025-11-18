using Sistema_HelpDesk.Desk.Application.CommomResult;
using Sistema_HelpDesk.Desk.Application.Contracts.UnitOfWork;
using Sistema_HelpDesk.Desk.Application.UseCases.Tickets.DTOs;
using Sistema_HelpDesk.Desk.Application.UseCases.Tickets.Interface;
using Sistema_HelpDesk.Desk.Domain.Users.Enum;

namespace Sistema_HelpDesk.Desk.Application.UseCases.Tickets
{
    public class RetornarInformacoesChamadosUseCase(IUnitOfWork unitOfWork) : IRetornarInformacoesChamadosUseCase
    {
        public async Task<ResultModel<List<InformacoesChamadoKanban>>> RetornarInformacoesKanban()
        {
            var chamados = await unitOfWork.ChamadoRepository.RetornarChamados();

            if (chamados.Count == 0)
                return ResultModel<List<InformacoesChamadoKanban>>.Erro("Sem Chamados em Aberto!");

            List<InformacoesChamadoKanban> informacoesChamados = [];

            foreach (var item in chamados)
            {
                var categoria = await unitOfWork.CategoriaRepository.RetornarCategoria(item.CategoriaId);

                var info = new InformacoesChamadoKanban
                {
                    NumeroChamado = item.NumeroChamado,
                    Assunto = item.Assunto,
                    Prioridade = item.SubCategoria.Prioridade,
                    SLA = item.StatusSLA,
                    Status = item.Status,
                    NomeEmpresa = item.Empresa.Nome,
                    NomeMesaAtendimento = item.MesaAtendimento.Nome,
                    NomeTecnicoResponsavel = item.TecnicoResponsavel?.Nome ?? "Não Atribuído"
                };

                informacoesChamados.Add(info);
            }

            return ResultModel<List<InformacoesChamadoKanban>>.Sucesso(informacoesChamados);
        }

        public async Task<ResultModel<List<InformacoesChamadoKanban>>> RetornarInformacoesChamadosUsuarioEmpresa(string userRequest)
        {
            var userLogin = await unitOfWork.UserLoginRepository.RetornarLogin(userRequest);

            var chamados = await unitOfWork.ChamadoRepository.RetornarChamadosUsuarioEmpresa(userLogin.Id);

            if (chamados.Count == 0)
                return ResultModel<List<InformacoesChamadoKanban>>.Erro("Sem Chamados em Aberto!");

            List<InformacoesChamadoKanban> informacoesChamados = [];

            foreach (var item in chamados)
            {
                var categoria = await unitOfWork.CategoriaRepository.RetornarCategoria(item.CategoriaId);

                var info = new InformacoesChamadoKanban
                {
                    NumeroChamado = item.NumeroChamado,
                    Assunto = item.Assunto,
                    Prioridade = item.SubCategoria.Prioridade,
                    SLA = item.StatusSLA,
                    Status = item.Status,
                    NomeEmpresa = item.Empresa.Nome,
                    NomeMesaAtendimento = item.MesaAtendimento.Nome,
                    NomeTecnicoResponsavel = item.TecnicoResponsavel?.Nome ?? "Não Atribuído"
                };

                informacoesChamados.Add(info);
            }

            return ResultModel<List<InformacoesChamadoKanban>>.Sucesso(informacoesChamados);
        }

        public async Task<ResultModel<RetornarDetalhesChamadosUsuarioEmpresa>> RetornarChamadoUsuarioEmpresa(string numeroChamado)
        {
            var chamado = await unitOfWork.ChamadoRepository.RetornarChamadoUsuarioEmpresa(numeroChamado);

            if (chamado == null)
                return ResultModel<RetornarDetalhesChamadosUsuarioEmpresa>.Erro("Chamado não encontrado!");

            var categoria = await unitOfWork.CategoriaRepository.RetornarCategoria(chamado.CategoriaId);

            var info = new RetornarDetalhesChamadosUsuarioEmpresa
            {
                NumeroChamado = chamado.NumeroChamado,
                CategoriaNome = categoria.Nome,
                SubCategoriaNome = chamado.SubCategoria.Nome,
                MesaAtendimento = chamado.MesaAtendimento.Nome,
                TecnicoResponsavelNome = chamado.TecnicoResponsavel?.Nome ?? "Não Atribuído",
                TempoAtendimentoTotal = chamado.TempoGastoTotalAtendimento,
                StatusSla = chamado.StatusSLA,
                Status = chamado.Status
            };

            return ResultModel<RetornarDetalhesChamadosUsuarioEmpresa>.Sucesso(info);
        }

        public async Task<ResultModel<RetornarQuantidadeChamado>> RetornarQuantidadeChamadoPorUsuarioEmpresa(string userRequest)
        {
            var usuarioLogin = await unitOfWork.UserLoginRepository.RetornarLogin(userRequest);

            if (usuarioLogin == null)
                return ResultModel<RetornarQuantidadeChamado>.Erro("Erro, usuário não encontrado!");

            if (usuarioLogin.TipoPerfil != TipoPerfil.usuario)
                return ResultModel<RetornarQuantidadeChamado>.Erro("Somente Usuários de Empresa tem acesso");

            var abertos = await unitOfWork.ChamadoRepository.RetornarQuantidadeChamadoAbertoPorUserEmpresa(usuarioLogin.Id);
            var pausados = await unitOfWork.ChamadoRepository.RetornarQuantidadeChamadoPausadoPorUserEmpresa(usuarioLogin.Id);
            var finalizados = await unitOfWork.ChamadoRepository.RetornarQuantidadeChamadoFinalizadoPorUserEmpresa(usuarioLogin.Id);

            return ResultModel<RetornarQuantidadeChamado>.Sucesso(new RetornarQuantidadeChamado
            {
                ChamadosAbertos = abertos,
                ChamadosPausados = pausados,
                ChamadosFinalizados = finalizados
            });
        }
    }
}
