import { Component, inject, OnInit, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { lastValueFrom } from 'rxjs';
import { DxChartModule, DxPieChartModule, DxLoadIndicatorModule, DxToolbarModule } from 'devextreme-angular';

interface TicketStats {
  unresolvedByStatus: { status: string; count: number }[];
  byPriority: { priority: string; count: number }[];
  teamUnresolvedByPriority: { priority: string; count: number }[] | null;
  isTeamLeader: boolean;
  isAdmin: boolean;
}

interface ChartItem {
  label: string;
  count: number;
}

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [DxChartModule, DxPieChartModule, DxLoadIndicatorModule, DxToolbarModule],
  templateUrl: './home.html',
})
export class Home implements OnInit {
  private http = inject(HttpClient);

  protected stats = signal<TicketStats | null>(null);
  protected teams = signal<any[]>([]);
  protected projects = signal<any[]>([]);
  protected loading = signal(true);

  protected get unresolvedData(): ChartItem[] {
    return (this.stats()?.unresolvedByStatus ?? []).map(x => ({ label: x.status, count: x.count }));
  }

  protected get priorityData(): ChartItem[] {
    return (this.stats()?.byPriority ?? []).map(x => ({ label: x.priority, count: x.count }));
  }

  protected get teamUrgencyData(): ChartItem[] {
    return (this.stats()?.teamUnresolvedByPriority ?? []).map(x => ({ label: x.priority, count: x.count }));
  }

  protected get isTeamLeader(): boolean {
    return this.stats()?.isTeamLeader ?? false;
  }

  protected get isAdmin(): boolean {
    return this.stats()?.isAdmin ?? false;
  }

  protected projectsForTeam(teamId: string): any[] {
    return this.projects().filter(p => (p.teamIds as string[]).includes(teamId));
  }

  async ngOnInit() {
    try {
      const data = await lastValueFrom(this.http.get<TicketStats>('/api/Ticket/stats'));
      this.stats.set(data);

      if (data.isTeamLeader) {
        const [teams, projects] = await Promise.allSettled([
          lastValueFrom(this.http.get<any[]>('/api/Teams/mine')),
          lastValueFrom(this.http.get<any[]>('/api/Projects')),
        ]);
        if (teams.status === 'fulfilled') this.teams.set(teams.value);
        if (projects.status === 'fulfilled') this.projects.set(projects.value);
      }
    } catch {
    } finally {
      this.loading.set(false);
    }
  }
}
