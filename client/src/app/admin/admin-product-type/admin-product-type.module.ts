import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AdminProductTypeRoutingModule } from './admin-product-type-routing.module';
import { AdminProductTypeComponent } from './admin-product-type.component';
import { SharedModule } from 'src/app/shared/shared.module';


@NgModule({
  declarations: [
    AdminProductTypeComponent
  ],
  imports: [
    CommonModule,
    AdminProductTypeRoutingModule,
    SharedModule,
  ]
})
export class AdminProductTypeModule { }
