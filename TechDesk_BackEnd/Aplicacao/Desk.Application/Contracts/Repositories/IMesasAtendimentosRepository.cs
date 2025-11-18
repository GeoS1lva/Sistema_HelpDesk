using Sistema_HelpDesk.Desk.Application.UseCases.ServiceDesks.DTOs;
using Sistema_HelpDesk.Desk.Domain.Mesa.Entities;

namespace Sistema_HelpDesk.Desk.Application.Contracts.Repositories
{
    public interface IMesasAtendimentosRepository
    {
        public void AdicionarMesaAtendimento(MesaAtendimento mesa);
        public void AdicionarTecnicoAMesa(MesaTecnicos relacionamento);
        public Task AdicionarAdministradorMesas(int administradorId);
        public Task RemoverMesaAtendimento(MesaAtendimento mesa);
        public Task<bool> ConfirmarMesaCadastrada(int id);
        public Task<bool> ConfirmarMesaCadastrada(string nome);
        public Task<MesaAtendimento?> RetornarMesaAtendimento(int? id);
        public Task<List<MesaAtendimentoInformacoes>> RetornarListaMesasAtendimento();
        public Task<List<string>> RetornarListaMesaTecnico(int IdUser);
        public Task RemoverRelacionamento(int tecnicoId);
        public Task RemoverRelacionamentosMesa(int mesaId);
    }
}
