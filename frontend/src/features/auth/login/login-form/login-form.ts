import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { DxFormModule } from 'devextreme-angular/ui/form';
import { DxLoadIndicatorModule } from 'devextreme-angular/ui/load-indicator';
import { DxButtonModule } from 'devextreme-angular/ui/button';
import notify from 'devextreme/ui/notify';
import { AuthService } from '../../../../core/services/auth.service';

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

  loading = false;

  formData: { username: string; password: string } = {
    username: '',
    password: '',
  };

  async onSubmit(e: Event) {
    e.preventDefault();
    this.loading = true;

    const result = await this.authService.logIn(
      this.formData.username,
      this.formData.password
    );
    this.loading = false;

    if (!result.ok) {
      notify(result.message ?? 'Login failed', 'error', 3000);
      return;
    }

    this.router.navigate(['/']);
  }
}