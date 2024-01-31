import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminOrderComponent } from './admin-order.component';
import { AdminOrderEditComponent } from './admin-order-edit/admin-order-edit.component';

const routes: Routes = [
  { path: '', component: AdminOrderComponent },
  {
    path: ':id',
    component: AdminOrderEditComponent,
    data: { breadcrumb: { alias: 'orderEdit' } },
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AdminOrderRoutingModule {}
