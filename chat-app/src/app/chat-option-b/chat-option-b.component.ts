import { Component } from '@angular/core';

import { ChatService } from '../chat.service';
import { AuthService } from '../auth.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
@Component({
  selector: 'app-chat-option-b',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './chat-option-b.component.html',
  styleUrl: './chat-option-b.component.css'
})
export class ChatOptionBComponent {
  messages: string[] = [];
  messageText: string = '';

  constructor(private chatService: ChatService, private authService: AuthService) { }

  sendMessage() {
    const username = this.authService.getUsername();
    if (username) {
      const message = `${this.messageText}`;
    this.messages.push(`${username}: ${this.messageText}`);
    this.messageText = '';

    const messageToSend = {
      username: username,
      content: message
    };

    this.chatService.sendMessageMethodTwo(messageToSend).subscribe(response => {
      this.messages.push(response.content);
      this.messageText = '';
    }); } else {
      alert('Please log in.');
    }
  }
}