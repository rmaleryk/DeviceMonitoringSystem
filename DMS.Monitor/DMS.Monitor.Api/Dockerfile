# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["DMS.Monitor.Api/DMS.Monitor.Api.csproj", "DMS.Monitor.Api/"]
COPY ["DMS.Monitor.Application.Shared/DMS.Monitor.Application.Shared.csproj", "DMS.Monitor.Application.Shared/"]
COPY ["DMS.Monitor.Domain/DMS.Monitor.Domain.csproj", "DMS.Monitor.Domain/"]
COPY ["DMS.Monitor.Infrastructure/DMS.Monitor.Infrastructure.csproj", "DMS.Monitor.Infrastructure/"]
COPY ["DMS.Monitor.Application.Write/DMS.Monitor.Application.Write.csproj", "DMS.Monitor.Application.Write/"]
COPY ["DMS.Monitor.Contracts/DMS.Monitor.Contracts.csproj", "DMS.Monitor.Contracts/"]
COPY ["DMS.Monitor.Application.Read/DMS.Monitor.Application.Read.csproj", "DMS.Monitor.Application.Read/"]
RUN dotnet restore "./DMS.Monitor.Api/DMS.Monitor.Api.csproj"
COPY . .
WORKDIR "/src/DMS.Monitor.Api"
RUN dotnet build "./DMS.Monitor.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./DMS.Monitor.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DMS.Monitor.Api.dll"]