import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AdminOrderRoutingModule } from './admin-order-routing.module';
import { AdminOrderComponent } from './admin-order.component';
import { AdminOrderEditComponent } from './admin-order-edit/admin-order-edit.component';
import { SharedModule } from 'src/app/shared/shared.module';


@NgModule({
  declarations: [
    AdminOrderComponent,
    AdminOrderEditComponent
  ],
  imports: [
    CommonModule,
    AdminOrderRoutingModule,
    SharedModule,
  ]
})
export class AdminOrderModule { }
