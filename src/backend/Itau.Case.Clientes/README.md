# Itau Case Clientes - Refatoração Completa

## Mudanças Realizadas

### 1. Padronização de Commands e Handlers
- ✅ Todos os Commands foram refatorados para usar `record` com namespace correto
- ✅ Todos os Handlers utilizam construtor primário e IHandler customizado
- ✅ Criados arquivos Request para todos os Commands (CriarClienteRequest, AtualizarClienteRequest, DepositarRequest, SacarRequest)
- ✅ Commands agora retornam DTOs apropriados (ClienteDto para operações CRUD, CommandResult para transações)

### 2. Padronização de Queries e QueryHandlers
- ✅ ObterClientePorIdQuery refatorada para record com IRequest<ClienteDto?>
- ✅ ObterTodosClientesQuery refatorada para record com IRequest<IEnumerable<ClienteDto>>
- ✅ Todos os QueryHandlers utilizam construtor primário e IHandler customizado

### 3. Unificação de Controllers
- ✅ Removido ClienteController.cs duplicado
- ✅ ClientesController.cs consolidado com todos os endpoints:
  - GET /api/clientes - Obter todos os clientes
  - GET /api/clientes/{id} - Obter cliente por ID
  - POST /api/clientes - Criar novo cliente
  - PUT /api/clientes/{id} - Atualizar cliente
  - DELETE /api/clientes/{id} - Deletar cliente
  - POST /api/clientes/{id}/depositar - Realizar depósito
  - POST /api/clientes/{id}/sacar - Realizar saque
- ✅ Tratamento de exceções padronizado (DomainException, InvalidOperationException)

### 4. Infrastructure - SQLite In-Memory
- ✅ Configurado Entity Framework Core com SQLite
- ✅ Criado ClientesDbContext com mapeamento completo da entidade Cliente
- ✅ Implementado ClienteRepository com todas as operações CRUD
- ✅ Banco de dados inicializado em memória automaticamente no startup

### 5. Dependency Injection e Program.cs
- ✅ DbContext configurado para usar SQLite in-memory
- ✅ IClienteRepository registrado com ClienteRepository
- ✅ Mediator customizado registrado
- ✅ Todos os Handlers registrados automaticamente via Reflection
- ✅ Database.EnsureCreated() chamado na inicialização

### 6. Melhorias no Domain
- ✅ ClienteDto atualizado para incluir DataAtualizacao
- ✅ Cliente.cs atualizado com construtor protegido para EF Core
- ✅ DataCriacao agora é setada no construtor da entidade

## Estrutura de Pastas

```
Itau.Case.Clientes/
├── Itau.Case.Clientes.Api/
│   ├── Controllers/
│   │   └── ClientesController.cs (unificado)
│   ├── Program.cs (configurado com DI e SQLite)
│   └── Itau.Case.Clientes.Api.csproj
├── Itau.Case.Clientes.Application/
│   ├── Common/
│   │   └── Mediator/ (IMediator, IHandler, IRequest, Mediator)
│   ├── Context/
│   │   ├── Commands/
│   │   │   ├── AtualizarCliente/
│   │   │   │   ├── AtualizarClienteCommand.cs (record)
│   │   │   │   ├── AtualizarClienteHandler.cs
│   │   │   │   └── AtualizarClienteRequest.cs
│   │   │   ├── CriarCliente/
│   │   │   │   ├── CriarClienteCommand.cs (record)
│   │   │   │   ├── CriarClienteHandler.cs
│   │   │   │   └── CriarClienteRequest.cs
│   │   │   ├── DeletarCliente/
│   │   │   │   ├── DeletarClienteCommand.cs (record)
│   │   │   │   └── DeletarClienteHandler.cs
│   │   │   ├── DepositarSaldoCliente/
│   │   │   │   ├── DepositarCommand.cs (record)
│   │   │   │   ├── DepositarHandler.cs
│   │   │   │   └── DepositarRequest.cs
│   │   │   └── SacarSaldoCliente/
│   │   │       ├── SacarCommand.cs (record)
│   │   │       ├── SacarHandler.cs
│   │   │       └── SacarRequest.cs
│   │   └── Queries/
│   │       ├── ObterClientePorId/
│   │       │   ├── ObterClientePorIdQuery.cs (record)
│   │       │   └── ObterClientePorIdQueryHandler.cs
│   │       └── ObterTodosClientes/
│   │           ├── ObterTodosClientesQuery.cs (record)
│   │           └── ObterTodosClientesQueryHandler.cs
│   ├── Interfaces/
│   │   └── IClienteRepository.cs
│   └── Itau.Case.Clientes.Application.csproj
├── Itau.Case.Clientes.Domain/
│   ├── Base/
│   ├── Dtos/
│   │   └── ClienteDto.cs (record com DataAtualizacao)
│   ├── Entities/
│   │   ├── Cliente.cs (com construtor EF Core)
│   │   └── Transacao.cs
│   ├── Enums/
│   ├── Exceptions/
│   └── Itau.Case.Clientes.Domain.csproj
├── Itau.Case.Clientes.Infrastructure/
│   ├── Data/
│   │   └── ClientesDbContext.cs (EF Core)
│   ├── Repositories/
│   │   └── ClienteRepository.cs
│   └── Itau.Case.Clientes.Infrastructure.csproj
└── Itau.Case.Clientes.sln
```

## Tecnologias Utilizadas

- .NET 8.0
- Entity Framework Core 8.0
- SQLite (In-Memory)
- Swagger/OpenAPI
- CQRS Pattern (Command/Query Separation)
- Mediator Pattern (Custom Implementation)
- Repository Pattern
- Domain-Driven Design (DDD)

## Como Executar

1. Restaurar pacotes:
```bash
dotnet restore
```

2. Compilar:
```bash
dotnet build
```

3. Executar:
```bash
cd Itau.Case.Clientes.Api
dotnet run
```

4. Acessar Swagger:
```
https://localhost:5001/swagger
```

## Observações Importantes

- O banco de dados é **in-memory** e os dados são perdidos ao reiniciar a aplicação
- Todos os Commands, Queries e Handlers seguem o mesmo padrão de nomenclatura e estrutura
- O Mediator customizado registra automaticamente todos os handlers via Reflection
- Validações de domínio são feitas através de DomainException
- Retornos HTTP apropriados: 200 OK, 201 Created, 204 No Content, 400 Bad Request, 404 Not Found, 409 Conflict, 500 Internal Server Error
