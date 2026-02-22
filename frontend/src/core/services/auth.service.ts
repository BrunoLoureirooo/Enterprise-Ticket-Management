import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { lastValueFrom } from 'rxjs';
import { LoginResponse } from '../../Models/Auth/LoginResponse';
import { UserInfo } from '../../Models/Users/UserInfo';
import { RegisterUser } from '../../Models/Users/RegisterUser';



@Injectable({
    providedIn: 'root'
})
export class AuthService {

    private readonly TOKEN_KEY = 'access_token';
    private readonly API = '/api/Auth';

    private http = inject(HttpClient);

    async logIn(username: string, password: string): Promise<{ ok: boolean; message?: string }> {
        try {
            const response = await lastValueFrom(
                this.http.post<LoginResponse>(`${this.API}/login`, { username, password })
            );
            this.setToken(response.accessToken);
            return { ok: true };
        } catch (err: any) {
            const message =
                err?.error?.message ??
                err?.error?.title ??
                (err?.status ? `Error ${err.status}: ${err.statusText}` : 'Login failed');
            return { ok: false, message };
        }
    }


    async register(registerUser: RegisterUser): Promise<{ ok: boolean; message?: string }> {
        try {
            const response = await lastValueFrom(
                this.http.post<LoginResponse>(`${this.API}/register`, registerUser)
            );
            this.setToken(response.accessToken);
            return { ok: true };
        } catch (err: any) {
            const message =
                err?.error?.message ??
                err?.error?.title ??
                (err?.status ? `Error ${err.status}: ${err.statusText}` : 'Registration failed');
            return { ok: false, message };
        }
    }


    isLoggedIn(): boolean {
        const token = localStorage.getItem(this.TOKEN_KEY);
        if (!token) return false;

        try {
            const payload = this.decodeToken(token);
            if (payload.exp && payload.exp * 1000 < Date.now()) {
                this.logout();
                return false;
            }
            return true;
        } catch {
            return false;
        }
    }

    getCurrentUser(): UserInfo | null {
        const token = localStorage.getItem(this.TOKEN_KEY);
        if (!token) return null;
        try {
            const payload = this.decodeToken(token);
            return {
                name: payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'] ?? '',
                email: payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress'] ?? '',
                role: payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] ?? '',
                avatarUrl: payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/uri'] ?? '',
            };
        } catch {
            return null;
        }
    }

    private decodeToken(token: string): any {
        // JWT uses base64url: replace chars and add padding before decoding
        const base64url = token.split('.')[1];
        const base64 = base64url.replace(/-/g, '+').replace(/_/g, '/');
        const padded = base64.padEnd(base64.length + (4 - base64.length % 4) % 4, '=');
        return JSON.parse(atob(padded));
    }

    getToken(): string | null {
        return localStorage.getItem(this.TOKEN_KEY);
    }

    setToken(token: string): void {
        localStorage.setItem(this.TOKEN_KEY, token);
    }

    logout(): void {
        localStorage.removeItem(this.TOKEN_KEY);
    }
}
