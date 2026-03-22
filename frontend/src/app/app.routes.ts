import { Routes } from '@angular/router';
import { authGuard } from '../core/guards/auth.guard';
import { permissionGuard } from '../core/guards/permission.guard';

export const routes: Routes = [
    {
        path: 'login',
        loadComponent: () => import('../features/auth/login/login').then(m => m.Login)
    },
    {
        path: 'register',
        loadComponent: () => import('../features/auth/register/register').then(m => m.Register)
    },
    {
        path: '',
        canActivate: [authGuard],
        loadComponent: () => import('../shared/components/Layout/layout').then(m => m.Layout),
        children: [
            {
                path: '',
                pathMatch: 'full',
                loadComponent: () => import('../features/home/home').then(m => m.Home)
            },
            {
                path: 'tickets',
                canActivate: [permissionGuard],
                data: { permission: 'ticket.read' },
                loadComponent: () => import('../features/tickets/tickets').then(m => m.Tickets)
            },
            {
                path: 'my-tickets',
                loadComponent: () => import('../features/tickets/my-tickets/my-tickets').then(m => m.MyTickets)
            },
            {
                path: 'users',
                canActivate: [permissionGuard],
                data: { permission: 'user.read' },
                loadComponent: () => import('../features/users/users').then(m => m.Users)
            },
            {
                path: 'roles',
                canActivate: [permissionGuard],
                data: { permission: 'role.read' },
                loadComponent: () => import('../features/roles/roles').then(m => m.Roles)
            },
            {
                path: 'teams',
                canActivate: [permissionGuard],
                data: { permission: 'teams.read' },
                loadComponent: () => import('../features/teams/teams').then(m => m.Teams)
            },
        ]
    },
    {
        path: '**',
        redirectTo: ''
    }
];
