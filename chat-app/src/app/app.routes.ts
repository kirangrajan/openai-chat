import { Routes } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { MainChatComponent } from './main-chat/main-chat.component';

export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'chat', component: MainChatComponent },
  { path: '', redirectTo: '/login', pathMatch: 'full' }
];

