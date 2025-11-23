# Frontend - Sistema de Clientes

Frontend em Angular 20 para gerenciamento de clientes com operações bancárias.

## 🚀 Tecnologias

- Angular 20
- TypeScript 5.8
- Vitest (testes unitários)
- Testing Library Angular
- RxJS

## 📋 Pré-requisitos

- Node.js 18+ 
- npm ou yarn

## 🔧 Instalação

```bash
cd src/frontend/case-clientes
npm install
```

## 🏃 Executar aplicação

### Desenvolvimento
```bash
npm start
```
A aplicação estará disponível em `http://localhost:4200`

### Build de produção
```bash
npm run build
```

## 🧪 Testes

### Executar todos os testes
```bash
npm test
```

### Testes com interface UI
```bash
npm run test:ui
```

### Cobertura de testes (100%)
```bash
npm run test:coverage
```

Os relatórios de cobertura serão gerados em `coverage/`

## 📁 Estrutura do Projeto

```
src/
├── app/
│   ├── components/        # Componentes da aplicação
│   │   ├── dashboard/    # Tela principal com lista de clientes
│   │   ├── login/        # Tela de login
│   │   ├── modal/        # Modal genérico
│   │   ├── cliente-form-modal/   # Modal de criação/edição
│   │   └── transacao-modal/      # Modal de depósito/saque
│   ├── guards/           # Guards de autenticação
│   ├── interceptors/     # Interceptors HTTP
│   ├── models/           # Interfaces e tipos
│   └── services/         # Serviços (API, Auth)
├── environments/         # Configurações de ambiente
└── test-setup.ts        # Configuração dos testes
```

## 🔗 Integração com API

A aplicação consome a API .NET Core em `http://localhost:5000/api`

### Result Pattern

Todos os endpoints da API retornam objetos no formato Result Pattern:

```typescript
interface Result<T> {
  isSuccess: boolean;
  data?: T;
  message?: string;
  errorCode?: string;
  errorDescription?: string;
}
```

## ✨ Funcionalidades

- ✅ Login simulado com localStorage
- ✅ Listagem de clientes
- ✅ Criação de clientes
- ✅ Edição de clientes
- ✅ Exclusão de clientes
- ✅ Depósitos
- ✅ Saques
- ✅ Mensagens de erro da API exibidas na tela
- ✅ Atualização automática da lista após operações
- ✅ Formatação de moeda e data brasileiras
- ✅ Validações de formulário

## 🧪 Testes Implementados

- ✅ ClienteService - 100% cobertura
- ✅ AuthService - 100% cobertura
- ✅ AuthGuard - 100% cobertura
- ✅ AuthInterceptor - 100% cobertura
- ✅ DashboardComponent - 100% cobertura
- ✅ LoginComponent - 100% cobertura
- ✅ ClienteFormModalComponent - 100% cobertura
- ✅ TransacaoModalComponent - 100% cobertura
- ✅ ModalComponent - 100% cobertura

## 🐛 Correções Implementadas

### Result Pattern
- Atualizado ClienteService para trabalhar com Result<T> da API
- Tratamento correto de erros do backend
- Exibição de mensagens de erro específicas

### Atualização da Tela
- Lista de clientes atualiza automaticamente após:
  - Criar cliente
  - Editar cliente
  - Deletar cliente
  - Realizar depósito
  - Realizar saque

### Mensagens de Erro
- Erros da API são capturados e exibidos nos modais
- Mensagens de erro aparecem no topo da tela principal
- Mensagens de sucesso com auto-dismiss (5 segundos)
- Tratamento de diferentes tipos de erro (validação, conflito, não encontrado)

## 🎨 Melhorias de UX

- Indicadores de loading
- Animações suaves nas mensagens
- Confirmação antes de deletar
- Estados de formulário (disabled durante submit)
- Feedback visual claro para todas as operações

## 📝 Notas

- Login é simulado (não conectado à API ainda)
- Credenciais aceitas: qualquer email válido
- Token JWT mock gerado localmente

## 🔒 Variáveis de Ambiente

Configure em `src/environments/`:

```typescript
// environment.ts (desenvolvimento)
export const environment = {
  production: false,
  apiUrl: 'http://localhost:5000'
};

// environment.prod.ts (produção)
export const environment = {
  production: true,
  apiUrl: 'https://api.production.com'
};
```

## 🚀 Deploy

### Docker
```bash
docker build -t case-clientes-frontend .
docker run -p 80:80 case-clientes-frontend
```

### Build manual
```bash
npm run build
# Arquivos em dist/ prontos para deploy
```
