FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["src/Garage.Api/Garage.Api.csproj", "src/Garage.Api/"]
COPY ["workers/Garage.Worker/Garage.Worker.csproj", "workers/Garage.Worker/"]

RUN dotnet restore "src/Garage.Api/Garage.Api.csproj"
RUN dotnet restore "workers/Garage.Worker/Garage.Worker.csproj"

COPY . .

COPY appsettings.json src/Garage.Api/appsettings.json
COPY appsettings.json workers/Garage.Worker/appsettings.json

COPY scripts/apply-migrations.sh scripts/apply-migrations.sh
RUN chmod +x scripts/apply-migrations.sh

RUN dotnet publish "src/Garage.Api/Garage.Api.csproj" -c Release -o /app/api /p:UseAppHost=false
RUN dotnet publish "workers/Garage.Worker/Garage.Worker.csproj" -c Release -o /app/worker /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

COPY --from=build /app/api /app/api
COPY --from=build /app/worker /app/worker

EXPOSE 80
ENV ASPNETCORE_URLS="http://+:80"