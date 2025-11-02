#!/usr/bin/env bash

TEST_PROJECT="tests/Garage.Tests/Garage.Tests.csproj"

dotnet test "$TEST_PROJECT" --no-build --verbosity normal
