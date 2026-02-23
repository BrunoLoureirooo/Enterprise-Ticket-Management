import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { DxFormModule } from 'devextreme-angular/ui/form';
import { DxLoadIndicatorModule } from 'devextreme-angular/ui/load-indicator';
import { DxButtonModule } from 'devextreme-angular/ui/button';
import { AuthService } from '../../../../core/services/auth.service';
import { ToastService } from '../../../../core/services/toast.service';

@Component({
  selector: 'app-login-form',
  templateUrl: './login-form.html',
  styles: ``,
  imports: [
    CommonModule,
    RouterModule,
    DxFormModule,
    DxLoadIndicatorModule,
    DxButtonModule,
  ]
})
export class LoginForm {
  resetLink = signal('/reset-password');
  createAccountLink = signal('/register');

  private authService = inject(AuthService);
  private router = inject(Router);
  private toastService = inject(ToastService);

  loading = signal(false);

  formData: { username: string; password: string } = {
    username: '',
    password: '',
  };

  async onSubmit(e: Event) {
    e.preventDefault();
    if (this.loading()) return;

    this.loading.set(true);
    try {
      const result = await this.authService.logIn(
        this.formData.username,
        this.formData.password
      );

      if (!result.ok) {
        this.toastService.error(result.message ?? 'Login failed');
        return;
      }

      this.toastService.success('Login successful');
      this.router.navigate(['/']);
    } catch (err) {
      this.toastService.error('An unexpected error occurred');
    } finally {
      this.loading.set(false);
    }
  }
}