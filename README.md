# Enterprise Ticket Management System

Enterprise Ticket Management — an internal web application for managing operational tickets across teams. The project has a JWT + claims-based authentication system, a YARP API gateway with permission enforcement, user and role management, team and project management, a full ticket lifecycle system with role-scoped visibility, event-driven cross-service sync via RabbitMQ, Redis-based token revocation, and a role-aware Angular frontend. The full stack is deployed to Azure Container Apps via a GitHub Actions CD pipeline with Terraform-managed infrastructure.

## Live Demo

| | URL |
|---|---|
| Frontend | https://ca-frontend.gentlerock-e6844739.westeurope.azurecontainerapps.io |
| Swagger UI | https://ca-gateway.gentlerock-e6844739.westeurope.azurecontainerapps.io/swagger |

**Demo credentials:** `admin@admin.com` / `a`

> First load may take a few seconds — containers scale to zero when idle (free tier).

---

## Features

- **User Authentication**:
  - Secure login system with role-based access control (Admin, TeamLeader, Technician) [Implemented]
  - User registration with validation [Implemented]
  - User management API [Implemented]
  - Redis-based token revocation — logout invalidates token server-side [Implemented]

- **Authorization**:
  - YARP gateway middleware validates JWTs and enforces `permissions` claims per route [Implemented]
  - Admin role bypasses all permission checks [Implemented]
  - JWT with claims — roles carry granular permission claims seeded via EF Core [Implemented]
  - Frontend sidebar tabs conditionally shown based on role/permissions [Implemented]
  - Permission hash caching via Redis — detects stale permissions mid-session [Implemented]
  - Role management API — CRUD operations for roles and permission assignments [Implemented]
  - User & role management UI [Implemented]

- **Team Management**: [Implemented]
  - Create, view, update, and delete teams
  - Add/remove team members; designate team leaders
  - Create, view, update, and delete projects; associate projects with teams
  - Event-driven sync: membership and team changes propagate to other services via RabbitMQ

- **Ticket Management**: [Implemented]
  - Create, view, update, and delete tickets
  - Set priority levels (Low, Medium, High, Critical)
  - Track ticket status (Open, InProgress, Resolved, Closed)
  - Cascading selectors: Team → filtered Projects → filtered assignable members
  - Role-scoped visibility:
    - **Admin** — full ticket list
    - **Team Leader** — all tickets in their team(s) + tickets assigned to them directly
    - **Regular Employee** — personal assigned tickets only (`/my-tickets`)

- **Assignment & Workflow**: [Implemented]
  - Tickets are assigned to a team member on creation
  - Team leaders assign/reassign tickets within their team
  - Status and assignee updatable via edit popup or dedicated API endpoints

- **Audit Trail**: [Planned]
  - Comprehensive logging of all ticket activities
  - Tracks who made changes and when

---

## Tech Stack

### Frontend
- **Framework**: Angular 21.1.0 (standalone components)
- **Language**: TypeScript 5.9.2
- **State Management**: NgRx (Planned)
- **Routing**: Angular Router (with auth guard, lazy-loaded feature modules)
- **UI Components**: DevExtreme 25.2.4 + Tailwind CSS 4.1.12
- **Icons**: Bootstrap Icons 1.13.1
- **HTTP Client**: Angular HttpClient
- **Testing**: Vitest 4.0.8
- **Auth**: JWT decoded client-side; permissions extracted and used to filter sidebar navigation

### Backend
- **Framework**: ASP.NET Core (.NET 8) — 4 microservices: gateway, identity, ticket, teams
- **ORM**: Entity Framework Core 8 (code-first migrations, seeded roles + permission claims)
- **Authentication**: JWT Bearer + ASP.NET Core Identity
- **Authorization**: Claims-based; gateway enforces per-route; Admin bypasses all
- **Messaging**: RabbitMQ 3 — fanout exchanges, durable queues, manual ACK, `BackgroundService` consumers; 4 event types (user sync, team membership, team sync, project sync)
- **Token Revocation**: Redis (StackExchange.Redis) — revoked tokens stored by JTI until expiry
- **Permission Caching**: Redis — permission hash stored per user, invalidated on role change
- **API Gateway**: YARP (Yet Another Reverse Proxy) with custom `GatewayAuthorizationMiddleware`
- **Rate Limiting**: Built-in ASP.NET Core RateLimiter (100 req/min per IP)
- **Error Tracking**: Sentry SDK
- **Mapping**: AutoMapper
- **Documentation**: Swagger / OpenAPI (aggregated at gateway)
- **Logging**: Serilog (structured, console)

