FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

COPY *.sln ./
COPY src/ ./src/
COPY workers/ ./workers/
COPY tests/ ./tests/

RUN dotnet restore

COPY . .

RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish ./
COPY --from=build /app/scripts ./scripts
RUN chmod +x scripts/apply-migrations.sh

EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS migrations
WORKDIR /app

COPY --from=build /app ./

RUN dotnet tool install --global dotnet-ef --version 8.0.0
ENV PATH="$PATH:/root/.dotnet/tools"
