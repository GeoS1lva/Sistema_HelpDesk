using Sistema_HelpDesk.Desk.Application.CommomResult;
using Sistema_HelpDesk.Desk.Application.Contracts.UnitOfWork;
using Sistema_HelpDesk.Desk.Application.UseCases.ServiceDesks.DTOs;
using Sistema_HelpDesk.Desk.Application.UseCases.Tickets.DTOs;
using Sistema_HelpDesk.Desk.Application.UseCases.Tickets.Interface;
using Sistema_HelpDesk.Desk.Domain.Chamados.Entidades;
using Sistema_HelpDesk.Desk.Domain.Chamados.Entities;
using Sistema_HelpDesk.Desk.Domain.Chamados.Enums;
using Sistema_HelpDesk.Desk.Domain.Users.Enum;

namespace Sistema_HelpDesk.Desk.Application.UseCases.Tickets
{
    public class AcoesChamadoUseCase(IUnitOfWork unitOfWork) : IAcoesChamadoUseCase
    {
        public async Task<ResultModel<RetornoAtendimentoDto>> PlayChamado(string numeroChamado, string userRequest)
        {
            var chamado = await unitOfWork.ChamadoRepository.RetornarChamado(numeroChamado);
            var userLogin = await unitOfWork.UserLoginRepository.RetornarLogin(userRequest);

            if (chamado == null)
                return ResultModel<RetornoAtendimentoDto>.Erro("Chamado não encontrado!");

            if (chamado.Status == Status.finalizado)
                return ResultModel<RetornoAtendimentoDto>.Erro("Não é possível iniciar o atendimento pois o chamado já foi finalizado!");

            if (chamado.Status == Status.emAndamento)
                return ResultModel<RetornoAtendimentoDto>.Erro("Não é possível iniciar o atendimento pois esse chamado já está em Andamento!");

            if (userLogin == null)
                return ResultModel<RetornoAtendimentoDto>.Erro("Usuario não encontrado!");

            var apontamento = new ApontamentoHorasChamado(chamado.Id, userLogin.Id);

            chamado.Apontamentos.Add(apontamento);

            var user = await unitOfWork.UsuarioSistemaRepository.RetornarUsuario(userLogin.Id);

            chamado.AlterarTecnicoResponsavel(user);

            unitOfWork.ApontamentosChamadosRepository.AdicionarApontamentoChamado(apontamento);
            chamado.Play();

            var retornoAtendimento = new RetornoAtendimento(chamado, RetornoAcaoChamaoEnum.iniciarAtendimento, user.Nome, "Atendimento Iniciado");

            unitOfWork.RetornoAtendimentoRepository.AdicionarRetornoAtendimento(retornoAtendimento);

            await unitOfWork.SaveChangesAsync();

            return ResultModel<RetornoAtendimentoDto>.Sucesso(new RetornoAtendimentoDto
            {
                NumeroChamado = retornoAtendimento.NumeroChamado,
                RetornoAcao = retornoAtendimento.RetornoAcao,
                UsuarioNome = retornoAtendimento.NomeTecnico,
                HoraAtendimento = retornoAtendimento.HoraAtendimento,
                Mensagem = retornoAtendimento.Mensagem
            });
        }
                                                                                                                                                                  
