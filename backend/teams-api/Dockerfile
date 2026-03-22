# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY TeamsService.sln ./
COPY API/API.csproj ./API/
COPY Repository/Repository.csproj ./Repository/
COPY Entities/Entities.csproj ./Entities/
COPY Application/Application.csproj ./Application/

RUN dotnet restore TeamsService.sln

COPY . .
WORKDIR /src/API
RUN dotnet build -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish API.csproj -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "API.dll"]
