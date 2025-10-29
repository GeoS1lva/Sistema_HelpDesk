using Sistema_HelpDesk.Desk.Domain.Chamados.Entidades;
using Sistema_HelpDesk.Desk.Domain.Common;
using Sistema_HelpDesk.Desk.Domain.Users;
using Sistema_HelpDesk.Desk.Domain.Users.Entities;

namespace Sistema_HelpDesk.Desk.Domain.Empresas.Entidades
{
    public class UsuariosEmpresa : Entidade
    {
        public string Nome { get; set; }
        public bool Status { get; set; }

        public int EmpresaId { get; set; }
        public Empresa Empresa { get; set; }

        public UserLogin UserLogin { get; set; }

        public UsuariosEmpresa(int id, string nome, int empresaId)
        {
            Id = id;
            Nome = nome;
            Status = true;
            EmpresaId = empresaId;
        }

        public void Ativo()
            => Status = true;

        public void Desativado()
            => Status = false;
    }
}
