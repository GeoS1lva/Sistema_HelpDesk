using Microsoft.EntityFrameworkCore;
using Sistema_HelpDesk.Desk.Application.Contracts.Repositories;
using Sistema_HelpDesk.Desk.Application.UseCases.ServiceDesks.DTOs;
using Sistema_HelpDesk.Desk.Domain.Mesa.Entities;
using Sistema_HelpDesk.Desk.Domain.Users.Entities;
using Sistema_HelpDesk.Desk.Infra.Context;

namespace Sistema_HelpDesk.Desk.Infra.Persistence.Repositories
{
    public class MesasAtendimentoRepository(SqlServerIdentityDbContext context) : IMesasAtendimentosRepository
    {
        private readonly SqlServerIdentityDbContext _context = context;

        public void AdicionarMesaAtendimento(MesaAtendimento mesa)
            => _context.MesasAtendimento.Add(mesa);
        public void AdicionarTecnicoAMesa(MesaTecnicos relacionamento)
            => _context.MesaTecnicos.Add(relacionamento);

        public async Task AdicionarAdministradorMesas(int administradorId)
        {
            var list = await _context.MesasAtendimento.ToListAsync();

            foreach (var item in list)
            {
                _context.MesaTecnicos.Add(new MesaTecnicos { MesaAtendimentoId = item.Id, TecnicoId = administradorId});
            }
        }

        public async Task RemoverMesaAtendimento(MesaAtendimento mesa)
        {
            await RemoverRelacionamentosMesa(mesa.Id);
            _context.MesasAtendimento.Remove(mesa);
        }

        public async Task<bool> ConfirmarMesaCadastrada(int id)
            => await _context.MesasAtendimento.AnyAsync(x => x.Id == id);
        public async Task<bool> ConfirmarMesaCadastrada(string nome)
            => await _context.MesasAtendimento.AnyAsync(x => x.Nome == nome);

        public async Task<MesaAtendimento?> RetornarMesaAtendimento(int id)
            => await _context.MesasAtendimento.FirstOrDefaultAsync(x => x.Id == id);

        public async Task<List<MesaAtendimentoInformacoes>> RetornarListaMesasAtendimento()
            => await _context.MesasAtendimento
                             .Select(x => new MesaAtendimentoInformacoes { id = x.Id, Nome = x.Nome })
                             .ToListAsync();

        public async Task<List<string>> RetornarListaMesaTecnico(int idUser)
        {
            List<string> listaNomesMesa = [];
            var lista = await _context.MesaTecnicos.Where(x => x.TecnicoId == idUser).ToListAsync();

            foreach (var item in lista)
            {
                var mesa = await _context.MesasAtendimento.FirstOrDefaultAsync(x => x.Id == item.MesaAtendimentoId);
                listaNomesMesa.Add(mesa.Nome);
            }

            return listaNomesMesa;
        }

        public async Task RemoverRelacionamento(int tecnicoId)
            => await _context.MesaTecnicos.Where(x => x.TecnicoId == tecnicoId).ExecuteDeleteAsync();

        public async Task RemoverRelacionamentosMesa(int mesaId)
            => await _context.MesaTecnicos.Where(x => x.MesaAtendimentoId == mesaId).ExecuteDeleteAsync();
    }
}
