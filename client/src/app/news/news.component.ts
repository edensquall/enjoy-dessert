import { Component, OnInit } from '@angular/core';
import { INews } from '../shared/models/news';
import { NewsParams } from '../shared/models/newsParams';
import { IPagination } from '../shared/models/pagination';
import { NewsService } from './news.service';

@Component({
  selector: 'app-news',
  templateUrl: './news.component.html',
  styleUrls: ['./news.component.scss'],
})
export class NewsComponent implements OnInit {
  newsAll: INews[] = [];
  newsParams: NewsParams;
  totalCount: number = 0;

  constructor(private newsService: NewsService) {
    this.newsParams = this.newsService.newsParams;
  }

  ngOnInit(): void {
    this.getNewsAll(true);
  }

  getNewsAll(useCache = false) {
    this.newsService.getNewsAll(useCache).subscribe({
      next: (response: IPagination<INews>) => {
        this.newsAll = response.data;
        this.newsParams.pageNumber = response.pageIndex;
        this.newsParams.pageSize = response.pageSize;
        this.totalCount = response.count;
      },
      error: (error: any) => {
        console.log(error);
      },
      complete: () => {},
    });
  }

  onPageChanged(event: any) {
    const params = this.newsService.getNewsParams();
    if (this.newsParams.pageNumber !== event) {
      this.newsParams.pageNumber = event;
      this.newsService.setNewsParams(params);
      this.getNewsAll(true);
    }
  }
}
