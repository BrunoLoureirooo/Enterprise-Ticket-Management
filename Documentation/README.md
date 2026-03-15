# Enterprise Ticket Management System (Work in Progress)

Enterprise Ticket Management — a work-in-progress internal web application for managing operational tickets. The project has a functional authentication system with JWT + claims-based authorization, a YARP API gateway with permission enforcement, user and role management APIs, Redis-based token revocation, and a fully navigable Angular frontend with role-aware sidebar.

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

## Prerequisites

- Node.js (v18 or higher)
- .NET 8 SDK
- Docker + Docker Compose
- **Environment File**: A `.env` file must be created at the root directory containing:
  - `SECRET` — JWT signing key
  - `DB_PASSWORD` — PostgreSQL password
  - `SENTRY_DSN_GATEWAY` — Sentry DSN for gateway service
  - `SENTRY_DSN_IDENTITY` — Sentry DSN for identity service

## Usage

### Running Locally

```bash
docker-compose up --build
```

### Default Credentials

- **Admin**: `admin@admin.pt` / `a`

### API Documentation

Interactive Swagger UI available at: `https://localhost:5001/swagger`

### Local Service Ports

| Service | Port |
|---|---|
| Angular Frontend (HTTPS) | 4200 |
| Angular Frontend (HTTP) | 4201 |
| API Gateway (HTTPS) | 5001 |
| PostgreSQL | 5435 |
| pgAdmin | 5050 |

## Project Structure

```
Enterprise-Ticket-Management/
├── backend/
│   ├── gateway-api/                  # YARP API Gateway (.NET 8)
│   │   ├── API/
│   │   │   ├── Middleware/           # GatewayAuthorizationMiddleware
│   │   │   ├── appsettings.json      # YARP routing config, Redis, JWT settings
│   │   │   └── Program.cs            # Gateway bootstrap, rate limiting, Sentry
│   │   └── Dockerfile
│   │
│   └── identity-api/                 # Authentication & User Management Service
│       ├── API/
│       │   ├── ActionFilters/        # ValidationFilterAttribute
│       │   ├── Controllers/          # AuthController, UserController, RoleController
│       │   └── Program.cs            # App bootstrap, DI, middleware
│       │
│       ├── Application/
│       │   ├── Services/             # AuthService, UserService, RoleService, TokenRevocationService
│       │   │   └── Contracts/        # Service interfaces
│       │   └── MappingProfile.cs     # AutoMapper profiles
│       │
│       ├── Entities/
│       │   ├── Models/               # ApplicationUser, ApplicationRole
│       │   ├── DataTransferObjects/  # TokenDto, User DTOs, LoginResponse
│       │   ├── Constants/            # Permission string constants
│       │   └── JWTConfiguration.cs
│       │
│       └── Repository/
│           ├── Configuration/        # EF Core seed configs (Roles, Users, UserRoles, Claims)
│           ├── Migrations/           # EF Core migrations
│           ├── RepositoryContext.cs
│           └── RepositoryManager.cs
│
├── frontend/
│   └── src/
│       ├── app/
│       │   ├── app.ts                # Root component
│       │   ├── app.routes.ts         # Routing with authGuard
│       │   └── app.config.ts         # App configuration + providers
│       │
│       ├── core/
│       │   ├── services/             # AuthService (JWT decode + permissions), NavService, ToastService
│       │   └── guards/               # authGuard (route protection)
│       │
│       ├── Models/
│       │   ├── Auth/                 # LoginResponse, TokenDto
│       │   └── Users/                # UserInfo (name, role, permissions[])
│       │
│       ├── features/                 # Lazy-loaded feature modules
│       │   ├── auth/login/           # Login page [Implemented]
│       │   ├── auth/register/        # Register page [Implemented]
│       │   ├── home/                 # Dashboard [Placeholder]
│       │   ├── tickets/              # Tickets page + My Tickets sub-page [Placeholder]
│       │   ├── users/                # Users management page [Placeholder]
│       │   └── settings/             # Settings page [Placeholder]
│       │
│       └── shared/
│           └── components/Layout/    # App shell — sidebar filters tabs by role/permission
│
├── https/                            # Dev SSL certificate for Kestrel (Docker)
├── docker-compose.yml                # Full local stack: PostgreSQL, Redis, Gateway, Identity, Frontend, pgAdmin
└── Documentation/                    # Project documentation
```

---

## Roadmap / To Implement

> This section tracks the major features and operational improvements planned for the project.

### 1. Ticket Service (Next Priority)

New microservice owning the ticket domain entirely.

