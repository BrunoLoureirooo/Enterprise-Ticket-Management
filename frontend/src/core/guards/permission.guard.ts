import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

export const permissionGuard: CanActivateFn = (route) => {
    const auth = inject(AuthService);
    const router = inject(Router);
    const user = auth.getCurrentUser();

    if (!user) return router.createUrlTree(['/login']);
    if (user.role === 'Admin') return true;

    const required = route.data['permission'] as string | undefined;
    if (!required) return true;

    // Tickets page is restricted to team leaders; regular employees use /my-tickets
    if (required === 'ticket.read') {
        return user.isTeamLeader ? true : router.createUrlTree(['/']);
    }

    return user.permissions.includes(required)
        ? true
        : router.createUrlTree(['/']);
};
