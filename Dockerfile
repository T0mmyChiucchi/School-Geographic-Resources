FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["SchoolGeoResources.Api/SchoolGeoResources.Api.csproj", "SchoolGeoResources.Api/"]
COPY ["SchoolGeoResources.Application/SchoolGeoResources.Application.csproj", "SchoolGeoResources.Application/"]
COPY ["SchoolGeoResources.Domain/SchoolGeoResources.Domain.csproj", "SchoolGeoResources.Domain/"]
COPY ["SchoolGeoResources.Infrastructure/SchoolGeoResources.Infrastructure.csproj", "SchoolGeoResources.Infrastructure/"]
RUN dotnet restore "./SchoolGeoResources.Api/SchoolGeoResources.Api.csproj"
COPY . .
WORKDIR "/src/SchoolGeoResources.Api"
RUN dotnet build "./SchoolGeoResources.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./SchoolGeoResources.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SchoolGeoResources.Api.dll"]
