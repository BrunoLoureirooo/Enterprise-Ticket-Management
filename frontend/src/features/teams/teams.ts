import { HttpClient } from '@angular/common/http';
import { Component, inject, OnInit, signal } from '@angular/core';
import { lastValueFrom } from 'rxjs';
import { CommonModule } from '@angular/common';
import { DxDataGridModule, DxToolbarModule, DxSelectBoxModule, DxCheckBoxModule, DxButtonModule } from 'devextreme-angular';
import { ToastService } from '../../core/services/toast.service';
import { AuthService } from '../../core/services/auth.service';

@Component({
  selector: 'app-teams',
  standalone: true,
  imports: [CommonModule, DxDataGridModule, DxToolbarModule, DxSelectBoxModule, DxCheckBoxModule, DxButtonModule],
  templateUrl: './teams.html',
})
export class Teams implements OnInit {
  private http = inject(HttpClient);
  private toastService = inject(ToastService);
  private authService = inject(AuthService);

  private has = (p: string) => {
    const u = this.authService.getCurrentUser();
    return u?.role === 'Admin' || (u?.permissions ?? []).includes(p);
  };

  protected canWrite = this.has('teams.create');
  protected canDelete = this.has('teams.delete');
  protected canWriteProject = this.has('projects.create');
  protected canDeleteProject = this.has('projects.delete');

  protected teams = signal<any[]>([]);
  protected projects = signal<any[]>([]);
  protected users = signal<any[]>([]);
  protected loading = signal(true);
  protected activeTab = signal<'teams' | 'projects'>('teams');

  // Master-detail state keyed by teamId
  protected teamDetails = signal<Record<string, any>>({});
  protected addMemberState = signal<Record<string, { userId: string | null; isLeader: boolean }>>({});

  onRemoveMemberClick = (e: any) => {
    this.removeMember(e.row.data.teamId, e.row.data.userId);
  };

  async ngOnInit() {
    await Promise.all([this.loadTeams(), this.loadProjects(), this.loadUsers()]);
  }

  async loadTeams() {
    this.loading.set(true);
    try {
      const data = await lastValueFrom(this.http.get<any[]>('/api/Teams'));
      this.teams.set(data);
    } catch (e: any) {
      this.toastService.error(e.message ?? 'Failed to load teams');
    } finally {
      this.loading.set(false);
    }
  }

  async loadProjects() {
    try {
      const data = await lastValueFrom(this.http.get<any[]>('/api/Projects'));
      this.projects.set(data);
    } catch {}
  }

  async loadUsers() {
    try {
      const data = await lastValueFrom(this.http.get<any[]>('/api/User'));
      this.users.set(data);
    } catch {}
  }

  async onTeamsSaving(e: any) {
    e.cancel = true;
    const change = e.changes[0];
    if (!change) return;
    try {
      if (change.type === 'insert') {
        await lastValueFrom(this.http.post('/api/Teams', change.data));
      } else if (change.type === 'update') {
        await lastValueFrom(this.http.put(`/api/Teams/${change.key}`, change.data));
      } else if (change.type === 'remove') {
        await lastValueFrom(this.http.delete(`/api/Teams/${change.key}`));
      }
      e.component.cancelEditData();
      await this.loadTeams();
    } catch (err: any) {
      this.toastService.error(err.message ?? 'Operation failed');
    }
  }

  async onProjectsSaving(e: any) {
    e.cancel = true;
    const change = e.changes[0];
    if (!change) return;
    try {
      if (change.type === 'insert') {
        await lastValueFrom(this.http.post('/api/Projects', { ...change.data, teamIds: [] }));
      } else if (change.type === 'update') {
        await lastValueFrom(this.http.put(`/api/Projects/${change.key}`, change.data));
      } else if (change.type === 'remove') {
        await lastValueFrom(this.http.delete(`/api/Projects/${change.key}`));
      }
      e.component.cancelEditData();
      await this.loadProjects();
    } catch (err: any) {
      this.toastService.error(err.message ?? 'Operation failed');
    }
  }

  async onTeamRowExpanded(e: any) {
    const teamId = e.key;
    try {
      const team = await lastValueFrom(this.http.get<any>(`/api/Teams/${teamId}`));
      this.teamDetails.update(d => ({ ...d, [teamId]: team }));
      if (!this.addMemberState()[teamId]) {
        this.addMemberState.update(s => ({ ...s, [teamId]: { userId: null, isLeader: false } }));
      }
    } catch {
      this.toastService.error('Failed to load team members');
    }
  }

  getMembersForTeam(teamId: string): any[] {
    const detail = this.teamDetails()[teamId];
    if (!detail) return [];
    return (detail.members ?? []).map((m: any) => ({
      ...m,
      teamId,
      name: this.getUserName(m.userId),
    }));
  }

  getUserName(userId: string): string {
    return this.users().find(u => u.id === userId)?.name ?? userId;
  }

  getAddMemberState(teamId: string) {
    return this.addMemberState()[teamId] ?? { userId: null, isLeader: false };
  }

  setAddMemberUserId(teamId: string, userId: string | null) {
    this.addMemberState.update(s => ({ ...s, [teamId]: { ...this.getAddMemberState(teamId), userId } }));
  }

  setAddMemberIsLeader(teamId: string, isLeader: boolean) {
    this.addMemberState.update(s => ({ ...s, [teamId]: { ...this.getAddMemberState(teamId), isLeader } }));
  }

  async addMember(teamId: string) {
    const state = this.getAddMemberState(teamId);
    if (!state.userId) return;
    try {
      await lastValueFrom(this.http.post(`/api/Teams/${teamId}/members`, {
        userId: state.userId,
        isLeader: state.isLeader,
      }));
      const team = await lastValueFrom(this.http.get<any>(`/api/Teams/${teamId}`));
      this.teamDetails.update(d => ({ ...d, [teamId]: team }));
      this.addMemberState.update(s => ({ ...s, [teamId]: { userId: null, isLeader: false } }));
    } catch (err: any) {
      this.toastService.error(err.message ?? 'Failed to add member');
    }
  }

  async removeMember(teamId: string, userId: string) {
    try {
      await lastValueFrom(this.http.delete(`/api/Teams/${teamId}/members/${userId}`));
      const team = await lastValueFrom(this.http.get<any>(`/api/Teams/${teamId}`));
      this.teamDetails.update(d => ({ ...d, [teamId]: team }));
    } catch (err: any) {
      this.toastService.error(err.message ?? 'Failed to remove member');
    }
  }
}
