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

# Copy the NuGet.Config file to configure the GitHub package source
COPY ["DMS.Client.Api/NuGet.Config", "/root/.nuget/NuGet/NuGet.Config"]

# Set the environment variable for the GitHub token (pass this dynamically during build)
ARG GITHUB_TOKEN
ENV GITHUB_TOKEN=${GITHUB_TOKEN}
ARG GITHUB_USER
ENV GITHUB_USER=${GITHUB_USER}

# Copy and restore the project
COPY ["DMS.Client.Api/DMS.Client.Api.csproj", "DMS.Client.Api/"]
RUN dotnet restore "./DMS.Client.Api/DMS.Client.Api.csproj"
COPY . .
WORKDIR "/src/DMS.Client.Api"
RUN dotnet build "./DMS.Client.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./DMS.Client.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DMS.Client.Api.dll"]