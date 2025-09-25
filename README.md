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


# Detalhamento dos Diretórios Front End (src/)
Abaixo está a descrição de cada diretório principal dentro da pasta src/:

# src/api/
Propósito: Centralizar toda a comunicação com a API externa (backend).

Conteúdo: Funções que realizam as chamadas HTTP (ex: login, fetchTickets, createTicket).

# src/assets/
Propósito: Armazenar todos os arquivos estáticos da aplicação.

Conteúdo: Imagens (logos, ícones), fontes personalizadas e outros recursos visuais.

# src/components/
Propósito: O coração da interface do usuário. Contém todos os componentes React reutilizáveis. É subdividido para melhor organização:

# ui/: Componentes de UI. São blocos de construção genéricos como Button.jsx, Input.jsx, Card.jsx. Eles não possuem lógica de negócio e são estilizados para seguir a identidade visual do TechDesk.

# layout/: Componentes responsáveis pela estrutura visual principal das páginas, como Sidebar.jsx ou DashboardLayout.jsx. Eles organizam o espaço onde o conteúdo das páginas será renderizado.

# features/: Componentes mais complexos que agrupam vários componentes de ui/ para realizar uma funcionalidade específica. Por exemplo, no TicketForm.jsx seria um componente de feature que combina inputs, botões e lógica para criar um novo chamado.


# src/contexts/
Propósito: Gerenciar o estado global da aplicação usando a Context API do React.

Conteúdo: Provedores de contexto para dados que precisam ser acessíveis em várias partes da aplicação sem a necessidade de "prop drilling". Um exemplo claro será o AuthContext.jsx, que controlará os dados do usuário autenticado.


# src/hooks/
Propósito: Armazenar hooks customizados do React.

Conteúdo: Funções reutilizáveis que encapsulam lógica com estado (ex: useAuth, useFetchTickets). Hooks customizados ajudam a manter os componentes mais limpos e a compartilhar lógica complexa de forma eficiente.


# src/pages/
Propósito: Representar as páginas completas da aplicação, que são renderizadas com base nas rotas.

Conteúdo: Componentes como LoginPage.jsx e DashboardPage.jsx. Eles são responsáveis por "montar" uma tela, organizando e orquestrando múltiplos componentes de layout e features para formar uma visão coesa para o usuário.


# src/routes/
Propósito: Definir e configurar o sistema de navegação (rotas) da aplicação.

Conteúdo: Lógica de roteamento (usado a biblioteca react-router-dom) que mapeia as URLs do navegador para os componentes de pages correspondentes. Aqui também são definidas rotas privadas (protegidas por autenticação) e públicas.


# src/utils/
Propósito: Conter funções auxiliares, puras e genéricas que podem ser usadas em qualquer parte do projeto.

Conteúdo: Funções que não dependem do estado do React, como formatadores de data (formatDate.js) etc.


## Link Figma
https://www.figma.com/design/KJFIsNApwUnpXrTFVwcEWq/Sistema-Help-Desk?node-id=0-1&t=bsnUhemxHY6fdTSL-1

## Link Documentação TechDesk - Em FAse de Construção
https://github.com/GeoS1lva/Sistema_HelpDesk/blob/main/Documenta%C3%A7%C3%A3o_TechDesk.pdf
