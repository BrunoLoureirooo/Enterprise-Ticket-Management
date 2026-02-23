# Enterprise Ticket Management System (Work in Progress)

Enterprise Ticket Management — a work-in-progress internal web application for managing operational tickets. The project currently has a functional authentication system, user management API, and core frontend scaffolding in place.

## Features (Planned)

- **User Authentication**: 
  - Secure login system with role-based access control (Admin, TeamLeader, Technician). [Implemented]
  - User registration [Implemented]
  - User management [Planned]
  - User roles [Planned]
  - User permissions [Planned]
- **Team Management**: [Planned]
  - Create, view, update, and delete teams.
  - Assign users to teams.
- **Ticket Management**: [Planned]
  - Create, view, update, and delete tickets.
  - Categorize tickets by type (e.g., Hardware, Software, Network).
  - Set priority levels (Low, Medium, High, Critical).
  - Track ticket status (Open, In Progress, On Hold, Resolved, Closed).
- **Assignment & Workflow**: [Planned]
  - TeamLeaders can assign tickets to specific technicians.
  - Technicians can update ticket status and add internal notes.
- **Audit Trail**: [Planned]
  - Comprehensive logging of all ticket activities.
  - Tracks who made changes and when.

## Tech Stack

### Frontend (Scaffolding Completed)
- **Framework**: Angular
- **Language**: TypeScript
- **UI Components**: DevExtreme
- **State Management**: NgRx (Planned)
- **Routing**: Angular Router
- **HTTP Client**: Angular HttpClient

### Backend (Auth & User API Ready)
- **Framework**: ASP.NET Core (.NET 8)
- **Database**: PostgreSQL (Dockerized)
- **ORM**: Entity Framework Core (Configured & Migrated)
- **Authentication**: JWT Bearer + ASP.NET Core Identity (Implemented)
- **Mapping**: AutoMapper
- **Documentation**: Swagger / OpenAPI

## Prerequisites

- Node.js (v18 or higher)
- .NET 8 SDK
- PostgreSQL (v12 or higher)
- npm or yarn

## Usage

### Default Credentials

After running the migrations, you can create an admin user via the API or use these defaults if seeded:

- **Admin**: [EMAIL_ADDRESS] / admin
- **Technician**: [EMAIL_ADDRESS] / tech
- **User**: [EMAIL_ADDRESS] / user

- **Passowrd**: [PASSWORD] / a

### API Documentation

Interactive API documentation (Swagger UI) is available at:
`http://localhost:5000/swagger`

## Project Structure

```
Enterprise-Ticket-Management/
├── backend/
│   ├── API/
│   │   ├── ActionFilters/            # Validation filter attribute
│   │   ├── Controllers/              # AuthController, UserController
│   │   └── Program.cs                # App bootstrap, DI, middleware
│   │
│   ├── Application/
│   │   ├── Services/                 # AuthService, UserService, LoggerManager, ServiceManager
│   │   │   └── Contracts/            # IAuthService, IUserService, IServiceManager, ILoggerManager
│   │   └── MappingProfile.cs         # AutoMapper profiles
│   │
│   ├── Entities/
│   │   ├── Models/                   # ApplicationUser, ApplicationRole
│   │   ├── DataTransferObjects/      # TokenDto, User DTOs (register, login, response)
│   │   ├── Enums/                    # Domain enumerations
│   │   └── JWTConfiguration.cs       # JWT settings model
│   │
│   └── Repository/
│       ├── Configuration/            # EF Core seed configs (User, Role, UserRole)
│       ├── Contracts/                # IRepositoryManager
│       ├── Migrations/               # EF Core migrations
│       ├── RepositoryContext.cs      # DbContext
│       ├── RepositoryContextFactory.cs
│       └── RepositoryManager.cs
│
├── frontend/
│   └── src/
│       ├── app/
│       │   ├── app.ts                # Root component
│       │   ├── app.routes.ts         # Routing configuration
│       │   └── app.config.ts         # App configuration
│       │
│       ├── core/
│       │   └── services/             # NavService (navigation state)
│       │
│       ├── features/
│       │   ├── auth/login/           # Login page
│       │   ├── home/                 # Dashboard page
│       │   ├── tickets/              # Tickets page + My Tickets sub-page
│       │   ├── users/                # Users management page
│       │   └── settings/             # Settings page
│       │
│       └── shared/
│           └── components/Layout/    # App shell (sidebar, toolbar, navigation)
│
├── https/                            # Dev SSL certificate for Kestrel (Docker)
├── docker-compose.yml                # PostgreSQL, Backend, Frontend, pgAdmin
└── Documentation/                    # Project documentation
```