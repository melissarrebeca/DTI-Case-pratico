# Estágio de build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copiar o projeto e restaurar as dependências
COPY *.csproj ./
RUN dotnet restore

# Copiar todo o código e fazer o build
COPY . ./
RUN dotnet publish -c Release -o out

# Estágio de runtime
FROM mcr.microsoft.com/dotnet/runtime:9.0
WORKDIR /app
COPY --from=build /app/out .

# Criar diretórios necessários
RUN mkdir -p ./Database
RUN mkdir -p ./Logs

# Definir o comando de entrada
ENTRYPOINT ["dotnet", "DTILivraria.dll"]