import { Component } from '@angular/core';
import { AuthCard } from '../../../shared/components/auth-card/auth-card';
import { RegisterForm } from './register-form/register-form';

@Component({
  selector: 'app-register',
  imports: [AuthCard, RegisterForm],
  templateUrl: './register.html',
  styles: ``,
})
export class Register {

}
