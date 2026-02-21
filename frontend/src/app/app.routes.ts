import { Routes } from '@angular/router';

export const routes: Routes = [
    {
        path: '',
        pathMatch: 'full',
        loadComponent: () => import('../features/home/home').then(m => m.Home)
    },
    {
        path: 'tickets',
        loadComponent: () => import('../features/tickets/tickets').then(m => m.Tickets)
    },
    {
        path: 'my-tickets',
        loadComponent: () => import('../features/tickets/my-tickets/my-tickets').then(m => m.MyTickets)
    },
    {
        path: 'users',
        loadComponent: () => import('../features/users/users').then(m => m.Users)
    },
    {
        path: 'settings',
        loadComponent: () => import('../features/settings/settings').then(m => m.Settings)
    },
    {
        path: 'auth',
        loadComponent: () => import('../features/auth/login/login').then(m => m.Login)
    }
];
