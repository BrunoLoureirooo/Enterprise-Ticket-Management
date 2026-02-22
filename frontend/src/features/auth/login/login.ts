import { Component } from '@angular/core';
import { AuthCard } from '../../../shared/components/auth-card/auth-card';
import { LoginForm } from './login-form/login-form';

@Component({
  selector: 'app-login',
  imports: [AuthCard, LoginForm],
  templateUrl: './login.html',
  styles: ``,
})
export class Login { }
