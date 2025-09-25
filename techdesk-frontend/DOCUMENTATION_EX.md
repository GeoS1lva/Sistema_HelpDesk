# Projeto TechDesk - Frontend

Bem-vindo(a) à documentação do frontend do sistema TechDesk. Este guia contém todas as informações necessárias para configurar, executar e testar o projeto localmente.

#  Sobre o Projeto

O TechDesk é um sistema web de gestão de chamados (HelpDesk) focado em usabilidade e eficiência. Este repositório contém o código-fonte da interface do usuário (frontend), desenvolvida para interagir com uma API de backend.

# Documentação da Estrutura de Pastas – Projeto TechDesk

# 1. Introdução

A arquitetura de pastas do projeto TechDesk foi projetada com foco em separação de responsabilidades, escalabilidade e manutenibilidade. O objetivo é criar um ambiente de desenvolvimento onde encontrar, modificar e adicionar funcionalidades seja uma tarefa intuitiva.

Esta estrutura se alinha com princípios da Clean Architecture, isolando a lógica de negócio, a interface do usuário (UI) e a camada de dados, facilitando a evolução do sistema e a futura integração com o backend.

# 2. Detalhamento dos Diretórios (src/)
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




## Tecnologias Utilizadas

* **Framework:** [React](https://react.dev/)
* **Build Tool:** [Vite](https://vitejs.dev/)
* **Gerenciador de Pacotes:** [pnpm](https://pnpm.io/)
* **Estilização:** [Tailwind CSS](https://tailwindcss.com/)
* **Cliente HTTP:** [Axios](https://axios-http.com/)

##  Pré-requisitos

Antes de começar, certifique-se de que você tem os seguintes softwares instalados em sua máquina:

* [Node.js](https://nodejs.org/) (versão LTS recomendada)
* [pnpm](https://pnpm.io/installation) (pode ser instalado com `npm install -g pnpm`)

##  Configuração do Ambiente Local

Siga estes passos para ter o projeto rodando em sua máquina.

### Passo 1: Clonar o Repositório

```bash
git clone <URL_DO_SEU_REPOSITORIO>
cd techdesk-frontend

Passo 2: Instalar as Dependências
Este projeto utiliza o pnpm para um gerenciamento mais eficiente de pacotes. Para instalar todas as dependências, execute:

pnpm install

Passo 3: Configurar as Variáveis de Ambiente
Este é o passo mais importante para conectar o frontend ao backend.

Na raiz do projeto, você encontrará um arquivo chamado .env.example. Ele serve como um modelo.

Crie uma cópia deste arquivo e renomeie-a para .env.


# No Windows (usando PowerShell)
copy .env.example .env

# No Linux ou macOS
cp .env.example .env
Abra o arquivo .env recém-criado e edite a variável VITE_API_BASE_URL. Você deve alterá-la para a URL base do servidor backend que está rodando na sua máquina.

Por exemplo:

# Arquivo .env
VITE_API_BASE_URL=http://localhost:7021
Por que isso é necessário? O arquivo .env é específico para a sua máquina e não é enviado para o repositório (está no .gitignore). Isso permite que cada desenvolvedor aponte o frontend para seu próprio ambiente de backend local sem afetar os outros.

5. Executando o Projeto
Com o ambiente configurado, você pode usar os seguintes scripts:

Para Desenvolvimento
Este comando inicia o servidor de desenvolvimento com "Hot-Reload", que atualiza a aplicação automaticamente no navegador sempre que você salva uma alteração no código.

pnpm dev
Após executar o comando, a aplicação estará disponível em http://localhost:5173 (ou outra porta indicada no terminal).

Para Produção
Estes comandos são para gerar a versão final da aplicação, que seria enviada para um servidor de hospedagem.

Bash

# 1. Gera a build otimizada do projeto na pasta /dist
pnpm build

# 2. Inicia um servidor local para testar a build de produção
pnpm preview

6. Como Testar a Integração com o Backend
A principal função desta documentação é permitir que o  backend teste a integração com a interface.

Tela de Login (Funcionalidade Atual)
Atualmente, a única funcionalidade implementada para teste de integração é a Tela de Login.

Comportamento Esperado:
Execute o projeto frontend com pnpm dev.

Execute o seu projeto backend (API C#).

Abra o frontend no navegador.

Preencha os campos "usuário" e "senha" e clique no botão "Login".

A aplicação frontend fará uma requisição do tipo POST para o endpoint que foi combinado (ex: /auth/login) na URL que você configurou no seu arquivo .env.

Formato dos Dados Enviados (Payload):
O frontend enviará um objeto JSON no corpo da requisição com a seguinte estrutura:

JSON

{
  "username": "o_valor_digitado_pelo_usuario",
  "password": "a_senha_digitada_pelo_usuario"
}
Obs: O nome das chaves (username, password) deve estar alinhado com o que a API espera receber.

Como Verificar a Requisição:
Você pode visualizar a requisição em tempo real e a resposta do seu backend diretamente no navegador:

Clique com o botão direito na página e vá em Inspecionar (ou pressione F12).

Vá para a aba Rede (Network).

Realize o login. Uma nova linha aparecerá na lista de requisições.

Clique nela para inspecionar os detalhes, como o Payload (o que o frontend enviou) e a Resposta (Response) (o que seu backend retornou).