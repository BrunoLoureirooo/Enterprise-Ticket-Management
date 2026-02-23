# Claims-based & Dynamic Authorization System Design

This document outlines the architecture for a flexible authorization system that supports static claims-based permissions, role-level assignments, and dynamic resource-based access (e.g., Team Leadership).

## 1. Core Concepts

*   **Permissions**: Static string constants (e.g., `Users.View`, `Tickets.Delete`) representing specific actions.
*   **Static Claims**: Key-value pairs stored in `AspNetUserClaims` or `AspNetRoleClaims`.
*   **Dynamic Policies**: Requirements that involve runtime logic (e.g., "Is Bob the manager of *this* specific team?").
*   **Query Filtering**: Automatic injection of `.Where()` clauses in the data layer to restrict visibility based on user scope.

## 2. Backend Architecture (ASP.NET Core)

### Authorization Handlers
- **PermissionHandler**: Checks for a specific claim type `Permission`.
- **ResourceAuthorizationHandler**: Evaluates logic against a specific object (the "Resource").
  - *Example*: Checks if `team.LeaderId` matches the current `UserId`.

### Global Constants
Stored in `backend.Entities.Constants.Permissions.cs`:
```csharp
public static class Permissions {
    public const string UsersView = "Users.View";
    public const string TicketsManage = "Tickets.Manage";
    // ...
}
```

### Management API
- **ClaimsController**: Endpoints to GET/POST claims for both Roles and individual Users. This allows assigning "one-off" permissions like giving a regular user access to a specific admin list.

## 3. Frontend Integration (Angular)

### Permission Service
A service that provides:
- `hasPermission(permission: string): boolean`: Checks the user's claim list.
- `hasAccessToResource(resource: any, logicType: string): boolean`: Helper for frontend dynamic checks.

### Structural Directive
`*appHasPermission="'Users.View'"`: Conditionally renders components based on authorization.

## 4. Resource-Based Data Filtering
Instead of just blocking pages, we filter the data returned by the API:
```csharp
// Example Extension Method logic
public static IQueryable<Team> FilterByScope(this IQueryable<Team> query, ClaimsPrincipal user) {
    if (user.HasClaim("Permission", "Teams.ViewAll")) return query;
    var userId = user.GetUserId();
    return query.Where(t => t.LeaderId == userId);
}
```

## 5. Management UI
A dedicated feature in `src/features/admin/claims-management` allowing:
1.  **Role Configuration**: Toggle claims for a specific role.
2.  **User Overrides**: Add/Remove specific claims for a single user without changing their role.
