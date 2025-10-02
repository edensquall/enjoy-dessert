import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ChatComponent } from './chat.component';
import { FormsModule } from '@angular/forms';
import { ChatBubbleComponent } from './chat-bubble/chat-bubble.component';  

@NgModule({
  declarations: [ChatComponent, ChatBubbleComponent, ChatBubbleComponent],
  imports: [CommonModule, FormsModule],
  exports: [ChatComponent],
})
export class ChatModule {}
