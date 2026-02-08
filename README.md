# Enterprise Ticket Management System

Enterprise Ticket Management — an internal web application for managing operational tickets through role-based workflows with full auditability.

## Features

- **User Authentication**: Secure login system with role-based access control (Admin, Technician, User).
- **Ticket Management**:
  - Create, view, update, and delete tickets.
  - Categorize tickets by type (e.g., Hardware, Software, Network).
  - Set priority levels (Low, Medium, High, Critical).
  - Track ticket status (Open, In Progress, On Hold, Resolved, Closed).
- **Assignment & Workflow**:
  - Admins can assign tickets to specific technicians.
  - Technicians can update ticket status and add internal notes.
  - Users can view their ticket history and add comments.
- **Audit Trail**:
  - Comprehensive logging of all ticket activities.
  - Tracks who made changes and when.
- **Responsive UI**:
  - Modern, clean interface built with DevExtreme.

## Tech Stack

### Frontend
- **Framework**: Angular
- **Language**: TypeScript
- **Styling**: DevExtreme
- **UI Components**: DevExtreme
- **State Management**: NgRx
- **Routing**: Angular Router
- **HTTP Client**: Axios

### Backend
- **Framework**: ASP.NET Core
- **Database**: PostgreSQL
- **ORM**: Entity Framework Core
- **Authentication**: JWT (JSON Web Tokens)

## Prerequisites

- Node.js (v18 or higher)
- .NET 10 SDK
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
`http://localhost:8000/docs`

## Project Structure

```
Enterprise-Ticket-Management/
├── backend/
│   ├── src/
│   │   ├── API/                          
│   │   │   ├── Controllers/              # Presentation layer (Controllers, Middleware)
│   │   │   └── Program.cs
│   │   │
│   │   ├── Application/                  
│   │   │   ├── Services/                 # Business logic layer
│   │   │   ├── Interfaces/               # Service contracts
│   │   │   ├── DTOs/                     # Request/Response models
│   │   │   └── Validators/               # FluentValidation
│   │   │
│   │   ├── Domain/                       # Core business entities
│   │   │   ├── Entities/                 # Your domain models (Ticket, User, etc.)
│   │   │   └── Interfaces/               # Repository contracts
│   │   │
│   │   └── Infrastructure/               # External concerns
│   │       ├── Data/
│   │       │   ├── Repositories/         # Data access layer
│   │       │   ├── Configurations/       # EF Core entity configs
│   │       │   └── ApplicationDbContext.cs
│   │       │
│   │       ├── Migrations/               # EF Core migrations
│   │       └── Authentication/           # JWT implementation
│   │
│   └── tests/                            # Test projects
│       ├── API.Tests/
│       ├── Application.Tests/
│       └── Infrastructure.Tests/
│
├── Common/                                 # Common code for backend and frontend
│   └── DTOs/                               # Data Transfer Objects
│
├── frontend/
│   └── src/
│      ├── app/
│      │   └── core/                   
│      │       └── services/
│      │           └── Contracts/
│      │
│      ├── features/  
│      │   └── auth/
│      │
│      └── shared/                          # Reusable components/pipes/directives
│          ├── components/
│          └── viewModels/
│   
└── README.md                              # Project documentation
```