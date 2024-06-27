#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["SteamAccountsHub.WebApi/SteamAccountsHub.WebApi.csproj", "SteamAccountsHub.WebApi/"]
RUN dotnet restore "./SteamAccountsHub.WebApi/./SteamAccountsHub.WebApi.csproj"
COPY . .
WORKDIR "/src/SteamAccountsHub.WebApi"
RUN dotnet build "./SteamAccountsHub.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./SteamAccountsHub.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SteamAccountsHub.WebApi.dll"]