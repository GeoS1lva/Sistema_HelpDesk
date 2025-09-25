using Sistema_HelpDesk.Desk.Domain.Chamados.Entidades;
using Sistema_HelpDesk.Desk.Domain.Common;

namespace Sistema_HelpDesk.Desk.Domain.Empresas.Entidades
{
    public class Empresa(string nome, string cnpj, string email, bool status, DateTime dataCriacao) : Entidade
    {
        public string Nome { get; set; } = nome;
        public string Cnpj { get; set; } = cnpj;
        public string Email { get; set; } = email;
        public bool Status { get; set; } = status;
        public DateTime DataCriacao { get; set; } = dataCriacao;

        public ICollection<UsuariosEmpresa> Usuarios { get; set; }
        public ICollection<Chamado> Chamados { get; set; }
    }
}
