import { Component, inject, signal } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { DxFormModule } from 'devextreme-angular';
import { RegisterUser } from '../../../../Models/Users/RegisterUser';
import { AuthService } from '../../../../core/services/auth.service';

@Component({
  selector: 'app-register-form',
  imports: [DxFormModule, RouterLink],
  templateUrl: './register-form.html',
  styles: ``,
})
export class RegisterForm {
  private authService = inject(AuthService);
  private router = inject(Router);

  loginLink = signal('/login');

  protected creds = {} as RegisterUser & { confirmPassword: string };
  protected isSubmitting = signal(false);

  protected passwordComparisonTarget = () => this.creds.password;

  protected async onSubmit(e: Event) {
    e.preventDefault();

    if (this.isSubmitting()) return;

    this.isSubmitting.set(true);
    try {
      const { confirmPassword: _, ...registerData } = this.creds;
      const result = await this.authService.register(registerData);
      if (result.ok) {
        this.router.navigate(['/login']);
      } else {
        alert(result.message ?? 'Registration failed');
      }
    } finally {
      this.isSubmitting.set(false);
    }
  }
}
