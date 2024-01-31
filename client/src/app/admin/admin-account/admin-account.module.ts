import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AdminAccountRoutingModule } from './admin-account-routing.module';
import { AdminAccountComponent } from './admin-account.component';
import { AdminAccountEditComponent } from './admin-account-edit/admin-account-edit.component';
import { CoreModule } from 'src/app/core/core.module';
import { SharedModule } from 'src/app/shared/shared.module';


@NgModule({
  declarations: [
    AdminAccountComponent,
    AdminAccountEditComponent
  ],
  imports: [
    CommonModule,
    AdminAccountRoutingModule,
    CoreModule,
    SharedModule
  ]
})
export class AdminAccountModule { }
