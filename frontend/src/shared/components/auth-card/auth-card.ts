import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';


@Component({
  selector: 'app-auth-card',
  imports: [CommonModule],
  templateUrl: './auth-card.html',
  styleUrls: ['./auth-card.css'],
})
export class AuthCard {
  @Input()
  title!: string;

  @Input()
  description!: string;
}