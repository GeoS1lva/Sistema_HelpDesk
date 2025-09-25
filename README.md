EM CONSTRUÇÃO...

# Sistema_HelpDesk

## 💡 Ideia

O objetivo deste projeto é desenvolver um **Sistema Web de HelpDesk** robusto e intuitivo, que inclua não apenas a aplicação principal, mas também um **portal do cliente** para abertura e acompanhamento de chamados.

---

## 🎯 Funcionalidades Previstas

- **Gestão de empresas cadastradas**  
  Possibilidade de criar, editar e gerenciar empresas que utilizarão o sistema.

- **Controle de usuários**  
  Usuários vinculados a empresas, com permissões para abertura e acompanhamento de chamados.

- **Cadastro e gerenciamento de técnicos**  
  Perfis de técnicos com atribuição de chamados e registro de atividades.

- **Categorização e mesas de atendimento**  
  Organização dos chamados por áreas como *suporte*, *infraestrutura*, etc.

- **Controle de tempo e produtividade**  
  Registro do tempo gasto em cada chamado, vinculado ao técnico responsável ou participantes na solução.

---

## 🛠️ Tecnologias Planejadas

### Front-End
- React

### Back-End
- C# (.NET 8) com ASP.NET Core Web API  
  - Criação de APIs RESTful  
  - Autenticação e autorização com **ASP.NET Identity** + JWT + Cookie 
  - **Entity Framework Core** para acesso a dados

### Banco de Dados
- SQL Server *(escolha motivada pela integração nativa com o ASP.NET)*

---

## 📅 Status do Projeto

🚧 **Em desenvolvimento** — funcionalidades iniciais já implementadas.

### Frontend
- Protótipo no **Figma** das principais partes do sistema:
  - Login do sistema
  - Login do portal do cliente
  - Painel de chamados
  - Painel para abertura de chamado pelo técnico
- Implementação/codificação das telas de login.

### Backend
- Construção das entidades principais do sistema e seus relacionamentos.
- Conexão com o banco de dados configurada.
- Implementação do **ASP.NET Identity** para controle de usuários e roles.
- Autenticação e autorização com **JWT + Cookie**, já aplicado nas APIs:
  - Apenas usuários com o papel/role correto conseguem realizar requisições.
- Início da construção do **serviço de e-mail**:
  - Envio de e-mail para redefinição de senha.
  - Estrutura preparada para futuros serviços de notificação.

## Link Figma
https://www.figma.com/design/KJFIsNApwUnpXrTFVwcEWq/Sistema-Help-Desk?node-id=0-1&t=bsnUhemxHY6fdTSL-1

## Link Documentação TechDesk - Em FAse de Construção
https://github.com/GeoS1lva/Sistema_HelpDesk/blob/main/Documenta%C3%A7%C3%A3o_TechDesk.pdf
