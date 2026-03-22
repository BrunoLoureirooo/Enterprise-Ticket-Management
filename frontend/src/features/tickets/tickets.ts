import { HttpClient } from '@angular/common/http';
import { Component, inject, OnInit, signal } from '@angular/core';
import { lastValueFrom } from 'rxjs';
import { DxDataGridModule, DxToolbarModule, DxSelectBoxModule } from 'devextreme-angular';
import { ToastService } from '../../core/services/toast.service';
import { AuthService } from '../../core/services/auth.service';

@Component({
  selector: 'app-tickets',
  standalone: true,
  imports: [DxDataGridModule, DxToolbarModule, DxSelectBoxModule],
  templateUrl: './tickets.html',
})
export class Tickets implements OnInit {
  private http = inject(HttpClient);
  private toastService = inject(ToastService);
  private authService = inject(AuthService);

  private has = (p: string) => {
    const u = this.authService.getCurrentUser();
    return u?.role === 'Admin' || u?.isTeamLeader === true || (u?.permissions ?? []).includes(p);
  };

  protected canCreate = this.has('ticket.create');
  protected canUpdate = this.has('ticket.update');
  protected canDelete = this.has('ticket.delete');

  protected tickets = signal<any[]>([]);
  protected users = signal<any[]>([]);
  protected teams = signal<any[]>([]);
  protected projects = signal<any[]>([]);
  protected teamMembers = signal<any[]>([]);
  protected selectedTeamId = signal<string | null>(null);
  protected loading = signal(true);

  readonly statusOptions = ['Open', 'InProgress', 'Resolved', 'Closed'];
  readonly priorityOptions = ['Low', 'Medium', 'High', 'Critical'];

  get filteredProjects(): any[] {
    const teamId = this.selectedTeamId();
    if (!teamId) return [];
    return this.projects().filter(p => (p.teamIds ?? []).includes(teamId));
  }

  get assigneeOptions(): any[] {
    const members = this.teamMembers();
    if (!members.length) return this.users();
    return members.map(m => this.users().find(u => u.id === m.userId) ?? { id: m.userId, name: m.userId });
  }

  async ngOnInit() {
    await Promise.all([this.loadTickets(), this.loadUsers(), this.loadTeams(), this.loadProjects()]);
  }

  async loadTickets() {
    this.loading.set(true);
    try {
      const data = await lastValueFrom(this.http.get<any[]>('/api/Ticket'));
      this.tickets.set(data);
    } catch (e: any) {
      this.toastService.error(e.message ?? 'Failed to load tickets');
    } finally {
      this.loading.set(false);
    }
  }

  async loadUsers() {
    try {
      const data = await lastValueFrom(this.http.get<any[]>('/api/User'));
      this.users.set(data);
    } catch {}
  }

  async loadTeams() {
    try {
      const data = await lastValueFrom(this.http.get<any[]>('/api/Teams'));
      this.teams.set(data);
    } catch {}
  }

  async loadProjects() {
    try {
      const data = await lastValueFrom(this.http.get<any[]>('/api/Projects'));
      this.projects.set(data);
    } catch {}
  }

  async loadTeamMembers(teamId: string) {
    try {
      const team = await lastValueFrom(this.http.get<any>(`/api/Teams/${teamId}`));
      this.teamMembers.set(team.members ?? []);
    } catch {
      this.teamMembers.set([]);
    }
  }

  onInitNewRow() {
    this.selectedTeamId.set(null);
    this.teamMembers.set([]);
  }

  onEditingStart(e: any) {
    const teamId = e.data?.teamId ?? null;
    this.selectedTeamId.set(teamId);
    if (teamId) {
      this.loadTeamMembers(teamId);
    } else {
      this.teamMembers.set([]);
    }
  }

  async onTeamChange(e: any, form: any) {
    form.updateData('teamId', e.value);
    form.updateData('projectId', null);
    form.updateData('assignedToId', null);
    this.selectedTeamId.set(e.value);
    if (e.value) {
      await this.loadTeamMembers(e.value);
    } else {
      this.teamMembers.set([]);
    }
  }

  onProjectChange(e: any, form: any) {
    form.updateData('projectId', e.value);
  }

  onAssigneeChange(e: any, form: any) {
    form.updateData('assignedToId', e.value);
  }

  async onSaving(e: any) {
    e.cancel = true;
    const change = e.changes[0];
    if (!change) return;

    try {
      if (change.type === 'insert') {
        await lastValueFrom(this.http.post('/api/Ticket', change.data));
      } else if (change.type === 'update') {
        await lastValueFrom(this.http.put(`/api/Ticket/${change.key}`, change.data));
      } else if (change.type === 'remove') {
        await lastValueFrom(this.http.delete(`/api/Ticket/${change.key}`));
      }
      e.component.cancelEditData();
      await this.loadTickets();
    } catch (err: any) {
      this.toastService.error(err.message ?? 'Operation failed');
    }
  }
}
