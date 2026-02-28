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

### Frontend (Core Features Scaffolding Completed)
- **Framework**: Angular 18
- **Language**: TypeScript
- **State Management**: NgRx (Planned)
- **Routing**: Angular Router
- **UI Components**: DevExtreme & Tailwind CSS (Planned/WIP)
- **HTTP Client**: Angular HttpClient

### Backend (Auth & User API Ready)
- **Framework**: ASP.NET Core (.NET 8)
- **Database**: PostgreSQL (Dockerized)
- **ORM**: Entity Framework Core
- **Authentication**: JWT Bearer + ASP.NET Core Identity
- **API Gateway**: YARP (Yet Another Reverse Proxy)
- **Error Tracking**: Sentry SDK
- **Mapping**: AutoMapper
- **Documentation**: Swagger / OpenAPI

## Prerequisites

- Node.js (v18 or higher)
- .NET 8 SDK
- PostgreSQL (v12 or higher)
- npm or yarn
- **Environment File**: A `.env` file must be created at the root directory containing `SECRET` (for JWT), `DB_PASSWORD`, and any `SENTRY_DSN` variables required by `docker-compose.yml`.

## Usage

### Default Credentials

After running the migrations, you can create an admin user via the API or use these defaults if seeded:

- **Admin**: admin@admin.pt

- **Password**: a

### API Documentation

Interactive API documentation (Swagger UI) is available at:
`http://localhost:5002/swagger` - Identity API

## Project Structure

```
Enterprise-Ticket-Management/
├── backend/
│   ├── gateway-api/                  # YARP API Gateway (.NET 8)
│   │   ├── API/
│   │   │   ├── appsettings.json      # Reverse proxy routing config
│   │   │   └── Program.cs            # Gateway bootstrap
│   │   └── Dockerfile
│   │
│   └── identity-api/                 # Authentication & User Management Service
│       ├── API/
│       │   ├── ActionFilters/            # Validation filter attribute
│       │   ├── Controllers/              # AuthController, UserController
│       │   └── Program.cs                # App bootstrap, DI, middleware
│       │
│       ├── Application/
│       │   ├── Services/                 # AuthService, UserService, LoggerManager, ServiceManager
│       │   │   └── Contracts/            # IAuthService, IUserService, IServiceManager, ILoggerManager
│       │   └── MappingProfile.cs         # AutoMapper profiles
│       │
│       ├── Entities/
│       │   ├── Models/                   # ApplicationUser, ApplicationRole
│       │   ├── DataTransferObjects/      # TokenDto, User DTOs (register, login, response)
│       │   ├── Enums/                    # Domain enumerations
│       │   └── JWTConfiguration.cs       # JWT settings model
│       │
│       └── Repository/
│           ├── Configuration/            # EF Core seed configs (User, Role, UserRole)
│           ├── Contracts/                # IRepositoryManager
│           ├── Migrations/               # EF Core migrations
│           ├── RepositoryContext.cs      # DbContext
│           ├── RepositoryContextFactory.cs
│           └── RepositoryManager.cs
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

---

## Roadmap / To Implement

> This section tracks the major architectural and operational improvements planned for the project.

### 1. Microservices Architecture Migration

Refactor the current monolithic backend into three focused microservices. Each service will own its domain, database, and API contract.

|      Service       |                  Responsibility                       |      Status      |
|--------------------|-------------------------------------------------------|------------------|
| `identity-service` | Authentication, JWT issuance & user management        | **Implemented**  |
| `ticket-service`   | Ticket lifecycle, teams, assignments & workflows      | Planned          |
| `api-gateway`      | Single entry point, routing & rate limiting           | **Implemented**  |

**Key decisions / technologies to evaluate:**
- **Inter-service communication**: REST (synchronous) [Implemented]
                                   Message broker **RabbitMQ** (async events) [Planned]
- **API Gateway**: YARP (Yet Another Reverse Proxy) for .NET or AWS API Gateway [Implemented]
- **Service discovery**: Docker Compose (local) [Implemented]
                       → AWS ECS Service Connect or AWS Cloud Map (production) [Planned]
---

### 2. CI/CD — GitHub Actions

Automate testing, building, and deployment for every push and pull request.

**Planned workflows:**

```
.github/workflows/
├── ci.yml            # Lint, build & unit-test on every PR
├── cd-staging.yml    # Build Docker images and deploy to AWS staging on merge to `develop`
└── cd-production.yml # Deploy to AWS production on merge to `main` (with manual approval gate)
```

**Pipeline stages per workflow:**
1. **Lint & format check** — backend (`dotnet format`) + frontend (`eslint` / `prettier`)
2. **Unit & integration tests** — `dotnet test` + `ng test --watch=false`
3. **Docker image build** — multi-stage Dockerfiles per service
4. **Push to registry** — Amazon ECR (Elastic Container Registry)
5. **Deploy** — update ECS task definitions and trigger rolling deployment
6. **Post-deploy smoke test** — hit health-check endpoints and report status

**Secrets to configure in GitHub repository settings:**
- `AWS_ACCESS_KEY_ID` / `AWS_SECRET_ACCESS_KEY`
- `ECR_REGISTRY` (account-specific ECR URL)
- `DB_CONNECTION_STRING` (staging / prod)
- `JWT_SECRET`

---

### 3.  AWS Deployment

**Target architecture on AWS:**

```
                         ┌─────────────────────┐
   Browser / App ──────► │   AWS API Gateway   │
                         └──────────┬──────────┘
                                    │
                         ┌──────────▼──────────┐
                         │  Application Load   │
                         │    Balancer (ALB)   │
                         └──────────┬──────────┘
                                    │
              ┌─────────────────────┼───────────────────────┐
              │                     │                        │
   ┌──────────▼──────┐  ┌──────────▼──────┐  ┌─────────────▼────┐
   │  ECS Fargate    │  │  ECS Fargate    │  │   ECS Fargate    │
   │  auth-service   │  │ ticket-service  │  │   ...services    │
   └─────────────────┘  └─────────────────┘  └──────────────────┘
              │                     │
   ┌──────────▼─────────────────────▼──────┐
   │          Amazon RDS (PostgreSQL)       │
   │      (one DB per service schema)       │
   └────────────────────────────────────────┘
