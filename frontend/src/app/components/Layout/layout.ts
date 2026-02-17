import { Component, inject, computed } from '@angular/core';
import { NavService } from '../../services/nav.service';
import { DxToolbarModule, DxDrawerModule, DxListModule } from 'devextreme-angular';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-layout',
  standalone: true,
  imports: [DxToolbarModule, DxDrawerModule, DxListModule, CommonModule, RouterLink],
  templateUrl: './layout.html',
  styleUrls: ['./layout.css']
})
export class Layout {
  navService = inject(NavService);
  router = inject(Router);

  menuItems = [
    { id: 1, text: 'Dashboard', icon: 'bi bi-house-door', path: '' },
    { id: 2, text: 'Tickets', icon: 'bi bi-ticket-perforated', path: '/tickets' },
    { id: 3, text: 'My Tickets', icon: 'bi bi-clipboard-check', path: '/my-tickets' },
    { id: 4, text: 'Users', icon: 'bi bi-people', path: '/users' },
    { id: 5, text: 'Settings', icon: 'bi bi-gear', path: '/settings' },
  ];

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

  onItemClick(e: any) {
    if (e.itemData.path) {
      this.router.navigate([e.itemData.path]);
    }
  }
}
