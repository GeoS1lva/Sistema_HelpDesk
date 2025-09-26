# Sistema_HelpDesk - TechDesk

### 💡 Ideia
O objetivo deste projeto é desenvolver um **Sistema Web de HelpDesk** robusto e intuitivo, que inclua não apenas a aplicação principal, mas também um **portal do cliente** para abertura e acompanhamento de chamados.

---

### 🎯 Funcionalidades Previstas
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

### 🛠️ Tecnologias Planejadas
#### Front-End
- React

#### Back-End
- C# (.NET 8) com ASP.NET Core Web API  
  - Criação de APIs RESTful  
  - Autenticação e autorização com **ASP.NET Identity** + JWT + Cookie 
  - **Entity Framework Core** para acesso a dados

#### Banco de Dados
- SQL Server *(escolha motivada pela integração nativa com o ASP.NET)*

---

### 📅 Status do Projeto
🚧 **Em desenvolvimento** — funcionalidades iniciais já implementadas.

#### Frontend
- Protótipo no **Figma** das principais partes do sistema:
  - Login do sistema
  - Login do portal do cliente
  - Painel de chamados
  - Painel para abertura de chamado pelo técnico
- Implementação/codificação das telas de login.

#### Backend
- Construção das entidades principais do sistema e seus relacionamentos.
- Conexão com o banco de dados configurada.
- Implementação do **ASP.NET Identity** para controle de usuários e roles.
- Autenticação e autorização com **JWT + Cookie**, já aplicado nas APIs:
  - Apenas usuários com o papel/role correto conseguem realizar requisições.
- Início da construção do **serviço de e-mail**:
  - Envio de e-mail para redefinição de senha.
  - Estrutura preparada para futuros serviços de notificação.

---

### 🔗 Link Figma
https://www.figma.com/design/KJFIsNApwUnpXrTFVwcEWq/Sistema-Help-Desk?node-id=0-1&t=bsnUhemxHY6fdTSL-1

### 📄 Link Documentação TechDesk - *Em Fase de Construção*
https://github.com/GeoS1lva/Sistema_HelpDesk/blob/main/Documenta%C3%A7%C3%A3o_TechDesk.pdf

---

## 📂 Detalhamento dos Diretórios Front End (src/)
Abaixo está a descrição de cada diretório principal dentro da pasta src/:

### src/api/
Propósito: Centralizar toda a comunicação com a API externa (backend).  
Conteúdo: Funções que realizam as chamadas HTTP (ex: login, fetchTickets, createTicket).

### src/assets/
Propósito: Armazenar todos os arquivos estáticos da aplicação.  
Conteúdo: Imagens (logos, ícones), fontes personalizadas e outros recursos visuais.

### src/components/
Propósito: O coração da interface do usuário. Contém todos os componentes React reutilizáveis. É subdividido para melhor organização:

- **ui/**: Componentes de UI. São blocos de construção genéricos como Button.jsx, Input.jsx, Card.jsx.  
- **layout/**: Componentes responsáveis pela estrutura visual principal das páginas, como Sidebar.jsx ou DashboardLayout.jsx.  
- **features/**: Componentes mais complexos que agrupam vários componentes de ui/ para realizar uma funcionalidade específica.

### src/contexts/
Propósito: Gerenciar o estado global da aplicação usando a Context API do React.  
Conteúdo: Provedores de contexto, como AuthContext.jsx para controlar dados do usuário autenticado.

### src/hooks/
Propósito: Armazenar hooks customizados do React.  
Conteúdo: Funções reutilizáveis que encapsulam lógica com estado (ex: useAuth, useFetchTickets).

### src/pages/
Propósito: Representar as páginas completas da aplicação.  
Conteúdo: Componentes como LoginPage.jsx e DashboardPage.jsx.

### src/routes/
Propósito: Definir e configurar o sistema de navegação.  
Conteúdo: Rotas privadas (autenticadas) e públicas com react-router-dom.

### src/utils/
Propósito: Conter funções auxiliares e genéricas.  
Conteúdo: Exemplo: formatDate.js para formatação de datas.

---

## 📂 Detalhamento dos Diretórios BackEnd (Aplicacao)

### 🏗️ Estrutura do Projeto
O projeto segue os princípios da **Clean Architecture**, separando responsabilidades em camadas para garantir desacoplamento, testabilidade e manutenção do código.

A estrutura atual está organizada da seguinte forma:
Sistema_HelpDesk/
- Controllers # Pontos de entrada da API (camada de interface/adapters)
- Desk.Application # Regras de negócio da aplicação (casos de uso)
- Desk.Domain # Entidades e contratos de domínio (regras centrais do sistema)
- Desk.Infra # Implementações de infraestrutura (acesso a dados, repositórios, serviços externos)
- Migrations # Migrações do Entity Framework para controle de banco de dados
- appsettings.json # Arquivo de configuração da aplicação
- Program.cs # Ponto de entrada da aplicação

### 📚 Descrição das Camadas
- **Desk.Domain**  
  Contém as **entidades de negócio** e **interfaces**. É a camada mais central, independente de frameworks ou bancos de dados.  

- **Desk.Application**  
  Implementa os **casos de uso** do sistema, orquestrando regras de negócio do domínio.  

- **Desk.Infra**  
  Responsável pela **persistência de dados** e integrações externas. Implementa interfaces definidas no domínio.  

- **Controllers**  
  Camada de **interface com o usuário** (no caso, API REST). Recebe as requisições, valida e repassa para a camada de aplicação.  

Essa organização facilita a evolução do sistema, garantindo baixo acoplamento e alta coesão entre as partes.
