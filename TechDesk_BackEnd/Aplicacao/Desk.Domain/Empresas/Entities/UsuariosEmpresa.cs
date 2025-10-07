using Sistema_HelpDesk.Desk.Domain.Chamados.Entidades;
using Sistema_HelpDesk.Desk.Domain.Common;
using Sistema_HelpDesk.Desk.Domain.Users;

namespace Sistema_HelpDesk.Desk.Domain.Empresas.Entidades
{
    public class UsuariosEmpresa(string nome, bool status, int empresaId) : Entidade
    {
        public string Nome { get; set; } = nome;
        public bool Status { get; set; } = status;

        public int EmpresaId { get; set; } = empresaId;
        public Empresa Empresa { get; set; }

        public void Ativo()
            => Status = true;

        public void Desativado()
            => Status = false;
    }
}
