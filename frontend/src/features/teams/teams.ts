import { HttpClient } from '@angular/common/http';
import { Component, inject, OnInit, signal } from '@angular/core';
import { lastValueFrom } from 'rxjs';
import { DxDataGridModule, DxToolbarModule, DxSelectBoxModule, DxCheckBoxModule, DxButtonModule } from 'devextreme-angular';
import { ToastService } from '../../core/services/toast.service';
import { AuthService } from '../../core/services/auth.service';

@Component({
  selector: 'app-teams',
  standalone: true,
  imports: [DxDataGridModule, DxToolbarModule, DxSelectBoxModule, DxCheckBoxModule, DxButtonModule],
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

  // Plain arrays for DX select boxes — signals don't reliably propagate into *dxTemplate embedded views
  protected usersArray: any[] = [];
  protected teamsArray: any[] = [];

  // Team edit popup state
  protected editingTeamId = signal<string | null>(null);
  protected editingTeamMembers = signal<any[]>([]);
  protected newMemberUserId: string | null = null;
  protected newMemberIsLeader = false;

  // Project edit popup state
  protected editingProjectId = signal<string | null>(null);
  protected editingProjectTeams = signal<any[]>([]);
  protected newProjectTeamId: string | null = null;

  async ngOnInit() {
    await Promise.all([this.loadTeams(), this.loadProjects(), this.loadUsers()]);
  }

  async loadTeams() {
    this.loading.set(true);
    try {
      const data = await lastValueFrom(this.http.get<any[]>('/api/Teams'));
      this.teams.set(data);
      this.teamsArray = data;
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
      this.usersArray = data;
    } catch (e: any) {
      this.toastService.error(e.message ?? 'Failed to load users');
    }
  }

  // ── Teams ──────────────────────────────────────────────────────────────────

  onTeamsInitNewRow() {
    this.editingTeamId.set(null);
    this.editingTeamMembers.set([]);
    this.newMemberUserId = null;
    this.newMemberIsLeader = false;
  }

  async onTeamsEditingStart(e: any) {
    this.editingTeamId.set(e.key);
    this.newMemberUserId = null;
    this.newMemberIsLeader = false;
    const [team] = await Promise.all([
      lastValueFrom(this.http.get<any>(`/api/Teams/${e.key}`)),
      this.loadUsers(),
    ]);
    this.editingTeamMembers.set(this.mapMembers(team.members ?? []));
  }

  async onTeamsSaving(e: any) {
    e.cancel = true;
    const change = e.changes[0];
    try {
      if (change?.type === 'insert') {
        await lastValueFrom(this.http.post('/api/Teams', change.data));
      } else if (change?.type === 'update') {
        await lastValueFrom(this.http.put(`/api/Teams/${change.key}`, change.data));
      } else if (change?.type === 'remove') {
        await lastValueFrom(this.http.delete(`/api/Teams/${change.key}`));
      }
      this.editingTeamId.set(null);
      this.editingTeamMembers.set([]);
      setTimeout(() => e.component.cancelEditData());
      await this.loadTeams();
    } catch (err: any) {
      this.toastService.error(err.message ?? 'Operation failed');
    }
  }

  async addMember() {
    const teamId = this.editingTeamId();
    if (!teamId || !this.newMemberUserId) return;
    try {
      await lastValueFrom(this.http.post(`/api/Teams/${teamId}/members`, {
        userId: this.newMemberUserId,
        isLeader: this.newMemberIsLeader,
      }));
      const team = await lastValueFrom(this.http.get<any>(`/api/Teams/${teamId}`));
      this.editingTeamMembers.set(this.mapMembers(team.members ?? []));
      this.newMemberUserId = null;
      this.newMemberIsLeader = false;
    } catch (err: any) {
      this.toastService.error(err.message ?? 'Failed to add member');
    }
  }

  async removeMember(userId: string) {
    const teamId = this.editingTeamId();
    if (!teamId) return;
    try {
      await lastValueFrom(this.http.delete(`/api/Teams/${teamId}/members/${userId}`));
      const team = await lastValueFrom(this.http.get<any>(`/api/Teams/${teamId}`));
      this.editingTeamMembers.set(this.mapMembers(team.members ?? []));
    } catch (err: any) {
      this.toastService.error(err.message ?? 'Failed to remove member');
    }
  }

  private mapMembers(members: any[]): any[] {
    return members;
  }

  // ── Projects ───────────────────────────────────────────────────────────────

  onProjectsInitNewRow() {
    this.editingProjectId.set(null);
    this.editingProjectTeams.set([]);
    this.newProjectTeamId = null;
  }

  async onProjectsEditingStart(e: any) {
    this.editingProjectId.set(e.key);
    this.newProjectTeamId = null;
    try {
      const project = await lastValueFrom(this.http.get<any>(`/api/Projects/${e.key}`));
      this.editingProjectTeams.set(this.mapProjectTeams(project.teamIds ?? []));
    } catch {}
  }

  async onProjectsSaving(e: any) {
    e.cancel = true;
    const change = e.changes[0];
    try {
      if (change?.type === 'insert') {
        await lastValueFrom(this.http.post('/api/Projects', { ...change.data, teamIds: [] }));
      } else if (change?.type === 'update') {
        await lastValueFrom(this.http.put(`/api/Projects/${change.key}`, change.data));
      } else if (change?.type === 'remove') {
        await lastValueFrom(this.http.delete(`/api/Projects/${change.key}`));
      }
      this.editingProjectId.set(null);
      this.editingProjectTeams.set([]);
      setTimeout(() => e.component.cancelEditData());
      await this.loadProjects();
    } catch (err: any) {
      this.toastService.error(err.message ?? 'Operation failed');
    }
  }

  async addProjectTeam() {
    const projectId = this.editingProjectId();
    if (!projectId || !this.newProjectTeamId) return;
    try {
      await lastValueFrom(this.http.post(`/api/Projects/${projectId}/teams/${this.newProjectTeamId}`, {}));
      const project = await lastValueFrom(this.http.get<any>(`/api/Projects/${projectId}`));
      this.editingProjectTeams.set(this.mapProjectTeams(project.teamIds ?? []));
      this.newProjectTeamId = null;
    } catch (err: any) {
      this.toastService.error(err.message ?? 'Failed to assign team');
    }
  }

  async removeProjectTeam(teamId: string) {
    const projectId = this.editingProjectId();
    if (!projectId) return;
    try {
      await lastValueFrom(this.http.delete(`/api/Projects/${projectId}/teams/${teamId}`));
      const project = await lastValueFrom(this.http.get<any>(`/api/Projects/${projectId}`));
      this.editingProjectTeams.set(this.mapProjectTeams(project.teamIds ?? []));
    } catch (err: any) {
      this.toastService.error(err.message ?? 'Failed to remove team');
    }
  }

  private mapProjectTeams(teamIds: string[]): any[] {
    return teamIds.map(teamId => ({
      teamId,
      name: this.teams().find(t => t.id === teamId)?.name ?? teamId,
    }));
  }
}
