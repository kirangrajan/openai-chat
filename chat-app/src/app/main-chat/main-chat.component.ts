import { Component } from '@angular/core';
import { ChatOptionAComponent } from '../chat-option-a/chat-option-a.component';
import { ChatOptionBComponent } from '../chat-option-b/chat-option-b.component';
import { ChatOptionCComponent } from '../chat-option-c/chat-option-c.component';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-main-chat',
  standalone: true,
  imports: [CommonModule, ChatOptionAComponent, ChatOptionBComponent, ChatOptionCComponent],
  templateUrl: './main-chat.component.html',
  styleUrls: ['./main-chat.component.css'],
})
export class MainChatComponent {
  activeChatMethod: number = 1; // Default to ChatMethod1

  setActiveChatMethod(method: number): void {
    this.activeChatMethod = method;
  }
}
