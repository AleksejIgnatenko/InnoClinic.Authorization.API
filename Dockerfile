# Этот этап используется при запуске из VS в быстром режиме (по умолчанию для конфигурации отладки)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Этот этап используется для сборки проекта службы
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["InnoClinic.Authorization.API/InnoClinic.Authorization.API.csproj", "InnoClinic.Authorization.API/"]
COPY ["InnoClinic.Authorization.Application/InnoClinic.Authorization.Application.csproj", "InnoClinic.Authorization.Application/"]
COPY ["InnoClinic.Authorization.Core/InnoClinic.Authorization.Core.csproj", "InnoClinic.Authorization.Core/"]
COPY ["InnoClinic.Authorization.DataAccess/InnoClinic.Authorization.DataAccess.csproj", "InnoClinic.Authorization.DataAccess/"]
COPY ["InnoClinic.Authorization.Infrastructure/InnoClinic.Authorization.Infrastructure.csproj", "InnoClinic.Authorization.Infrastructure/"]

RUN dotnet restore "InnoClinic.Authorization.API/InnoClinic.Authorization.API.csproj"

COPY . .

WORKDIR "/src/InnoClinic.Authorization.API"
RUN dotnet build "InnoClinic.Authorization.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
RUN dotnet publish "InnoClinic.Authorization.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "InnoClinic.Authorization.API.dll"]