### Infrastructure (Local)
- **Orchestration**: Docker Compose
- **Database**: PostgreSQL 17 (Alpine) — three separate databases (`identitydb`, `teamsdb`, `ticketdb`)
- **Cache**: Redis 7 (Alpine)
- **Message Broker**: RabbitMQ 3 (management UI on port 15672)
- **SSL**: Self-signed Kestrel certificates for local HTTPS

### Infrastructure (Production)
- **Hosting**: Azure Container Apps (free tier — 180k vCPU-sec/month)
- **Container Registry**: GitHub Container Registry (`ghcr.io`) — free
- **Database**: Neon serverless PostgreSQL (free tier, Azure Germany West Central)
- **Cache**: Upstash serverless Redis (free tier)
- **IaC**: Terraform — provisions Resource Group, Log Analytics, Container Apps Environment, 3 Container Apps
- **CI/CD**: GitHub Actions — builds Docker images, pushes to ghcr.io, deploys to Azure on push to `main`
- **HTTPS/TLS**: Automatic via Azure Container Apps

> **Why not AKS?** Azure Kubernetes Service requires paying for node VMs (~$30–50/month) plus a load balancer (~$18/month). Azure Container Apps provides the same container orchestration managed by Microsoft with a permanent free tier that comfortably covers a low-traffic application.

---

## Running Locally

### Prerequisites

- Node.js (v18 or higher)
- .NET 8 SDK
- Docker + Docker Compose

### Environment File

Create a `.env` file at the project root:

```
SECRET=<jwt-signing-key>
DB_PASSWORD=<postgres-password>
SENTRY_DSN_GATEWAY=<sentry-dsn>
SENTRY_DSN_IDENTITY=<sentry-dsn>
SENTRY_DSN_TICKET=<sentry-dsn>
SENTRY_DSN_TEAMS=<sentry-dsn>
```

### Start

```bash
docker-compose up --build
```

### Local Service Ports

| Service | Port |
|---|---|
| Angular Frontend | 4200 |
| API Gateway (HTTPS) | 5001 |
| PostgreSQL | 5435 |
| pgAdmin | 5050 |
| RabbitMQ Management | 15672 |

### API Documentation

Swagger UI: `https://localhost:5001/swagger`

---

## Deployment

### Architecture

```
Browser
   │
   ▼
Azure Container Apps (free tier, West Europe)
├── ca-frontend  (Angular / Nginx)   ← public HTTPS ingress
├── ca-gateway   (YARP / .NET 8)     ← public HTTPS ingress
├── ca-identity  (ASP.NET Core)      ← internal only
├── ca-ticket    (ASP.NET Core)      ← internal only
└── ca-teams     (ASP.NET Core)      ← internal only
        │                    │
        ▼                    ▼
  Neon PostgreSQL       Upstash Redis
  (Azure GWC)           (AWS Frankfurt)
```

### CD Pipeline

Every push to `main` triggers `.github/workflows/cd.yml`:
1. Build Docker images for all services (`identity`, `gateway`, `ticket`, `teams`, `frontend`)
2. Push to `ghcr.io`
3. Deploy each image to its Azure Container App

### Infrastructure (Terraform)

Terraform manages Azure resources. Run locally — tfvars are gitignored.

```bash
cd terraform
terraform init
terraform apply
```

**Required GitHub Secrets** (for the CD pipeline):
- `AZURE_CREDENTIALS` — Service Principal JSON

---

## Project Structure

