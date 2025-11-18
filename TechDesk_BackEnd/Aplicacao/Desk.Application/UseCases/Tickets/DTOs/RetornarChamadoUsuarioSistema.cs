using Sistema_HelpDesk.Desk.Application.UseCases.Category.DTOs;
using Sistema_HelpDesk.Desk.Application.UseCases.Companies.DTOs;
using Sistema_HelpDesk.Desk.Domain.Chamados.Enums;

namespace Sistema_HelpDesk.Desk.Application.UseCases.Tickets.DTOs
{
    public class RetornarChamadoUsuarioSistema
    {
        public string NumeroChamado { get; set; }
        public string Assunto { get; set; }
        public string Descricao { get; set; }
        public CategoriaInformacoesDTO Categoria { get; set; }
        public string NomeMesaAtendimento { get; set; }
        public DateTime DataCriacao { get; set; }
        public Status Status { get; set; }
        public EmpresaInformacoes Empresa { get; set; }
        public UsuarioEmpresaChamadoDTO Usuario { get; set; }
        public string NomeTecnicoAberturaChamado { get; set; }
    }
}
