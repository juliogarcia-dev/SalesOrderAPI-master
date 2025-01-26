# Use a imagem base do .NET
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Use a imagem do SDK do .NET para construir o projeto
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copia o arquivo csproj e restaura dependências
COPY ["SalesOrderAPI.csproj", "./"]
RUN dotnet restore "SalesOrderAPI.csproj"

# Copia todos os arquivos restantes
COPY . . 
WORKDIR "/src"
RUN dotnet build "SalesOrderAPI.csproj" -c Release -o /app/build

# Publica a aplicação
FROM build AS publish
RUN dotnet publish "SalesOrderAPI.csproj" -c Release -o /app/publish

# Usa a imagem base para rodar a aplicação
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SalesOrderAPI.dll"]