# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copia o solution e todos os csproj
COPY *.sln ./
COPY src/ ./src/
COPY workers/ ./workers/
COPY tests/ ./tests/

# Restaura dependências
RUN dotnet restore

# Copia o restante do código
COPY . .

# Publica a solution inteira para uma única pasta
RUN dotnet publish -c Release -o /app/publish

# Permite executar script de migrations
RUN chmod +x scripts/apply-migrations.sh

# Instalar dotnet-ef global
RUN dotnet tool install --global dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish ./
COPY --from=build /app/src ./src
COPY --from=build /app/workers ./workers
COPY --from=build /app/scripts ./scripts
COPY --from=build /app/tests ./tests/
COPY --from=build /app/*.sln ./

EXPOSE 5001

# Migrations stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS migrations
WORKDIR /app

# Copia toda a estrutura
COPY --from=build /app ./

# Garante que dotnet-ef está disponível
RUN dotnet tool install --global dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"
