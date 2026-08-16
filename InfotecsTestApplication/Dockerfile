FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

WORKDIR /app

EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

ARG BUILD_CONFIGURATION=Release

WORKDIR /src

COPY ["InfotecsTestApplication/InfotecsTestApplication.csproj", "InfotecsTestApplication/"]

RUN dotnet restore "InfotecsTestApplication/InfotecsTestApplication.csproj"

COPY . .

WORKDIR "/src/InfotecsTestApplication"

RUN dotnet build "InfotecsTestApplication.csproj" \
    -c $BUILD_CONFIGURATION \
    -o /app/build


FROM build AS test
WORKDIR "src/InfotecsTestApplication.Tests"
RUN dotnet test "InfotecsTestApplication.Tests.csproj" \
        -c $BUILD_CONFIGURATION \
        --no-restore \
        --no-build \
        --logger "console;verbosity=normal"

FROM test AS publish

ARG BUILD_CONFIGURATION=Release

RUN dotnet publish "InfotecsTestApplication.csproj" \
    -c $BUILD_CONFIGURATION \
    -o /app/publish \
    /p:UseAppHost=false


FROM base AS final

WORKDIR /app

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "InfotecsTestApplication.dll"]