# Etapa 1: build no SDK .NET 8
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia e restaura dependências
COPY ["*.csproj", "./"]
RUN dotnet restore

# Copia o restante do código-fonte e publica
COPY . .
RUN dotnet publish -c Release -o /app/publish

# Etapa 2: runtime runtime .NET 8
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Ajuste: se o app escuta na porta 80 ou outra, exponha ali
EXPOSE 35000

# Comando de entrada (ajuste se for console em vez de web)
ENTRYPOINT ["dotnet", "OpenPortTest.dll"]
