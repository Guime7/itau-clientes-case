# ⚡ Início Rápido - 5 Minutos

## 🚀 Modo Mais Rápido (Com Docker)

```bash
# 1. Entre no diretório do projeto
cd frontend/case-clientes

# 2. Inicie em modo desenvolvimento
docker-compose --profile dev up

# 3. Acesse no navegador
http://localhost:4200

# ✅ Pronto! O sistema está rodando
```

## 💻 Modo Local (Sem Docker)

```bash
# 1. Entre no diretório do projeto
cd frontend/case-clientes

# 2. Instale as dependências (apenas primeira vez)
npm install

# 3. Inicie o servidor de desenvolvimento
npm start

# 4. Acesse no navegador
http://localhost:4200

# ✅ Pronto! O sistema está rodando
```

## 🎯 Como Usar

### 1. Fazer Login
- Email: `qualquer@email.com`
- Senha: `qualquersenha`
- Clique em "Logar"

### 2. Dashboard
- Veja a lista de clientes (vazia inicialmente)
- Clique em "Novo Cliente" para adicionar

### 3. Criar Cliente
- Preencha Nome e Email
- Clique em "Criar"
- Cliente aparece na tabela com R$ 0,00

### 4. Ações com Cliente
- **✏️ Editar:** Alterar nome/email
- **🗑️ Deletar:** Remover cliente
- **💰 Depositar:** Adicionar saldo
- **💸 Sacar:** Remover saldo

### 5. Logout
- Clique no botão "Sair" no topo
- Volta para tela de login

## 🔌 Conectar com Backend

```bash
# 1. Edite o arquivo de ambiente
# src/environments/environment.ts

export const environment = {
  production: false,
  apiUrl: 'http://localhost:5000' // ← Sua URL do backend
};

# 2. Reinicie o servidor
```

## ⚠️ Solução de Problemas Rápida

### Porta 4200 já está em uso
```bash
# Windows
netstat -ano | findstr :4200
taskkill /PID <número> /F

# Ou use outra porta
npm start -- --port 4300
```

### Erro ao instalar dependências
```bash
# Limpar e reinstalar
rm -rf node_modules package-lock.json
npm install
```

### Docker não inicia
```bash
# Verificar se Docker está rodando
docker ps

# Rebuildar
docker-compose --profile dev up --build
```

## 📚 Documentação Completa

- 📖 **README.md** - Guia completo do projeto
- 🔗 **INTEGRATION.md** - Como integrar com backend
- 📊 **OVERVIEW.md** - Visão geral visual
- 🛠️ **COMMANDS.md** - Todos os comandos úteis
- 📝 **CHANGELOG.md** - Histórico de mudanças

## 🆘 Precisa de Ajuda?

1. Verifique os arquivos de documentação acima
2. Procure no COMMANDS.md para comandos específicos
3. Leia INTEGRATION.md para integração com backend

## 🎉 Pronto para Produção?

```bash
# Build de produção
npm run build

# Com Docker
docker-compose --profile prod up
```

---

**💡 Dica:** Para um teste completo, inicie o backend primeiro!

```bash
# Terminal 1: Backend
cd backend/Itau.Case.Clientes
dotnet run

# Terminal 2: Frontend
cd frontend/case-clientes
npm start
```
