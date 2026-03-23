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

  private currentUser = this.authService.getCurrentUser();
  private isAdmin = this.currentUser?.role === 'Admin';
  private isTeamLeader = this.currentUser?.isTeamLeader ?? false;

  private has = (p: string) =>
    this.isAdmin || this.isTeamLeader || (this.currentUser?.permissions ?? []).includes(p);

  protected canCreate = this.has('ticket.create');
  protected canUpdate = this.has('ticket.update');
  protected canDelete = this.has('ticket.delete');

  protected tickets = signal<any[]>([]);
  protected users = signal<any[]>([]);
  protected teams = signal<any[]>([]);
  protected projects = signal<any[]>([]);
  protected teamMembers = signal<any[]>([]);
  protected selectedTeamId = signal<string | null>(null);
  protected selectedProjectId = signal<string | null>(null);
  protected selectedAssigneeId = signal<string | null>(null);
  protected loading = signal(true);

  // Cache of full team objects (with members) for team leaders
  private teamsWithMembers: any[] = [];

  readonly statusOptions = ['Open', 'InProgress', 'Resolved', 'Closed'];
  readonly priorityOptions = ['Low', 'Medium', 'High', 'Critical'];

  get filteredProjects(): any[] {
    const teamId = this.selectedTeamId();
    if (!teamId) return [];
    return this.projects().filter(p => (p.teamIds ?? []).includes(teamId));
  }

  get assigneeOptions(): any[] {
    return this.teamMembers().map(m => ({ id: m.userId, nome: m.nome || m.userId }));
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
      if (this.isAdmin) {
        const data = await lastValueFrom(this.http.get<any[]>('/api/Teams'));
        this.teams.set(data);
      } else {
        // Team leaders: scoped endpoint returns TeamDto with full members list
        const data = await lastValueFrom(this.http.get<any[]>('/api/Teams/mine'));
        this.teamsWithMembers = data;
        this.teams.set(data);
      }
    } catch {}
  }

  async loadProjects() {
    try {
      const endpoint = this.isAdmin ? '/api/Projects' : '/api/Projects/mine';
      const data = await lastValueFrom(this.http.get<any[]>(endpoint));
      this.projects.set(data);
    } catch {}
  }

  async loadTeamMembers(teamId: string) {
    if (!this.isAdmin && this.teamsWithMembers.length > 0) {
      const team = this.teamsWithMembers.find(t => t.id === teamId);
      this.teamMembers.set(team?.members ?? []);
      return;
    }
    try {
      const team = await lastValueFrom(this.http.get<any>(`/api/Teams/${teamId}`));
      this.teamMembers.set(team.members ?? []);
    } catch {
      this.teamMembers.set([]);
    }
  }

  onInitNewRow() {
    this.selectedTeamId.set(null);
    this.selectedProjectId.set(null);
    this.selectedAssigneeId.set(null);
    this.teamMembers.set([]);
  }

  onEditingStart(e: any) {
    const teamId = e.data?.teamId ?? null;
    this.selectedTeamId.set(teamId);
    this.selectedProjectId.set(e.data?.projectId ?? null);
    this.selectedAssigneeId.set(e.data?.assignedToId ?? null);
    if (teamId) {
      this.loadTeamMembers(teamId);
    } else {
      this.teamMembers.set([]);
    }
  }

  async onTeamChange(e: any, _form: any) {
    if (!e.event) return;
    this.selectedTeamId.set(e.value);
    this.selectedProjectId.set(null);
    this.selectedAssigneeId.set(null);
    if (e.value) {
      await this.loadTeamMembers(e.value);
    } else {
      this.teamMembers.set([]);
    }
  }

  onProjectChange(e: any, _form: any) {
    if (!e.event) return;
    this.selectedProjectId.set(e.value);
  }

  onAssigneeChange(e: any, _form: any) {
    if (!e.event) return;
    this.selectedAssigneeId.set(e.value);
  }

  async onSaving(e: any) {
    e.cancel = true;
    const change = e.changes[0];
    if (!change) return;

    const customFields = {
      teamId: this.selectedTeamId(),
      projectId: this.selectedProjectId(),
      assignedToId: this.selectedAssigneeId(),
    };

    try {
      if (change.type === 'insert') {
        await lastValueFrom(this.http.post('/api/Ticket', { ...change.data, ...customFields }));
      } else if (change.type === 'update') {
        const original = this.tickets().find((t: any) => t.id === change.key) ?? {};
        await lastValueFrom(this.http.put(`/api/Ticket/${change.key}`, { ...original, ...change.data, ...customFields }));
      } else if (change.type === 'remove') {
        await lastValueFrom(this.http.delete(`/api/Ticket/${change.key}`));
      }
      setTimeout(() => e.component.cancelEditData());
      await this.loadTickets();
    } catch (err: any) {
      this.toastService.error(err.message ?? 'Operation failed');
    }
  }
}
