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

    if (required === 'ticket.read' && user.isTeamLeader) return true;

    return user.permissions.includes(required)
        ? true
        : router.createUrlTree(['/']);
};
