import { HttpClient } from '@angular/common/http';
import { Component, inject, OnInit, signal } from '@angular/core';
import { lastValueFrom } from 'rxjs';
import { DxDataGridModule, DxToolbarModule } from 'devextreme-angular';
import { ToastService } from '../../core/services/toast.service';
import { AuthService } from '../../core/services/auth.service';

@Component({
  selector: 'app-users',
  imports: [DxDataGridModule, DxToolbarModule],
  templateUrl: './users.html',
  styles: ``,
})
export class Users implements OnInit {

  private http = inject(HttpClient);
  private toastService = inject(ToastService);
  private authService = inject(AuthService);

  protected users = signal<any>([]);
  protected loading = signal(true);

  private get perms() { return this.authService.getCurrentUser(); }
  private get isAdmin() { return this.perms?.role === 'Admin'; }
  private has = (p: string) => this.isAdmin || (this.perms?.permissions ?? []).includes(p);

  protected canUpdate = this.has('user.update');
  protected canDelete = this.has('user.delete');

  async ngOnInit() {
    try {
      this.users.set(await this.GetUsers());
    } finally {
      this.loading.set(false);
    }
  }


  async GetUsers() {
    try {
      return lastValueFrom(this.http.get('/api/User'));
    } catch (error: any) {
      this.toastService.error(error.message ?? 'Failed to load users');
      throw error;
    }
  }

}
