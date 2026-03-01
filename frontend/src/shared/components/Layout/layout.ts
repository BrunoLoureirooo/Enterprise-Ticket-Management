import { Component, inject } from '@angular/core';
import { NavService } from '../../../core/services/nav.service';
import { DxToolbarModule, DxDrawerModule, DxListModule, DxDropDownButtonModule } from 'devextreme-angular';
import { Router, RouterOutlet } from '@angular/router';
import { CommonModule } from '@angular/common';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';
import { UserInfo } from '../../../Models/Users/UserInfo';

@Component({
  selector: 'app-layout',
  standalone: true,
  imports: [DxToolbarModule, DxDrawerModule, DxListModule, DxDropDownButtonModule, CommonModule, RouterLink, RouterLinkActive, RouterOutlet],
  templateUrl: './layout.html',
  styleUrls: ['./layout.css']
})
export class Layout {
  navService = inject(NavService);
  router = inject(Router);
  authService = inject(AuthService);

  currentUser: UserInfo | null = this.authService.getCurrentUser();

  userMenuItems = [
    { id: 'logout', text: 'Logout', icon: 'runner' },
  ];

  onUserMenuItemClick(e: any) {
    if (e.itemData.id === 'logout') {
      this.authService.logout();
      this.router.navigate(['/login']);
    }
  }

  readonly menuItems: { id: number; text: string; icon: string; path: string; permission: string | null }[];

  constructor() {
    const isAdmin = this.currentUser?.role === 'Admin';
    this.menuItems = [
      { id: 1, text: 'Dashboard', icon: 'bi bi-house-door', path: '', permission: null },
      { id: 2, text: 'Tickets', icon: 'bi bi-ticket-perforated', path: '/tickets', permission: null },
      { id: 3, text: 'My Tickets', icon: 'bi bi-clipboard-check', path: '/my-tickets', permission: null },
      { id: 4, text: 'Users', icon: 'bi bi-people', path: '/users', permission: 'user:read' },
      { id: 5, text: 'Settings', icon: 'bi bi-gear', path: '/settings', permission: null },
    ].filter(item =>
      item.permission === null ||
      isAdmin ||
      (this.currentUser?.permissions ?? []).includes(item.permission!)
    );
  }

  get isOpen() {
    return this.navService.isOpen();
  }

  set isOpen(value: boolean) {
    this.navService.isOpen.set(value);
  }

  burgerOptions = {
    icon: 'menu',
    stylingMode: 'text',
    onClick: () => { this.isOpen = !this.isOpen; },
  }

  toggleSidebar() {
    this.navService.toggle();
  }

  get displayName(): string {
    const name = this.currentUser?.name ?? '';
    const parts = name.trim().split(/\s+/);
    if (parts.length <= 2) return name;
    return `${parts[0]} ${parts[parts.length - 1]}`;
  }



  onItemClick(e: any) {
    if (e.itemData.path) {
      this.router.navigate([e.itemData.path]);
    }
  }
}