| Service | Responsibility | Status |
|---|---|---|
| `identity-service` | Authentication, JWT, user/role/permission management | **Implemented** |
| `ticket-service` | Ticket lifecycle, teams, assignments, workflows | **Planned** |
| `api-gateway` | Single entry point, JWT validation, rate limiting | **Implemented** |

**Planned ticket-service features:**
- Ticket CRUD (create, read, update, delete)
- Categories: Hardware, Software, Network
- Priority levels: Low, Medium, High, Critical
- Status workflow: Open → In Progress → On Hold → Resolved → Closed
- Team management (create teams, assign users, designate TeamLeaders)
- Assignment engine (TeamLeaders assign tickets to technicians)
- Internal notes per ticket
- Audit trail (full activity log with user attribution and timestamps)

---

### 2. Frontend Feature Pages

All feature pages are currently placeholder components. Will be implemented in parallel with the ticket service.

| Page | Status |
|---|---|
| Login / Register | **Implemented** |
| Dashboard / Home | Placeholder |
| Tickets | Placeholder |
| My Tickets | Placeholder |
| Users Management | Placeholder |
| Settings | Placeholder |
| Role & Permission Management UI | Planned |

---

### 3. CI/CD — GitHub Actions

Automate building and deploying on every push to `main`.

**Planned workflows:**

```
.github/workflows/
├── ci.yml   # Build & test on every PR (dotnet build, ng build, dotnet test)
└── cd.yml   # Build images, push to registry, deploy to Azure on merge to main
```

**Pipeline stages:**
1. Build Docker images for gateway, identity, frontend
2. Push to **GitHub Container Registry** (`ghcr.io`) — free
3. Deploy updated images to **Azure Container Apps**
4. Post-deploy health check

**GitHub Secrets required:**
- `AZURE_CREDENTIALS` — Service Principal for GitHub Actions → Azure
- `JWT_SECRET`
- `DB_CONNECTION_STRING` — Neon PostgreSQL connection string
- `REDIS_CONNECTION_STRING` — Upstash Redis connection string
- `SENTRY_DSN_GATEWAY`
- `SENTRY_DSN_IDENTITY`

---

### 4. Cloud Deployment — Azure Container Apps (Free Tier)

Running on Azure with zero monthly cost using free-tier services throughout.

**Architecture:**

```
Browser
   │
   ▼
Azure Container Apps (free tier)
├── frontend  (Angular / Nginx)   ← public ingress, automatic HTTPS
├── gateway   (YARP / .NET 8)     ← internal ingress
└── identity  (ASP.NET Core)      ← internal only
   │                    │
   ▼                    ▼
Neon PostgreSQL     Upstash Redis
(free tier)         (free tier)
```

**Service stack — chosen for zero cost:**

| Need | Service | Cost |
|---|---|---|
| Container hosting | Azure Container Apps | Free tier (180k vCPU-sec/month) |
| Docker image registry | GitHub Container Registry (ghcr.io) | Free |
| PostgreSQL | Neon (serverless Postgres) | Free tier |
| Redis | Upstash (serverless Redis) | Free tier |
| CI/CD | GitHub Actions | Free |
| HTTPS / TLS | Built into Container Apps | Free |
| Infrastructure-as-Code | Terraform | Free (open source) |

> **Why not AKS?** Azure Kubernetes Service requires paying for node VMs (~$30-50/month minimum) plus a load balancer (~$18/month). Azure Container Apps provides the same container orchestration capabilities managed by Microsoft, with a permanent free tier that comfortably covers a low-traffic application.

**Infrastructure provisioned by Terraform:**
- Azure Resource Group
- Azure Container Apps Environment
- Container Apps for gateway, identity, frontend

**Migration path:** When traffic justifies it, Container Apps can be replaced with AKS without any changes to the application code or Docker images.

---

### 5. Advanced Authorization Features

- Resource-based access control (TeamLeader scoped to their own team's tickets only)
- Dynamic policy evaluation per request
- User permissions management UI (complement to existing role management API)

---

### 6. Async Messaging

- RabbitMQ or Azure Service Bus for event-driven inter-service communication
- Example: ticket-service publishes `TicketAssigned` event → identity-service sends notification

---

### 7. State Management

- NgRx store integration for frontend (currently using service-level state only)

---

### 8. Cross-Platform Client

| Target | Technology | Status |
|---|---|---|
| Web | Angular (current) | In progress |
| Desktop (Windows / macOS / Linux) | Electron wrapping the Angular app | Planned |
| Mobile (iOS & Android) | Capacitor bridging Angular to native | Planned |
