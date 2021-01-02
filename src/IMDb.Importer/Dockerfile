#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:5.0-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["src/IMDb.Importer/IMDb.Importer.csproj", "src/IMDb.Importer/"]
COPY ["src/IMDb.Core/IMDb.Core.csproj", "src/IMDb.Core/"]
COPY ["src/IMDb.Infrastructure/IMDb.Infrastructure.csproj", "src/IMDb.Infrastructure/"]
RUN dotnet restore "src/IMDb.Importer/IMDb.Importer.csproj"
COPY . .
WORKDIR "/src/src/IMDb.Importer"
RUN dotnet build "IMDb.Importer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "IMDb.Importer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IMDb.Importer.dll"]