```
Enterprise-Ticket-Management/
├── .github/
│   └── workflows/
│       ├── ci.yml               # Build check on PRs
│       └── cd.yml               # Build, push, deploy on push to main
│
├── backend/
│   ├── gateway-api/             # YARP API Gateway (.NET 8)
│   │   ├── API/
│   │   │   ├── Middleware/      # GatewayAuthorizationMiddleware
│   │   │   ├── appsettings.json # YARP routing config, Redis, JWT settings
│   │   │   └── Program.cs       # Gateway bootstrap, rate limiting, Sentry
│   │   └── Dockerfile
│   │
│   ├── identity-api/            # Authentication & User Management Service
│   │   ├── API/
│   │   │   ├── ActionFilters/   # ValidationFilterAttribute
│   │   │   ├── Controllers/     # AuthController, UserController, RoleController
│   │   │   └── Program.cs       # App bootstrap, DI, middleware
│   │   ├── Application/
│   │   │   ├── Services/        # AuthService, UserService, RoleService, TokenRevocationService
│   │   │   ├── Messaging/       # RabbitMqPublisher, TeamMembershipConsumer
│   │   │   └── MappingProfile.cs
│   │   ├── Entities/
│   │   │   ├── Models/          # ApplicationUser, ApplicationRole
│   │   │   └── Constants/       # Permission string constants
│   │   └── Repository/
│   │       └── Configuration/   # EF Core seed configs (Roles, Users, Claims)
│   │
│   ├── ticket-api/              # Ticket Lifecycle Service
│   │   ├── API/Controllers/     # TicketController
│   │   ├── Application/
│   │   │   ├── Services/        # TicketService (role-scoped GetScopedAsync)
│   │   │   └── Messaging/       # UserSyncConsumer, TeamMembershipConsumer,
│   │   │                        # TeamSyncConsumer, ProjectSyncConsumer
│   │   ├── Entities/Models/     # Ticket, TeamMembership, SyncedUser/Team/Project
│   │   └── Repository/
│   │
│   └── teams-api/               # Teams & Projects Service
│       ├── API/Controllers/     # TeamController, ProjectController
│       ├── Application/
│       │   ├── Services/        # TeamService, ProjectService
│       │   └── Messaging/       # RabbitMqPublisher, UserSyncConsumer
│       ├── Entities/Models/     # Team, TeamMember, Project, ProjectTeam, SyncedUser
│       └── Repository/
│
├── frontend/
│   └── src/
│       ├── app/                 # Root component, routes, config
│       ├── core/
│       │   ├── services/        # AuthService, NavService, ToastService
│       │   └── guards/          # authGuard, permissionGuard
│       ├── Models/              # Auth + User DTOs
│       ├── features/            # Lazy-loaded pages
│       │   ├── auth/login/      # [Implemented]
│       │   ├── auth/register/   # [Implemented]
│       │   ├── home/            # [Placeholder]
│       │   ├── tickets/         # Full ticket grid — admin + team leaders [Implemented]
│       │   │   └── my-tickets/  # Personal assigned tickets — all users [Implemented]
│       │   ├── teams/           # Teams & Projects management (tabbed) [Implemented]
│       │   ├── users/           # User management [Implemented]
│       │   └── roles/           # Role & permission management [Implemented]
│       └── shared/components/Layout/  # Sidebar (role/permission filtered)
│
├── terraform/                   # Azure infrastructure (IaC)
│   ├── main.tf                  # Resource Group, Container Apps Environment, 3 Container Apps
│   ├── variables.tf
│   └── outputs.tf
│
├── https/                       # Dev SSL certificate for Kestrel
├── docker-compose.yml           # Full local stack
└── Documentation/               # Project documentation
```

---

## Roadmap

### Services

| Service | Responsibility | Status |
|---|---|---|
| `identity-api` | Authentication, JWT, user/role/permission management | **Done** |
| `gateway-api` | Single entry point, JWT validation, rate limiting | **Done** |
| `teams-api` | Teams, projects, membership management | **Done** |
| `ticket-api` | Ticket lifecycle, role-scoped queries, assignment | **Done** |

### Frontend Feature Pages

| Page | Status |
|---|---|
| Login / Register | **Done** |
| Dashboard / Home | Placeholder |
| Tickets (admin + team leaders) | **Done** |
| My Tickets (all users) | **Done** |
| Teams & Projects | **Done** |
| Users Management | **Done** |
| Role & Permission Management | **Done** |

### Further Ahead

- **Audit Trail** — full activity log per ticket with timestamps and actor
- **Internal Notes** — per-ticket comment thread for technicians
- **State Management** — NgRx store integration
- **Cross-Platform** — Electron (desktop), Capacitor (iOS/Android)
