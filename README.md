# Enterprise Ticket Management System (Work in Progress)

Enterprise Ticket Management — a work-in-progress internal web application for managing operational tickets. The project has a functional authentication system with JWT + claims-based authorization, a YARP API gateway with permission enforcement, user and role management APIs, Redis-based token revocation, and a fully navigable Angular frontend with role-aware sidebar. The full stack is deployed to Azure Container Apps via a GitHub Actions CD pipeline with Terraform-managed infrastructure.

## Live Demo

| | URL |
|---|---|
| Frontend | https://ca-frontend.gentlerock-e6844739.westeurope.azurecontainerapps.io |
| Swagger UI | https://ca-gateway.gentlerock-e6844739.westeurope.azurecontainerapps.io/swagger |

**Demo credentials:** `admin@admin.pt` / `a`

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
  - User roles management UI [Planned]
  - User permissions management UI [Planned]

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
- **Framework**: ASP.NET Core (.NET 8)
- **ORM**: Entity Framework Core 8 (code-first migrations, seeded roles + permission claims)
- **Authentication**: JWT Bearer + ASP.NET Core Identity
- **Authorization**: Claims-based; gateway enforces per-route; Admin bypasses all
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
- **Database**: PostgreSQL 17 (Alpine)
- **Cache**: Redis 7 (Alpine)
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
```

### Start

```bash
docker-compose up --build
```

### Local Service Ports

| Service | Port |
|---|---|
| Angular Frontend (HTTPS) | 4200 |
| Angular Frontend (HTTP) | 4201 |
| API Gateway (HTTPS) | 5001 |
| PostgreSQL | 5435 |
| pgAdmin | 5050 |

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
└── ca-identity  (ASP.NET Core)      ← internal only
        │                    │
        ▼                    ▼
  Neon PostgreSQL       Upstash Redis
  (Azure GWC)           (AWS Frankfurt)
```

### CD Pipeline

Every push to `main` triggers `.github/workflows/cd.yml`:
1. Build Docker images for `identity`, `gateway`, `frontend`
2. Push to `ghcr.io/brunoloureirooo/ticket-mgmt-{service}:latest`
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
│   └── identity-api/            # Authentication & User Management Service
│       ├── API/
│       │   ├── ActionFilters/   # ValidationFilterAttribute
│       │   ├── Controllers/     # AuthController, UserController, RoleController
│       │   └── Program.cs       # App bootstrap, DI, middleware
│       ├── Application/
│       │   ├── Services/        # AuthService, UserService, RoleService, TokenRevocationService
│       │   └── MappingProfile.cs
│       ├── Entities/
│       │   ├── Models/          # ApplicationUser, ApplicationRole
│       │   ├── DataTransferObjects/
│       │   └── Constants/       # Permission string constants
│       └── Repository/
│           ├── Configuration/   # EF Core seed configs (Roles, Users, Claims)
│           └── Migrations/
│
├── frontend/
│   └── src/
│       ├── app/                 # Root component, routes, config
│       ├── core/
│       │   ├── services/        # AuthService, NavService, ToastService
│       │   └── guards/          # authGuard
│       ├── Models/              # Auth + User DTOs
│       ├── features/            # Lazy-loaded pages
│       │   ├── auth/login/      # [Implemented]
│       │   ├── auth/register/   # [Implemented]
│       │   ├── home/            # [Placeholder]
│       │   ├── tickets/         # [Placeholder]
│       │   ├── users/           # [Placeholder]
│       │   └── settings/        # [Placeholder]
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

### Next: Ticket Service

New microservice owning the ticket domain.

| Service | Responsibility | Status |
|---|---|---|
| `identity-service` | Authentication, JWT, user/role/permission management | **Done** |
| `api-gateway` | Single entry point, JWT validation, rate limiting | **Done** |
| `ticket-service` | Ticket lifecycle, teams, assignments, workflows | **Planned** |

**Planned features:**
- Ticket CRUD — categories (Hardware, Software, Network), priority levels (Low → Critical)
- Status workflow: Open → In Progress → On Hold → Resolved → Closed
- Team management — create teams, assign users, designate TeamLeaders
- Assignment engine — TeamLeaders assign tickets to technicians
- Internal notes per ticket
- Audit trail — full activity log with timestamps

### Frontend Feature Pages

| Page | Status |
|---|---|
| Login / Register | **Done** |
| Dashboard / Home | Placeholder |
| Tickets | Placeholder |
| My Tickets | Placeholder |
| Users Management | Placeholder |
| Settings | Placeholder |
| Role & Permission Management UI | Planned |

### Further Ahead

- **Advanced Authorization** — resource-based access control (TeamLeader scoped to own team)
- **Async Messaging** — RabbitMQ / Azure Service Bus for inter-service events
- **State Management** — NgRx store integration
- **Cross-Platform** — Electron (desktop), Capacitor (iOS/Android)
