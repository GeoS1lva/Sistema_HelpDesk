# Sistema_HelpDesk - TechDesk

### 💡 Ideia
O objetivo deste projeto é desenvolver um **Sistema Web de HelpDesk** robusto e intuitivo, que inclua não apenas a aplicação principal, mas também um **portal do cliente** para abertura e acompanhamento de chamados.
Alunos:
- Geovana Paula da Silva - RA 170610-2024 (Desenvolvedora BackEnd)
- Eduardo Bernardes Zanin - RA 183624-2024 (Desenvolvedor FrontEnd)

---

### 🎯 Funcionalidades Previstas
- **Gestão de empresas cadastradas**  
  Possibilidade de criar, editar e gerenciar empresas que utilizarão o sistema.
- **Controle de usuários**  
  Usuários vinculados a empresas, com permissões para abertura e acompanhamento de chamados.
- **Cadastro e gerenciamento de técnicos**  
  Perfis de técnicos com atribuição de chamados e registro de atividades.
- **Categorização e mesas de atendimento**  
  Organização dos chamados por áreas como suporte, infraestrutura, etc.
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
🚧 **Finalizado V1.0**

#### Frontend
- Protótipo no **Figma** das principais partes do sistema:
  - Login do sistema
  - Login do portal do cliente
  - Painel de chamados
  - Painel para abertura de chamado pelo técnico
- Implementação/codificação das telas de login.
- Implementação/codificação portal do cliente
- Implementação/codificação painel de chamados
- Implementação/codificação painel do cliente
- Implementação/codificação tela gerenciamento de empresas e usuários
- Implementação/codificação tela gerenciamento de técnicos
- Implementação/codificação tela gerenciamento de mesas de atendimento

#### Backend
- Construção das entidades principais do sistema e seus relacionamentos.
- Conexão com o banco de dados configurada.
- Implementação do **ASP.NET Identity** para controle de usuários e roles.
- Autenticação e autorização com **JWT + Cookie**, já aplicado nas APIs:
  - Apenas usuários com o papel/role correto conseguem realizar requisições.
- Implementado **serviço de e-mail**:
  - Envio de e-mail para redefinição de senha.
  - Estrutura preparada para futuros serviços de notificação.
- Implementado CRUD completo das entidades:
  - UsuarioSistema
  - UsuarioEmpresa
  - Empresa
  - Mesas de Atendimento
  - Categorias
  - Chamados
- Implemenado módulo de SLA
- Implementado rotinas agendadas para serem executadas e atualizar o status do SLA com Background Jobs utilizando Quartz
- Construção de APIs com Arquitetura RestFul
- Implementado o Design Pattern UnitOfWork para a consistência dos dados
- Implementado Result Pattern (padrão de resultado)
- Iniciado implementação do chat entre cliente e técnico com SignalR (Em Construção)

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

---

## 📋 Pré-requisitos

Para executar o projeto, certifique-se de ter instalado:

* [.NET SDK 8.0 ou superior](https://dotnet.microsoft.com/download)
* [Node.js](https://nodejs.org/) (v18 ou superior)
* SQL Server (Express ou LocalDB)

---

### 1. Siga os passos abaixo para rodar o BackEnd
 1. Clone o repositório Online
 2. Navegue até a pasta `TechDesk_BackEnd`.
 3. Abra o arquivo de solução **`Sistema_HelpDesk.sln`** no Visual Studio 2022.

### 2. Configuração do `appsettings.json`
Dentro da pasta `TechDesk_BackEnd/Aplicacao`, crie um arquivo com o nome **`appsettings.json`** e insira o seguinte conteúdo:

```json
{
  "EmailConfiguracao": {
    "SmtpServer": "smtp.titan.email",
    "Port": 587,
    "UserName": "suporte@techdesk.dev.br",
    "Password": "Senha_Email",
    "FromEmail": "suporte@techdesk.dev.br",
    "FromName": "Suporte_TechDesk"
  },
  "Jwt": {
    "Issuer": "https://localhost",
    "Audience": "api",
    "Key": "iLTCvJfm16nJfwNl4XcHs933aTIYaFHF",
    "AccessTokenMinutes": 480
  },
  "ConnectionStrings": {
    "ConexaoSql": "Server=NOME_SERVIDOR_LOCAL_BANCO_DE_DADOS;Database=Sistema_HelpDesk;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```
IMPORTANTE: Substitua NOME_SERVIDOR_LOCAL_BANCO_DE_DADOS pelo hostname do seu SQL Server (ex: DESKTOP-XXXX\SQLEXPRESS ou (localdb)\mssqllocaldb).

### 3. Instalação de Dependências e Banco de Dados
Abra o terminal (CMD), navegue até a pasta TechDesk_BackEnd/Aplicacao e execute os comandos:

Restaurar pacotes e instalar ferramenta do EF:
- dotnet restore
- dotnet tool install --global dotnet-ef - (Nota: Se a ferramenta já estiver instalada, ele apenas avisará, pode ignorar o erro).

Executar as Migrações (Criar Banco): 
- dotnet ef database update

### 4. Executar a Aplicação
- Volte ao Visual Studio 2022.
- Clique no botão de Play (perfil Https).
- Após iniciar, será aberto a documentação Swagger - (OBS: No Swagger, verifique a aba Schemas. Alguns campos utilizam valores Enum para o envio correto)

### 5. Executar FrontEnd
Para Executar o FrontEnd acesse a Documentação interna do projeto dentro da pasta techdesk_frontend
