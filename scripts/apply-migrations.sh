#!/usr/bin/env bash

PROJECT_DEFAULT="src/Garage.Infrastructure/Garage.Infrastructure.csproj"
STARTUP_DEFAULT="src/Garage.Api/Garage.Api.csproj"
CONTEXT_DEFAULT="GarageContext"
CONN_STRING=${ConnectionStrings__GarageDatabase}

dotnet ef database update \
    --project "$PROJECT_DEFAULT" \
    --startup-project "$STARTUP_DEFAULT" \
    --context "$CONTEXT_DEFAULT" \
    --connection "$CONN_STRING"
