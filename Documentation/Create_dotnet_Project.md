#Create Solution
dotnet new sln -n [name]

# Create Project
dotnet new [type] -n [name] -o [path]

# Add project to solution
dotnet sln add [path]/[name].csproj

# Set up project references (Clean Architecture dependencies)
cd [path]
dotnet add reference [path_new_reference]/[name_new_reference].csproj

# List all .NET templates
dotnet new list

# Create specific project types
dotnet new webapi -n [name]
dotnet new classlib -n [name]
dotnet new xunit -n [name]
dotnet new console -n [name]

# Manage packages
dotnet add package [PackageName]
dotnet remove package [PackageName]
dotnet list package

# Build and run
dotnet build
dotnet run
dotnet test
dotnet watch run

# Clean build artifacts
dotnet clean

# Create Migration
cd backend/Repository
dotnet ef migrations add [Name] 
dotnet ef database update