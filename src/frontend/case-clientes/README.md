# Sistema de Clientes - Frontend# CaseClientes



Sistema de gerenciamento de clientes desenvolvido em Angular 20 com autenticação JWT.This project was generated using [Angular CLI](https://github.com/angular/angular-cli) version 20.0.4.



## 🚀 Funcionalidades## Development server



- ✅ **Autenticação JWT** - Login com email e senhaTo start a local development server, run:

- ✅ **Listagem de Clientes** - Tabela com todos os clientes cadastrados

- ✅ **CRUD de Clientes** - Criar, editar e deletar clientes```bash

- ✅ **Transações Financeiras** - Depositar e sacar saldo dos clientesng serve

- ✅ **Interface Responsiva** - Design moderno e adaptável```



## 📋 Pré-requisitosOnce the server is running, open your browser and navigate to `http://localhost:4200/`. The application will automatically reload whenever you modify any of the source files.



- Node.js 20+ ## Code scaffolding

- npm ou yarn

- Docker e Docker Compose (opcional)Angular CLI includes powerful code scaffolding tools. To generate a new component, run:



## 🛠️ Instalação e Execução```bash

ng generate component component-name

### Modo Desenvolvimento (Local)```



```bashFor a complete list of available schematics (such as `components`, `directives`, or `pipes`), run:

# Instalar dependências

npm install```bash

ng generate --help

# Executar servidor de desenvolvimento```

npm start

## Building

# Aplicação estará disponível em http://localhost:4200

```To build the project run:



### Modo Desenvolvimento (Docker)```bash

ng build

```bash```

# Build e execução do container de desenvolvimento

docker-compose --profile dev upThis will compile your project and store the build artifacts in the `dist/` directory. By default, the production build optimizes your application for performance and speed.



# Ou com rebuild## Running unit tests

docker-compose --profile dev up --build

To execute unit tests with the [Karma](https://karma-runner.github.io) test runner, use the following command:

# Aplicação estará disponível em http://localhost:4200

``````bash

ng test

### Modo Produção (Docker)```



```bash## Running end-to-end tests

# Build e execução do container de produção

docker-compose --profile prod upFor end-to-end (e2e) testing, run:



# Ou com rebuild```bash

docker-compose --profile prod up --buildng e2e

```

# Aplicação estará disponível em http://localhost:8080

```Angular CLI does not come with an end-to-end testing framework by default. You can choose one that suits your needs.



### Build para Produção (Local)## Additional Resources



```bashFor more information on using the Angular CLI, including detailed command references, visit the [Angular CLI Overview and Command Reference](https://angular.dev/tools/cli) page.

# Gerar build otimizado
npm run build

# Output estará em dist/case-clientes/browser
```

## 🐳 Comandos Docker Úteis

```bash
# Parar containers
docker-compose down

# Parar e remover volumes
docker-compose down -v

# Ver logs
docker-compose logs -f

# Rebuild completo
docker-compose build --no-cache
```

## 🔧 Configuração da API

A URL da API backend pode ser configurada nos arquivos:
- `src/environments/environment.ts` (desenvolvimento)
- `src/environments/environment.prod.ts` (produção)

Por padrão, a aplicação aponta para `http://localhost:5000`.

## 📱 Fluxo de Uso

### 1. Login
- Acesse a aplicação
- Use qualquer email/senha (modo demo)
- Será redirecionado para o dashboard

### 2. Dashboard
- Visualize todos os clientes cadastrados
- Use os botões de ação para:
  - ✏️ **Editar** - Alterar nome e email do cliente
  - 🗑️ **Deletar** - Remover cliente do sistema
  - 💰 **Depositar** - Adicionar saldo ao cliente
  - 💸 **Sacar** - Remover saldo do cliente

### 3. Criar Cliente
- Clique no botão "Novo Cliente"
- Preencha nome e email
- Cliente é criado com saldo inicial de R$ 0,00

## 🏗️ Estrutura do Projeto

```
src/
├── app/
│   ├── components/           # Componentes da aplicação
│   │   ├── login/           # Tela de login
│   │   ├── dashboard/       # Dashboard principal
│   │   ├── modal/           # Modal base reutilizável
│   │   ├── cliente-form-modal/  # Modal de criar/editar cliente
│   │   └── transacao-modal/     # Modal de depósito/saque
│   ├── guards/              # Guards de proteção de rotas
│   │   └── auth.guard.ts    # Verificação de autenticação
│   ├── interceptors/        # Interceptors HTTP
│   │   └── auth.interceptor.ts  # Adiciona JWT nas requisições
│   ├── models/              # Interfaces TypeScript
│   │   ├── auth.model.ts    # Modelos de autenticação
│   │   └── cliente.model.ts # Modelos de cliente
│   ├── services/            # Serviços da aplicação
│   │   ├── auth.service.ts  # Gerenciamento de autenticação
│   │   └── cliente.service.ts   # Operações com clientes
│   ├── app.config.ts        # Configuração do app
│   └── app.routes.ts        # Definição de rotas
├── environments/            # Configurações de ambiente
└── styles.css              # Estilos globais
```

## 🔐 Autenticação JWT

O sistema implementa autenticação JWT com as seguintes características:

- Token armazenado no localStorage
- Interceptor HTTP adiciona token automaticamente em todas as requisições
- Guard protege rotas que requerem autenticação
- Logout limpa token e redireciona para login

**Nota:** Atualmente o login é simulado. Para integrar com o backend real, atualize o método `login()` em `auth.service.ts`.

## 🎨 Tecnologias Utilizadas

- **Angular 20** - Framework principal
- **TypeScript** - Linguagem de programação
- **RxJS** - Programação reativa
- **Standalone Components** - Arquitetura moderna do Angular
- **Docker** - Containerização
- **Nginx** - Servidor web para produção

## 📝 Endpoints da API Backend

O frontend consome os seguintes endpoints:

- `GET /api/clientes` - Listar todos os clientes
- `GET /api/clientes/:id` - Obter cliente por ID
- `POST /api/clientes` - Criar novo cliente
- `PUT /api/clientes/:id` - Atualizar cliente
- `DELETE /api/clientes/:id` - Deletar cliente
- `POST /api/clientes/:id/depositar` - Depositar saldo
- `POST /api/clientes/:id/sacar` - Sacar saldo

## 🚧 Próximas Melhorias

- [ ] Integrar endpoint real de autenticação JWT no backend
- [ ] Adicionar validação de formulários mais robusta
- [ ] Implementar paginação na tabela de clientes
- [ ] Adicionar filtros e busca de clientes
- [ ] Mostrar histórico de transações
- [ ] Testes unitários e e2e
- [ ] Melhorias de acessibilidade (a11y)

## 📄 Licença

Este projeto foi desenvolvido como um case técnico.
