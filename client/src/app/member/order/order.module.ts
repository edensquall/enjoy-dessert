import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { OrderRoutingModule } from './order-routing.module';
import { SharedModule } from 'src/app/shared/shared.module';
import { OrderDetailsComponent } from './order-details/order-details.component';
import { OrderComponent } from './order.component';

@NgModule({
  declarations: [OrderComponent, OrderDetailsComponent],
  imports: [CommonModule, SharedModule, OrderRoutingModule],
})
export class OrderModule {}