        public async Task<ResultModel<RetornoAtendimentoDto>> PausarChamado(PausaChamado pausaChamado, string userRequest)
        {
            var chamado = await unitOfWork.ChamadoRepository.RetornarChamado(pausaChamado.NumeroChamado);
            var userLogin = await unitOfWork.UserLoginRepository.RetornarLogin(userRequest);

            if (chamado == null)
                return ResultModel<RetornoAtendimentoDto>.Erro("Chamado não encontrado!");

            if (chamado.Status == Status.finalizado)
                return ResultModel<RetornoAtendimentoDto>.Erro("Não é possível pausar o atendimento pois o chamado já foi finalizado!");

            if (chamado.Status == Status.pausado)
                return ResultModel<RetornoAtendimentoDto>.Erro("Não é possível realizar a operação, o atendimento já foi pausado!");

            if (chamado.Status == Status.aberto)
                return ResultModel<RetornoAtendimentoDto>.Erro("Não é possível pausar o atendimento, o chamado precisa ser iniciado antes!");

            var apontamentoAtivo = await unitOfWork.ApontamentosChamadosRepository.RetornarApontamentoAtivo(chamado.Id);

            if (apontamentoAtivo == null)
                return ResultModel<RetornoAtendimentoDto>.Erro("Não foi possível localicar um Apontamento em Andamento!");

            if (apontamentoAtivo.TecnicoId != userLogin.Id && userLogin.TipoPerfil == TipoPerfil.tecnico)
                return ResultModel<RetornoAtendimentoDto>.Erro("Somente o Tecnico que está com o chamado em andamento pode pausar o mesmo ou o Administrador!!");

            apontamentoAtivo.EncerrarApontamento(pausaChamado.Comentario);
            var totalTempo = apontamentoAtivo.CalcularTicksTotal();

            var userApontamento = await unitOfWork.UsuarioSistemaRepository.RetornarUsuario(apontamentoAtivo.TecnicoId);
            userApontamento.AcrescentarHorasTotalAtendimento(totalTempo);

            chamado.Pausar();
            chamado.CalcularTempoGasto(totalTempo);

            var usuarioComentario = await unitOfWork.UsuarioSistemaRepository.RetornarUsuario(userLogin.Id);

            var retornoAtendimento = new RetornoAtendimento(chamado, RetornoAcaoChamaoEnum.pausarAtendimento, usuarioComentario.Nome, pausaChamado.Comentario);
            unitOfWork.RetornoAtendimentoRepository.AdicionarRetornoAtendimento(retornoAtendimento);

            await unitOfWork.SaveChangesAsync();

            return ResultModel<RetornoAtendimentoDto>.Sucesso(new RetornoAtendimentoDto
            {
                NumeroChamado = retornoAtendimento.NumeroChamado,
                RetornoAcao = retornoAtendimento.RetornoAcao,
                UsuarioNome = retornoAtendimento.NomeTecnico,
                HoraAtendimento = retornoAtendimento.HoraAtendimento,
                Mensagem = retornoAtendimento.Mensagem
            });
        }

        public async Task<ResultModel<RetornoAtendimentoDto>> FinalizarChamado(FinalizarChamado finalizarChamado, string userRequest)
        {
            var chamado = await unitOfWork.ChamadoRepository.RetornarChamado(finalizarChamado.NumeroChamado);
            var userLogin = await unitOfWork.UserLoginRepository.RetornarLogin(userRequest);

            if (chamado == null)
                return ResultModel<RetornoAtendimentoDto>.Erro("Chamado não encontrado!");

            if (chamado.Status == Status.finalizado)
                return ResultModel<RetornoAtendimentoDto>.Erro("Não foi possível realizar a operação, o chamado já foi finalizado!");

            switch (chamado.Status)
            {
                case Status.emAndamento:
                    var apontamentoAtivo = await unitOfWork.ApontamentosChamadosRepository.RetornarApontamentoAtivo(chamado.Id);

                    if (apontamentoAtivo.TecnicoId != userLogin.Id && userLogin.TipoPerfil == TipoPerfil.tecnico)
                        return ResultModel<RetornoAtendimentoDto>.Erro("Somente o Tecnico que está com o chamado em andamento pode finalizar o mesmo ou o Administrador!");

                    var totalTempo = apontamentoAtivo.CalcularTicksTotal();

                    chamado.TecnicoResponsavel.AcrescentarHorasTotalAtendimento(totalTempo);

                    chamado.Finalizar(finalizarChamado.ComentarioFinalizacao);
                    chamado.CalcularTempoGasto(totalTempo);
                    break;
                case Status.pausado:
                    if (chamado.TecnicoResponsavelId != userLogin.Id && userLogin.TipoPerfil == TipoPerfil.tecnico)
                        return ResultModel<RetornoAtendimentoDto>.Erro("Somente o Tecnico que está responsável pelo chamadopode finalizar o mesmo ou o Administrador!");

                    chamado.Finalizar(finalizarChamado.ComentarioFinalizacao);
                    break;
                default:
                    return ResultModel<RetornoAtendimentoDto>.Erro("Erro ao realizar operação!");
            }

            var usuarioComentario = await unitOfWork.UsuarioSistemaRepository.RetornarUsuario(userLogin.Id);

            var retornoAtendimento = new RetornoAtendimento(chamado, RetornoAcaoChamaoEnum.finalizarAtendimento, usuarioComentario.Nome, finalizarChamado.ComentarioFinalizacao);
            unitOfWork.RetornoAtendimentoRepository.AdicionarRetornoAtendimento(retornoAtendimento);

            await unitOfWork.SaveChangesAsync();

            return ResultModel<RetornoAtendimentoDto>.Sucesso(new RetornoAtendimentoDto
            {
                NumeroChamado = retornoAtendimento.NumeroChamado,
                RetornoAcao = retornoAtendimento.RetornoAcao,
                UsuarioNome = retornoAtendimento.NomeTecnico,
                HoraAtendimento = retornoAtendimento.HoraAtendimento,
                Mensagem = retornoAtendimento.Mensagem
            });
        }

