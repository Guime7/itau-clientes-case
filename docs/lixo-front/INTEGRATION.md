# Guia de Integração Frontend + Backend

Este guia explica como conectar o frontend Angular com o backend .NET Core.

## 🔌 Configuração da API

### 1. Ajustar URL da API

Edite o arquivo `src/environments/environment.ts`:

```typescript
export const environment = {
  production: false,
  apiUrl: 'http://localhost:5000' // Ajuste a porta conforme sua API
};
```

### 2. Configurar CORS no Backend

No backend .NET Core, certifique-se de configurar CORS no `Program.cs`:

```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        builder => builder
            .WithOrigins("http://localhost:4200") // URL do Angular em dev
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});

// ...

app.UseCors("AllowAngular");
```

### 3. Implementar Autenticação JWT no Backend

O frontend já está preparado para receber JWT. Você precisa criar um endpoint de autenticação no backend:

**Endpoint sugerido:** `POST /api/auth/login`

**Request:**
```json
{
  "email": "usuario@email.com",
  "senha": "senha123"
}
```

**Response:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "email": "usuario@email.com"
}
```

**Atualizar AuthService:**

Após criar o endpoint no backend, atualize o método `login()` em `auth.service.ts`:

```typescript
login(credentials: LoginRequest): Observable<LoginResponse> {
  return this.http.post<LoginResponse>(`${environment.apiUrl}/api/auth/login`, credentials)
    .pipe(
      tap((response: LoginResponse) => {
        this.saveToken(response.token);
        this.saveUser(response.email);
        this.currentUserSubject.next({ email: response.email });
      })
    );
}
```

### 4. Validar JWT no Backend

Configure a validação de JWT no backend:

```csharp
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "your-issuer",
            ValidAudience = "your-audience",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your-secret-key"))
        };
    });
```

Adicione o atributo `[Authorize]` nos controllers:

```csharp
[ApiController]
[Route("api/clientes")]
[Authorize] // Requer autenticação JWT
public class ClientesController : ControllerBase
{
    // ...
}
```

## 🧪 Testando a Integração

### 1. Iniciar o Backend
```bash
cd backend/Itau.Case.Clientes
dotnet run
```

### 2. Iniciar o Frontend
```bash
cd frontend/case-clientes
npm start
```

### 3. Testar Fluxo Completo

1. Acesse `http://localhost:4200`
2. Faça login (atualmente simulado)
3. Verifique se a tabela carrega os clientes do backend
4. Teste criar, editar, deletar clientes
5. Teste depositar e sacar saldo

## 🐛 Troubleshooting

### Erro de CORS

Se receber erro de CORS, verifique:
- CORS está configurado corretamente no backend
- A origem (URL) do frontend está permitida
- O middleware `UseCors()` está na ordem correta no pipeline

### Token não está sendo enviado

Verifique:
- O interceptor está registrado em `app.config.ts`
- O token está sendo salvo no localStorage após login
- O header `Authorization: Bearer <token>` está presente nas requisições

### Backend retorna 401 Unauthorized

Verifique:
- O token JWT está válido
- A configuração de validação do JWT no backend está correta
- O middleware `UseAuthentication()` está antes de `UseAuthorization()`

## 📦 Deploy

### Frontend (Nginx)

```bash
# Build do frontend
npm run build

# O output estará em dist/case-clientes/browser
# Configure o nginx.conf para servir esses arquivos
```

### Backend + Frontend (Docker Compose)

Crie um `docker-compose.yml` na raiz do projeto:

```yaml
version: '3.8'

services:
  backend:
    build: ./backend/Itau.Case.Clientes
    ports:
      - "5000:5000"
    environment:
      - ASPNETCORE_ENVIRONMENT=Production
    
  frontend:
    build: ./frontend/case-clientes
    ports:
      - "80:80"
    depends_on:
      - backend
    environment:
      - API_URL=http://backend:5000
```

Execute:
```bash
docker-compose up -d
```

## 🔐 Segurança

### Recomendações

1. **Nunca commite secrets:** Use variáveis de ambiente para tokens e chaves
2. **HTTPS em produção:** Use SSL/TLS em produção
3. **Tokens com expiração:** Configure tempo de expiração adequado para JWT
4. **Refresh tokens:** Implemente refresh tokens para melhor UX
5. **Sanitização de inputs:** Valide e sanitize todas as entradas do usuário
6. **Rate limiting:** Implemente rate limiting no backend
7. **CSP Headers:** Configure Content Security Policy headers

## 📚 Referências

- [Angular HTTP Client](https://angular.dev/guide/http)
- [JWT Authentication](https://jwt.io/introduction)
- [ASP.NET Core Authentication](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/)
- [CORS in ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/security/cors)
