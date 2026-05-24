FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["AbsensiApp/AbsensiApp.API/AbsensiApp.API.csproj", "AbsensiApp/AbsensiApp.API/"]
COPY ["AbsensiApp/AbsensiApp.Application/AbsensiApp.Application.csproj", "AbsensiApp/AbsensiApp.Application/"]
COPY ["AbsensiApp/AbsensiApp.Domain/AbsensiApp.Domain.csproj", "AbsensiApp/AbsensiApp.Domain/"]
COPY ["AbsensiApp/AbsensiApp.Infrastructure/AbsensiApp.Infrastructure.csproj", "AbsensiApp/AbsensiApp.Infrastructure/"]

RUN dotnet restore "AbsensiApp/AbsensiApp.API/AbsensiApp.API.csproj"

COPY . .

RUN dotnet build "AbsensiApp/AbsensiApp.API/AbsensiApp.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AbsensiApp/AbsensiApp.API/AbsensiApp.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AbsensiApp.API.dll"]