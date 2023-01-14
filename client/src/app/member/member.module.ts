import { NgModule } from '@angular/core';
import { MemberComponent } from './member.component';
import { MemberRoutingModule } from './member-routing.module';
import { MemberCenterComponent } from './member-center/member-center.component';
import { MemberMenuComponent } from './member-menu/member-menu.component';
import { PersonalInfoComponent } from './personal-info/personal-info.component';
import { SharedModule } from '../shared/shared.module';
import { CommonModule } from '@angular/common';

@NgModule({
  declarations: [
    MemberComponent,
    MemberCenterComponent,
    MemberMenuComponent,
    PersonalInfoComponent,
  ],
  imports: [CommonModule, MemberRoutingModule, SharedModule],
})
export class MemberModule {}
