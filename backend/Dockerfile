# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files
COPY Backend.sln ./
COPY API/API.csproj ./API/
COPY Repository/Repository.csproj ./Repository/
COPY Entities/Entities.csproj ./Entities/
COPY Application/Application.csproj ./Application/

# Restore dependencies
RUN dotnet restore Backend.sln

# Copy everything else and build
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

# Expose ports
EXPOSE 8080
EXPOSE 8081

ENTRYPOINT ["dotnet", "API.dll"]