# Enterprise Ticket Management System (Work in Progress)

Enterprise Ticket Management — a work-in-progress internal web application for managing operational tickets. Currently, the project consists of the initial infrastructure and database schema.

## Features (Planned)

- **User Authentication**: Secure login system with role-based access control (Admin, Technician, User). [Planned]
- **Ticket Management**: [Planned]
  - Create, view, update, and delete tickets.
  - Categorize tickets by type (e.g., Hardware, Software, Network).
  - Set priority levels (Low, Medium, High, Critical).
  - Track ticket status (Open, In Progress, On Hold, Resolved, Closed).
- **Assignment & Workflow**: [Planned]
  - Admins can assign tickets to specific technicians.
  - Technicians can update ticket status and add internal notes.
  - Users can view their ticket history and add comments.
- **Audit Trail**: [Planned]
  - Comprehensive logging of all ticket activities.
  - Tracks who made changes and when.
- **Responsive UI**: [Planned]
  - Modern, clean interface built with DevExtreme.

## Tech Stack

### Frontend (Scaffolding Completed)
- **Framework**: Angular
- **Language**: TypeScript
- **Styling**: DevExtreme 
- **UI Components**: DevExtreme
- **State Management**: NgRx (Planned)
- **Routing**: Angular Router
- **HTTP Client**: Axios (Planned)

### Backend (Infrastructure Ready)
- **Framework**: ASP.NET Core
- **Database**: PostgreSQL (Dockerized)
- **ORM**: Entity Framework Core (Configured & Migrated)
- **Authentication**: JWT (Identity Configured, Tokens Pending)

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
│   ├── src/
│   │   ├── API/
│   │   │   ├── Controllers/              # Presentation layer (Empty)
│   │   │   ├── DataTransferObjects/      # DTOs
│   │   │   └── Program.cs
│   │   │
│   │   ├── Application/
│   │   │   ├── Interfaces/               # Service contracts (Empty)
│   │   │   ├── Services/                 # Business logic layer (Empty)
│   │   │   └── Validators/               # FluentValidation
│   │   │
│   │   ├── Domain/                       # Core business entities
│   │   │   └── Entities/                 # Domain models
│   │   │
│   │   └── Infrastructure/               # External concerns
│   │       ├── Data/                     # Data access layer
│   │       └── Migrations/               # EF Core migrations
│   │
│   └── tests/                            # Test projects (Missing)
│
├── frontend/
│   └── src/
│      └── app/
│          ├── components/                # Reusable UI components
│          ├── features/                  # Application pages/features (Auth, Tickets)
│          ├── services/                  # API and state services
│          ├── app.routes.ts              # Routing configuration
│          └── app.config.ts              # App configuration
│
└── Documentation/                        # Project documentation
```