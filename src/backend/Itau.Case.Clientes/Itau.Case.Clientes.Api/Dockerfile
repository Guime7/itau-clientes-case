# Acesse https://aka.ms/customizecontainer para saber como personalizar seu contêiner de depuração e como o Visual Studio usa este Dockerfile para criar suas imagens para uma depuração mais rápida.

# Define argumentos para configuração de porta
ARG HTTP_PORT=8080

# Esta fase é usada durante a execução no VS no modo rápido (Padrão para a configuração de Depuração)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
ARG HTTP_PORT=8080
WORKDIR /app
EXPOSE ${HTTP_PORT}


# Esta fase é usada para compilar o projeto de serviço
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Itau.Case.Clientes.Api/Itau.Case.Clientes.Api.csproj", "Itau.Case.Clientes.Api/"]
RUN dotnet restore "./Itau.Case.Clientes.Api/Itau.Case.Clientes.Api.csproj"
COPY . .
WORKDIR "/src/Itau.Case.Clientes.Api"
RUN dotnet build "./Itau.Case.Clientes.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Esta fase é usada para publicar o projeto de serviço a ser copiado para a fase final
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Itau.Case.Clientes.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Esta fase é usada na produção ou quando executada no VS no modo normal (padrão quando não está usando a configuração de Depuração)
FROM base AS final
ARG HTTP_PORT=8080
WORKDIR /app
COPY --from=publish /app/publish .

# Configurar apenas HTTP (sem HTTPS para evitar problemas de certificado)
ENV ASPNETCORE_URLS=http://+:${HTTP_PORT}
ENV ASPNETCORE_ENVIRONMENT=Development

ENTRYPOINT ["dotnet", "Itau.Case.Clientes.Api.dll"]


