import { HttpClient } from '@angular/common/http';
import { Component, inject, OnInit, signal } from '@angular/core';
import { lastValueFrom } from 'rxjs';
import { CommonModule } from '@angular/common';
import { DxDataGridModule, DxToolbarModule, DxTabPanelModule } from 'devextreme-angular';
import { ToastService } from '../../core/services/toast.service';
import { AuthService } from '../../core/services/auth.service';

interface Role {
  id: string;
  name: string;
}

interface RoleWithPermissions extends Role {
  permissions: string[];
}

interface PermissionGroup {
  group: string;
  permissions: string[];
}

interface PermissionRow {
  action: string;
  perm: string;
  enabled: boolean;
}

@Component({
  selector: 'app-roles',
  standalone: true,
  imports: [CommonModule, DxDataGridModule, DxToolbarModule, DxTabPanelModule],
  templateUrl: './roles.html',
})
export class Roles implements OnInit {
  private http = inject(HttpClient);
  private toastService = inject(ToastService);
  private authService = inject(AuthService);

  private get perms() { return this.authService.getCurrentUser(); }
  private has = (p: string) => this.perms?.role === 'Admin' || (this.perms?.permissions ?? []).includes(p);

  protected canUpdate = this.has('role.update');

  protected roles = signal<RoleWithPermissions[]>([]);
  protected permissionGroups = signal<PermissionGroup[]>([]);
  protected loading = signal(true);

  protected editingRole = signal<RoleWithPermissions | null>(null);
  protected editingPermissions = signal<string[]>([]);

  async ngOnInit() {
    await Promise.all([this.loadRoles(), this.loadPermissionGroups()]);
  }

  async loadPermissionGroups() {
    try {
      const perms = await lastValueFrom(this.http.get<string[]>('/api/permission'));
      const map = new Map<string, string[]>();
      for (const p of perms) {
        const resource = p.split('.')[0];
        const group = resource.charAt(0).toUpperCase() + resource.slice(1);
        if (!map.has(group)) map.set(group, []);
        map.get(group)!.push(p);
      }
      this.permissionGroups.set([...map.entries()].map(([group, permissions]) => ({ group, permissions })));
    } catch (error: any) {
      this.toastService.error(error.message ?? 'Failed to load permissions');
    }
  }

  async loadRoles() {
    try {
      this.loading.set(true);
      const list = await lastValueFrom(this.http.get<Role[]>('/api/Role'));
      const filtered = list.filter(r => r.name !== 'Admin');
      const detailed = await Promise.all(
        filtered.map(r => lastValueFrom(this.http.get<RoleWithPermissions>(`/api/Role/${r.id}`)))
      );
      this.roles.set(detailed);
    } catch (error: any) {
      this.toastService.error(error.message ?? 'Failed to load roles');
    } finally {
      this.loading.set(false);
    }
  }

  onEditingStart(e: any) {
    this.editingRole.set(e.data);
    this.editingPermissions.set([...(e.data.permissions ?? [])]);
  }

  getTabGridData(permissions: string[]): PermissionRow[] {
    const current = this.editingPermissions();
    return permissions.map(perm => ({
      action: perm.split('.')[1].charAt(0).toUpperCase() + perm.split('.')[1].slice(1),
      perm,
      enabled: current.includes(perm),
    }));
  }

  onPermRowUpdating(e: any) {
    e.cancel = true;
    const row: PermissionRow = { ...e.oldData, ...e.newData };
    const current = [...this.editingPermissions()];
    if (row.enabled && !current.includes(row.perm)) {
      current.push(row.perm);
    } else if (!row.enabled) {
      const idx = current.indexOf(row.perm);
      if (idx >= 0) current.splice(idx, 1);
    }
    this.editingPermissions.set(current);
  }

  async onSaving(e: any) {
    e.cancel = true;
    const role = this.editingRole();
    if (!role) return;
    try {
      await lastValueFrom(this.http.put(`/api/Role/${role.id}/permissions`, {
        permissions: this.editingPermissions(),
      }));
      this.toastService.success('Permissions saved');
      e.component.cancelEditData();
      await this.loadRoles();
    } catch (error: any) {
      this.toastService.error(error.message ?? 'Failed to save permissions');
    }
  }
}
