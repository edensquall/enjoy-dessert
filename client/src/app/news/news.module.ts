import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NewsComponent } from './news.component';
import { SharedModule } from '../shared/shared.module';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { NewsItemComponent } from './news-item/news-item.component';
import { RouterModule } from '@angular/router';
import { NewsDetailsComponent } from './news-details/news-details.component';
import { NewsRoutingModule } from './news-routing.module';

@NgModule({
  declarations: [NewsComponent, NewsItemComponent, NewsDetailsComponent],
  imports: [CommonModule, SharedModule, PaginationModule, NewsRoutingModule],
})
export class NewsModule {}
