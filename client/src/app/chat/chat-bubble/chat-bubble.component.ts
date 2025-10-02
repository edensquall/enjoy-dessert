import { Component, Input, OnInit } from '@angular/core';
import { IChat } from 'src/app/shared/models/chat';

@Component({
  selector: 'app-chat-bubble',
  templateUrl: './chat-bubble.component.html',
  styleUrls: ['./chat-bubble.component.scss']
})
export class ChatBubbleComponent implements OnInit {
   @Input() chat!: IChat;

  constructor() { }

  ngOnInit(): void {
  }

}
