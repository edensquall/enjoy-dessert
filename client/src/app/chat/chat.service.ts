import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { IAnswer, IAsk } from '../shared/models/chat';
import { catchError, map, of, switchMap, tap } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ChatService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }
  
  getChatEnabled() {
    return this.http.get<{ chatEnabled: boolean }>(this.baseUrl + 'chat/enabled')
      .pipe(map(res => res.chatEnabled));
  }

  getChatToken() {
    const localChatToken = localStorage.getItem('chat_token');

    if (localChatToken) {
      return of(localChatToken);
    } else {
      return this.http.get<string>(this.baseUrl + 'chat/token').pipe(
        tap((res: any) => {
          localStorage.setItem('chat_token', res.chatToken);
        }),
        map((res) => res.chatToken),
        catchError((err) => {
          console.error(err);
          return of('');
        })
      );
    }
  }

  askQuestion(ask: IAsk) {
    return this.getChatToken().pipe(
      switchMap((token) => {
        const headers = new HttpHeaders({ ChatToken: token });
        return this.http.post<IAnswer>(
          this.baseUrl + 'chat', ask, { headers }
        );
      }),
      catchError((err) => {
        console.error(err);
        return of(null as unknown as IAnswer);
      })
    );
  }
}
