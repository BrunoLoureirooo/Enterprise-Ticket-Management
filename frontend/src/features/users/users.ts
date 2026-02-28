import { HttpClient } from '@angular/common/http';
import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { lastValueFrom } from 'rxjs';
import { DxDataGridModule, DxToolbarModule } from 'devextreme-angular';
import { ToastService } from '../../core/services/toast.service';

@Component({
  selector: 'app-users',
  imports: [DxDataGridModule, DxToolbarModule],
  templateUrl: './users.html',
  styles: ``,
})
export class Users implements OnInit {

  private http = inject(HttpClient);
  private toastService = inject(ToastService);

  protected users = signal<any>([]);
  protected loading = signal(true);

  async ngOnInit() {
    try {
      this.users.set(await this.GetUsers());
    } finally {
      this.loading.set(false);
    }
  }


  async GetUsers() {
    try {
      return lastValueFrom(this.http.get('https://localhost:5001/api/User'));
    } catch (error: any) {
      this.toastService.error(error.message ?? 'Failed to load users');
      throw error;
    }
  }

}
