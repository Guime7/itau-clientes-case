# 🛠️ Comandos Úteis - Quick Reference

## 📦 NPM Scripts

```bash
# Desenvolvimento
npm start                    # Inicia servidor dev em http://localhost:4200
npm run watch               # Build com watch mode

# Build
npm run build               # Build de produção
npm run build -- --configuration development  # Build de dev

# Testes
npm test                    # Executa testes unitários
npm run test -- --code-coverage  # Testes com cobertura

# Linting & Format (se configurado)
npm run lint                # Verifica código
npm run format              # Formata código
```

## 🐳 Docker

### Desenvolvimento
```bash
# Build e start
docker-compose --profile dev up

# Com rebuild forçado
docker-compose --profile dev up --build

# Em background
docker-compose --profile dev up -d

# Ver logs
docker-compose logs -f frontend-dev

# Parar
docker-compose down
```

### Produção
```bash
# Build e start
docker-compose --profile prod up

# Com rebuild forçado
docker-compose --profile prod up --build

# Em background
docker-compose --profile prod up -d

# Ver logs
docker-compose logs -f frontend-prod

# Parar
docker-compose down
```

### Docker Avançado
```bash
# Remover volumes
docker-compose down -v

# Rebuild sem cache
docker-compose build --no-cache

# Verificar containers
docker ps

# Entrar no container
docker exec -it <container_id> sh

# Ver uso de recursos
docker stats

# Limpar tudo (cuidado!)
docker system prune -a
```

## 🔧 Angular CLI

```bash
# Gerar componente
ng generate component components/nome-do-componente
ng g c components/nome-do-componente --standalone

# Gerar service
ng generate service services/nome-do-service
ng g s services/nome-do-service

# Gerar guard
ng generate guard guards/nome-do-guard
ng g g guards/nome-do-guard

# Gerar interceptor
ng generate interceptor interceptors/nome
ng g interceptor interceptors/nome

# Gerar interface/model
ng generate interface models/nome
ng g i models/nome

# Informações do projeto
ng version
ng analytics off  # Desabilitar analytics
```

## 🌐 Servidor Web

```bash
# Servir build de produção localmente
npx http-server dist/case-clientes/browser -p 8080

# Com CORS habilitado
npx http-server dist/case-clientes/browser -p 8080 --cors

# Nginx (se instalado localmente)
nginx -t  # Testar configuração
nginx -s reload  # Recarregar configuração
```

## 🧪 Testes & Debugging

```bash
# Testes em modo watch
npm test -- --watch

# Testes específicos
npm test -- --include='**/auth.service.spec.ts'

# Build com source maps
npm run build -- --source-map

# Analisar bundle size
npm run build -- --stats-json
npx webpack-bundle-analyzer dist/case-clientes/browser/stats.json
```

## 🔍 Inspeção & Debugging

```bash
# Ver dependências desatualizadas
npm outdated

# Verificar vulnerabilidades
npm audit
npm audit fix

# Limpar cache
npm cache clean --force

# Reinstalar dependências
rm -rf node_modules package-lock.json
npm install

# Ver tamanho dos node_modules
du -sh node_modules
```

## 📊 Git

```bash
# Status
git status

# Adicionar mudanças
git add .
git add src/app/components/

# Commit
git commit -m "feat: adiciona componente de login"

# Ver diff
git diff
git diff --staged

# Ver histórico
git log --oneline
git log --graph --oneline --all

# Criar branch
git checkout -b feature/nome-da-feature

# Push
git push origin main
```

## 🔐 Ambiente & Variáveis

```bash
# Windows (PowerShell)
$env:API_URL = "http://localhost:5000"
npm start

# Windows (CMD)
set API_URL=http://localhost:5000 && npm start

# Linux/Mac
export API_URL=http://localhost:5000
npm start

# Ou criar arquivo .env e usar dotenv
```

## 🚀 Deploy

### Build Otimizado
```bash
# Produção com otimizações
npm run build -- --configuration production

# Com base href customizado
npm run build -- --base-href=/app/

# Com output path customizado
npm run build -- --output-path=custom-dist
```

### Deploy em Serviços

```bash
# Netlify
npm install -g netlify-cli
netlify deploy --prod --dir=dist/case-clientes/browser

# Vercel
npm install -g vercel
vercel --prod

# GitHub Pages
npm install -g angular-cli-ghpages
ng build --base-href=/nome-do-repo/
npx angular-cli-ghpages --dir=dist/case-clientes/browser

# AWS S3
aws s3 sync dist/case-clientes/browser s3://meu-bucket --delete
```

## 🔄 Atualização de Dependências

```bash
# Atualizar Angular CLI globalmente
npm uninstall -g @angular/cli
npm install -g @angular/cli@latest

# Atualizar projeto Angular
ng update @angular/core @angular/cli

# Atualizar todas as dependências (cuidado!)
npx npm-check-updates -u
npm install

# Atualizar apenas dependências seguras
npm update
```

## 🐛 Troubleshooting

```bash
# Erro: Port 4200 em uso
# Windows
netstat -ano | findstr :4200
taskkill /PID <PID> /F

# Linux/Mac
lsof -ti:4200 | xargs kill -9

# Erro: node_modules corrompido
rm -rf node_modules package-lock.json
npm install

# Erro: Angular CLI não encontrado
npm install -g @angular/cli
npm link @angular/cli

# Limpar cache do Angular
rm -rf .angular/cache

# Reiniciar do zero
rm -rf node_modules package-lock.json .angular
npm install
```

## 📱 Testes em Dispositivos

```bash
# Obter IP local
# Windows
ipconfig

# Linux/Mac
ifconfig

# Iniciar servidor acessível na rede
ng serve --host 0.0.0.0 --port 4200

# Acessar de outro dispositivo
# http://SEU_IP:4200
```

## 🎯 Performance

```bash
# Lighthouse audit
npm install -g lighthouse
lighthouse http://localhost:4200 --view

# Bundle analyzer
npm run build -- --stats-json
npx webpack-bundle-analyzer dist/case-clientes/browser/stats.json

# Verificar tamanho do build
du -sh dist/case-clientes/browser
```

## 💾 Backup & Restore

```bash
# Backup de configurações importantes
tar -czf backup.tar.gz \
  package.json \
  package-lock.json \
  angular.json \
  tsconfig.json \
  src/environments/

# Restore
tar -xzf backup.tar.gz
```

---

**💡 Dica:** Adicione aliases no seu shell para comandos frequentes!

```bash
# Exemplo de aliases (~/.bashrc ou ~/.zshrc)
alias ngstart="npm start"
alias ngbuild="npm run build"
alias ddup="docker-compose --profile dev up"
alias dddown="docker-compose down"
```
