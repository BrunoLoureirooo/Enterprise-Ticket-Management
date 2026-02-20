import { HttpClient } from '@angular/common/http';
import { Component, inject, OnInit } from '@angular/core';

@Component({
  selector: 'app-users',
  imports: [],
  templateUrl: './users.html',
  styles: ``,
})
export class Users implements OnInit {

  private http = inject(HttpClient);

  ngOnInit(): void {
    this.http.get('https://localhost:5001/api/User').subscribe({
      next: respose => console.log(respose),
      error: err => console.log(err),
      complete: () => console.log('complete')
    });
  }

}
