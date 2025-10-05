import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ChatService } from './chat.service';
import { IAnswer, IChat } from '../shared/models/chat';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.scss'],
})
export class ChatComponent implements OnInit {
  @ViewChild('chatBody') private chatBody!: ElementRef;
  chatEnabled = false;
  isChatWindowVisible: boolean = false;
  isToggleBtnVisible: boolean = true;
  input: string = '';
  loading: boolean = false;
  isComposing = false;
  shouldScroll = false;

  chats: IChat[] = [
    {
      speaker: 'ai',
      content: `
        <p>歡迎光臨 <strong>enjoy<span class="red-dot">.</span></strong>！我是智能客服助理，能提供：</p>
        <ul>
          <li>甜點種類與詳細資訊</li>
          <li>熱銷商品推薦</li>
          <li>回答甜點相關問題</li>
        </ul>
        <p>您想先了解哪一部分呢？</p>
        `,
      timestamp: new Date(),
      isHtml: true,
    },
  ];

  constructor(private chatService: ChatService) {}

  ngOnInit(): void {
    this.chatService.getChatEnabled().subscribe((chatEnabled) => {
      this.chatEnabled = chatEnabled;
    });
  }

  ngAfterViewChecked() {
    if (this.shouldScroll) {
      this.scrollToBottom();
      this.shouldScroll = false;
    }
  }

  onCompositionStart() {
    this.isComposing = true;
  }

  onCompositionEnd() {
    this.isComposing = false;
  }

  onKeyDown(event: KeyboardEvent) {
    if (event.key === 'Enter' && !this.isComposing) {
      this.sendMessage();
      event.preventDefault();
    }
  }

  toggleVisibility() {
    this.isChatWindowVisible = !this.isChatWindowVisible;

    if (window.innerWidth <= 576) {
      this.isToggleBtnVisible = false;
    }
  }

  addMessage(content: string, speaker: 'user' | 'ai') {
    this.chats.push({ speaker, content, timestamp: new Date() });
    this.shouldScroll = true;
  }

  closeChat() {
    this.isChatWindowVisible = false;
    this.isToggleBtnVisible = true;
  }

  clearInput() {
    this.input = '';
  }

  scrollToBottom() {
    try {
      if (this.chatBody?.nativeElement) {
        this.chatBody.nativeElement.scrollTop =
          this.chatBody.nativeElement.scrollHeight;
      }
    } catch (error: any) {
      console.log(error);
    }
  }

  sendMessage() {
    if (!this.input.trim()) return;
    const question = this.input.trim();
    if (!question) return;

    this.addMessage(this.input, 'user');
    this.clearInput();
    this.loading = true;

    this.chatService.askQuestion({ question: question }).subscribe({
      next: (res: IAnswer) => {
        this.addMessage(res.answer, 'ai');
      },
      error: (error: any) => {
        console.log(error);
      },
      complete: () => {
        this.loading = false;
      },
    });
  }
}
