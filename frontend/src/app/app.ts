import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Layout } from './components/Layout/layout';
import { Home } from './pages/home/home';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, Layout, Home],
  templateUrl: './app.html',
})
export class App {
  protected readonly title = signal('frontend');
}
