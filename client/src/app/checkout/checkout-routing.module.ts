import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CheckoutComponent } from './checkout.component';
import { RouterModule, Routes } from '@angular/router';
import { CheckoutSuccessComponent } from './checkout-success/checkout-success.component';

const routes: Routes = [
  { path: '', component: CheckoutComponent },
  {
    path: 'success',
    component: CheckoutSuccessComponent,
    data: { breadcrumb: '訂單成立' },
  },
];

@NgModule({
  declarations: [],
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class CheckoutRoutingModule {}
