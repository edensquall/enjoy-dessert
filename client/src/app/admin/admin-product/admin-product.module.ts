import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AdminProductRoutingModule } from './admin-product-routing.module';
import { AdminProductComponent } from './admin-product.component';
import { AdminProductEditComponent } from './admin-product-edit/admin-product-edit.component';
import { SharedModule } from 'src/app/shared/shared.module';


@NgModule({
  declarations: [
    AdminProductComponent,
    AdminProductEditComponent
  ],
  imports: [
    CommonModule,
    AdminProductRoutingModule,
    SharedModule,
  ]
})
export class AdminProductModule { }
