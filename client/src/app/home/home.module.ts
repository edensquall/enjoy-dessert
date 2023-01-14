import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HomeComponent } from './home.component';
import { BestsellerItemComponent } from './bestseller-item/bestseller-item.component';
import { HomeRoutingModule } from './home-routing.module';
import { SharedModule } from '../shared/shared.module';

@NgModule({
  declarations: [HomeComponent, BestsellerItemComponent],
  imports: [CommonModule, HomeRoutingModule, SharedModule],
  exports: [HomeComponent],
})
export class HomeModule {}