```

**AWS services to provision:**

| Service | Purpose |
|---|---|
| **ECS Fargate** | Run containerised microservices (serverless containers) |
| **ECR** | Private Docker image registry |
| **RDS (PostgreSQL)** | Managed relational database |
| **ALB** | Layer-7 load balancing & HTTPS termination |
| **Route 53** | DNS management |
| **ACM** | TLS/SSL certificates |
| **SQS / SNS** | Async messaging between services |
| **S3** | Static asset hosting (Angular build artifacts) |
| **CloudFront** | CDN for the frontend |
| **Secrets Manager** | Store JWT secrets, DB passwords, API keys |
| **CloudWatch** | Centralised logs, metrics & alerts |

**Infrastructure-as-Code:** provision with **AWS CDK (TypeScript)** or **Terraform** (to be decided).

---

### 4. Cross-Platform Client

Make the Angular frontend compile to multiple target platforms from a single codebase.

| Target | Technology | Status |
|---|---|---|
| **Web** | Angular (current) | In progress |
| **Desktop (Windows / macOS / Linux)** | [**Electron**](https://www.electronjs.org/) wrapping the Angular app | ⬜ Planned |
| **Mobile (iOS & Android)** | [**Capacitor**](https://capacitorjs.com/) (Ionic team) bridging Angular to native | ⬜ Planned |

**Why Capacitor + Electron?**
- Capacitor is the recommended bridge for Angular → native mobile; it requires minimal code changes and gives access to native device APIs (camera, push notifications, biometric auth).
- Electron packages the same Angular build into a desktop shell, enabling offline capability and OS-level integrations.
- Both share the **same Angular source** — no duplicate UI code.

**Planned additions for cross-platform support:**
- `@capacitor/core` + platform-specific plugins (`@capacitor/push-notifications`, `@capacitor/biometrics`)
- Electron main process (`electron/main.ts`) with auto-update via `electron-updater`
- Platform detection service in Angular to conditionally show/hide native features
- Separate CI build jobs in GitHub Actions for each target (web, Android APK, Windows `.exe`)