using Sistema_HelpDesk.Desk.Domain.Chamados.Entidades;
using Sistema_HelpDesk.Desk.Domain.Common;

namespace Sistema_HelpDesk.Desk.Domain.Empresas.Entidades
{
    public class Empresa(string nome, string cnpj, string email) : Entidade
    {
        public string Nome { get; set; } = nome;
        public string Cnpj { get; set; } = cnpj;
        public string Email { get; set; } = email;
        public bool Status { get; set; } = true;
        public DateTime DataCriacao { get; set; } = DateTime.Now;

        public ICollection<UsuariosEmpresa> Usuarios { get; set; }
        public ICollection<Chamado> Chamados { get; set; }

        public void Ativo()
            => Status = true;

        public void Desativado()
            => Status = false;
    }
}
