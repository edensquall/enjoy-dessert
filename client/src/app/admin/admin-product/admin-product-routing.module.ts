import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminProductComponent } from './admin-product.component';
import { AdminProductEditComponent } from './admin-product-edit/admin-product-edit.component';

const routes: Routes = [
  { path: '', component: AdminProductComponent },
  {
    path: ':id',
    component: AdminProductEditComponent,
    data: { breadcrumb: { alias: 'editProduct' } },
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AdminProductRoutingModule {}
