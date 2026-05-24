FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["AbsensiApp.API/AbsensiApp.API.csproj", "AbsensiApp.API/"]
COPY ["AbsensiApp.Application/AbsensiApp.Application.csproj", "AbsensiApp.Application/"]
COPY ["AbsensiApp.Domain/AbsensiApp.Domain.csproj", "AbsensiApp.Domain/"]
COPY ["AbsensiApp.Infrastructure/AbsensiApp.Infrastructure.csproj", "AbsensiApp.Infrastructure/"]

RUN dotnet restore "AbsensiApp.API/AbsensiApp.API.csproj"

COPY . .

RUN dotnet build "AbsensiApp.API/AbsensiApp.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AbsensiApp.API/AbsensiApp.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AbsensiApp.API.dll"]