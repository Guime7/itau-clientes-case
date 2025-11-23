@echo off
echo ========================================
echo Instalando dependencias do Frontend
echo ========================================
echo.

cd src\frontend\case-clientes

echo Instalando pacotes NPM...
call npm install

if %ERRORLEVEL% NEQ 0 (
    echo.
    echo [ERRO] Falha ao instalar dependencias
    pause
    exit /b 1
)

echo.
echo ========================================
echo Instalacao concluida com sucesso!
echo ========================================
echo.
echo Comandos disponiveis:
echo   npm start           - Inicia servidor de desenvolvimento
echo   npm test            - Executa testes unitarios
echo   npm run test:ui     - Executa testes com interface
echo   npm run test:coverage - Gera relatorio de cobertura
echo   npm run build       - Build de producao
echo.
pause
