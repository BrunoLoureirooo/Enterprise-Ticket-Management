import { HttpClient } from '@angular/common/http';
import { Component, inject, OnInit, signal } from '@angular/core';
import { lastValueFrom } from 'rxjs';
import { DxDataGridModule, DxToolbarModule, DxSelectBoxModule } from 'devextreme-angular';
import { ToastService } from '../../../core/services/toast.service';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-my-tickets',
  standalone: true,
  imports: [DxDataGridModule, DxToolbarModule, DxSelectBoxModule],
  templateUrl: './my-tickets.html',
})
export class MyTickets implements OnInit {
  private http = inject(HttpClient);
  private toastService = inject(ToastService);
  private authService = inject(AuthService);

  private has = (p: string) => {
    const u = this.authService.getCurrentUser();
    return u?.role === 'Admin' || (u?.permissions ?? []).includes(p);
  };

  protected canCreate = this.has('ticket.create');
  protected canUpdate = this.has('ticket.update');

  protected tickets = signal<any[]>([]);
  protected teams = signal<any[]>([]);
  protected projects = signal<any[]>([]);
  protected selectedTeamId = signal<string | null>(null);
  protected loading = signal(true);

  readonly priorityOptions = ['Low', 'Medium', 'High', 'Critical'];
  readonly statusOptions = ['Open', 'InProgress', 'Resolved', 'Closed'];

  get filteredProjects(): any[] {
    const teamId = this.selectedTeamId();
    if (!teamId) return [];
    return this.projects().filter(p => (p.teamIds ?? []).includes(teamId));
  }

  async ngOnInit() {
    await Promise.all([this.loadTickets(), this.loadTeams(), this.loadProjects()]);
  }

  async loadTickets() {
    this.loading.set(true);
    try {
      const data = await lastValueFrom(this.http.get<any[]>('/api/Ticket/my'));
      this.tickets.set(data);
    } catch (e: any) {
      this.toastService.error(e.message ?? 'Failed to load tickets');
    } finally {
      this.loading.set(false);
    }
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

  onInitNewRow() {
    this.selectedTeamId.set(null);
  }

  onEditingStart(e: any) {
    this.selectedTeamId.set(e.data?.teamId ?? null);
  }

  onTeamChange(e: any, form: any) {
    form.updateData('teamId', e.value);
    form.updateData('projectId', null);
    this.selectedTeamId.set(e.value);
  }

  onProjectChange(e: any, form: any) {
    form.updateData('projectId', e.value);
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
      }
      e.component.cancelEditData();
      await this.loadTickets();
    } catch (err: any) {
      this.toastService.error(err.message ?? 'Operation failed');
    }
  }

  async onCloseClick(e: any) {
    try {
      await lastValueFrom(this.http.patch(`/api/Ticket/${e.row.data.id}/close`, {}));
      await this.loadTickets();
    } catch (err: any) {
      this.toastService.error(err.message ?? 'Failed to close ticket');
    }
  }

  isClosable = (e: any) => e.row.data.status !== 'Closed';
}
