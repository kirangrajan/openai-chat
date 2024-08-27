import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private username: string | null = null;

  constructor() {}

  setUsername(username: string): void {
    this.username = username;
    localStorage.setItem('username', username); 
  }

  getUsername(): string | null {
    if (!this.username) {
      this.username = localStorage.getItem('username'); 
    }
    return this.username;
  }

  isLoggedIn(): boolean {
    return this.username !== null;
  }

  logout(): void {
    this.username = null;
    localStorage.removeItem('username'); 
  }
}
