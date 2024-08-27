import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../auth.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})
export class LoginComponent {
  username: string = '';

  constructor(private authService: AuthService, private router: Router) {}

  login() {
    
    if (this.username.trim()) {
      this.authService.setUsername(this.username);
      this.router.navigate(['/chat']);
    } 
    else {
      alert('Please enter a username.');
    }
  }
}
