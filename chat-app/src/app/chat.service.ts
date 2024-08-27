import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map, filter, catchError } from 'rxjs/operators';

export interface Message {
  id?: number;
  username: string;
  content: string;
}

@Injectable({
  providedIn: 'root'
})
export class ChatService {

  private apiUrl = 'https://localhost:7029/chat';

  constructor(private http: HttpClient) { }


  sendMessageMethodOne(message: Message): Observable<Message> {
    console.log(message);
    return this.sendMessage( message,'optionA');
  }

  sendMessageMethodTwo(message: Message): Observable<Message> {
    return this.sendMessage( message,'optionB');
  }

  sendMessageMethodThree(message: Message): Observable<Message> {
    return this.sendMessage( message,'optionC');
  }

  sendMessage(message: Message, endpoint:string): Observable<Message> {  
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json; charset=utf-8',
        'Accept': '*/*'
      })
    }

    return this.http.post<Message>(`${this.apiUrl}/${endpoint}`, message, httpOptions);
  }
}