        public async Task<ResultModel<RetornoAtendimentoDto>> AdicionarComentarioChamado(AdicionarComentario comentario, string userRequest)
        {
            var userLogin = await unitOfWork.UserLoginRepository.RetornarLogin(userRequest);
            var chamado = await unitOfWork.ChamadoRepository.RetornarChamado(comentario.NumeroChamado);

            RetornoAtendimento retornoAtendimento = new RetornoAtendimento();

            switch (userLogin.TipoPerfil)
            {
                case TipoPerfil.administrador:
                    var userAdmin = await unitOfWork.UsuarioSistemaRepository.RetornarUsuario(userLogin.Id);

                    retornoAtendimento = new RetornoAtendimento(chamado, RetornoAcaoChamaoEnum.comentario, userAdmin.Nome, comentario.Comentario);

                    break;
                case TipoPerfil.tecnico:
                    var userTecnico = await unitOfWork.UsuarioSistemaRepository.RetornarUsuario(userLogin.Id);

                    retornoAtendimento = new RetornoAtendimento(chamado, RetornoAcaoChamaoEnum.comentario, userTecnico.Nome, comentario.Comentario);

                    break;
                case TipoPerfil.usuario:
                    var userEmpresa = await unitOfWork.UsuarioEmpresaRepository.RetornarUsuario(userLogin.Id);

                    retornoAtendimento = new RetornoAtendimento(chamado, RetornoAcaoChamaoEnum.comentario, userEmpresa.Nome, comentario.Comentario);

                    break;
            }

            unitOfWork.RetornoAtendimentoRepository.AdicionarRetornoAtendimento(retornoAtendimento);
            await unitOfWork.SaveChangesAsync();

            return ResultModel<RetornoAtendimentoDto>.Sucesso(new RetornoAtendimentoDto
            {
                NumeroChamado = retornoAtendimento.NumeroChamado,
                RetornoAcao = retornoAtendimento.RetornoAcao,
                UsuarioNome = retornoAtendimento.NomeTecnico,
                HoraAtendimento = retornoAtendimento.HoraAtendimento,
                Mensagem = retornoAtendimento.Mensagem
            });
        }

        public async Task<List<RetornoAtendimentoDto>> RetornoTodasAcoesChamado(string numeroChamado)
        {
            var acoesChamados = await unitOfWork.RetornoAtendimentoRepository.RetornoAcoesChamados(numeroChamado);

            return acoesChamados;
        }
    }
}
