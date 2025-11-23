# Docker Configuration - Case Clientes

## Visão Geral

Este projeto utiliza Docker e Docker Compose para orquestrar três serviços principais:

- **Database**: MySQL 8.0 configurado para simular AWS RDS Aurora
- **Backend**: API .NET 8 com limites de recursos similares ao ECS Fargate
- **Frontend**: Aplicação Angular com hot-reload

## Estrutura de Containers

```
case-clientes-network (bridge)
├── case-clientes-database (MySQL 8.0)
│   ├── Port: 3306 (configurável)
│   └── Volume: case-clientes-mysql-data
├── case-clientes-backend (.NET 8 API)
│   ├── Port HTTP: 8080 (configurável)
│   ├── Port HTTPS: 8081 (configurável)
│   ├── CPU: 0.25 vCPU (limit) / 0.125 vCPU (reservation)
│   └── Memory: 512MB (limit) / 256MB (reservation)
└── case-clientes-frontend (Angular)
    ├── Port: 4200 (configurável)
```

## Configuração Inicial

1. **Copie o arquivo de variáveis de ambiente:**
   ```bash
   copy .env.example .env
   ```

2. **Ajuste as variáveis no arquivo `.env` conforme necessário**

## Como Usar

### Subir todos os serviços

```bash
docker-compose up -d
```

### Subir serviços específicos

```bash
# Apenas database
docker-compose up -d database

# Database e backend
docker-compose up -d database backend

# Todos os serviços
docker-compose up -d
```

### Ver logs

```bash
# Todos os serviços
docker-compose logs -f

# Serviço específico
docker-compose logs -f backend
docker-compose logs -f frontend
docker-compose logs -f database
```

### Verificar status dos containers

```bash
docker-compose ps
```

### Parar os serviços

```bash
docker-compose down
```

### Parar e remover volumes (CUIDADO: apaga dados do banco)

```bash
docker-compose down -v
```

## Recursos AWS Simulados

### Database (MySQL Aurora-like)

- Storage Engine: InnoDB
- Buffer Pool: 256MB
- Max Connections: 150
- Character Set: utf8mb4
- Collation: utf8mb4_unicode_ci
- Health checks configurados

### Backend (ECS Fargate)

Limites configurados similares a uma task ECS Fargate básica:
- **CPU**: 0.25 vCPU (256 CPU units)
- **Memory**: 512MB
- **Reservation**: 0.125 vCPU / 256MB

## Portas Configuráveis

Todas as portas podem ser personalizadas via arquivo `.env`:

| Serviço  | Variável            | Porta Padrão |
|----------|---------------------|--------------|
| MySQL    | MYSQL_PORT          | 3306         |
| Backend  | BACKEND_HTTP_PORT   | 8080         |
| Backend  | BACKEND_HTTPS_PORT  | 8081         |
| Frontend | FRONTEND_PORT       | 4200         |

## Volumes

- **mysql-data**: Persiste os dados do banco de dados MySQL

## Network

- **case-clientes-network**: Rede bridge isolada para comunicação entre containers

## Troubleshooting

### Container não inicia

```bash
# Ver logs detalhados
docker-compose logs <service-name>

# Verificar health status
docker inspect case-clientes-backend | grep -A 10 Health
```

### Conflito de portas

Altere as portas no arquivo `.env` e reinicie:

```bash
docker-compose down
docker-compose up -d
```

### Resetar banco de dados

```bash
docker-compose down -v
docker-compose up -d
```

### Rebuild dos containers

```bash
# Rebuild todos
docker-compose build --no-cache

# Rebuild específico
docker-compose build --no-cache backend

# Rebuild e start
docker-compose up -d --build
```

## Acesso aos Serviços

Após subir os containers:

- **Frontend**: http://localhost:4200
- **Backend API**: http://localhost:8080
- **Backend Swagger**: http://localhost:8080/swagger
- **MySQL**: localhost:3306

## Credenciais Padrão

⚠️ **IMPORTANTE**: Altere estas credenciais em produção!

- **MySQL Root**: root_password
- **MySQL User**: case_user
- **MySQL Password**: case_password
- **MySQL Database**: case_clientes

## Dados de Exemplo

O banco é inicializado automaticamente com:
- 3 clientes de exemplo
- 5 transações de exemplo
- Tabelas: `clientes` e `transacoes`
