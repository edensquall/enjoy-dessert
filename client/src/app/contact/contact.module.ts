import { NgModule } from '@angular/core';
import { ContactComponent } from './contact.component';
import { ContactRoutingModule } from './contact-routing.module';
import { CommonModule } from '@angular/common';

@NgModule({
  declarations: [ContactComponent],
  imports: [CommonModule, ContactRoutingModule],
})
export class ContactModule {}
