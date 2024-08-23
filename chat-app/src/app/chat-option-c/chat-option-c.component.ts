import { Component } from '@angular/core';

import { ChatService } from '../chat.service';
import { AuthService } from '../auth.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
@Component({
  selector: 'app-chat-option-c',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './chat-option-c.component.html',
  styleUrl: './chat-option-c.component.css'
})
export class ChatOptionCComponent {
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

    this.chatService.sendMessageMethodThree(messageToSend).subscribe(response => {
      this.messages.push(response.content);
      this.messageText = '';
    });} else {
      alert('Please log in.');
    }
  }
}
