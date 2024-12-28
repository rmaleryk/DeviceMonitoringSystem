#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 54320
EXPOSE 54321

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["DMS.FakeDevices.Api/DMS.FakeDevices.Api.csproj", "DMS.FakeDevices.Api/"]
RUN dotnet restore "./DMS.FakeDevices.Api/DMS.FakeDevices.Api.csproj"
COPY . .
WORKDIR "/src/DMS.FakeDevices.Api"
RUN dotnet build "./DMS.FakeDevices.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./DMS.FakeDevices.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DMS.FakeDevices.Api.dll"]