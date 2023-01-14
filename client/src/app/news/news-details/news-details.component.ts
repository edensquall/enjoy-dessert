import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { INews } from 'src/app/shared/models/news';
import { BreadcrumbService } from 'xng-breadcrumb';
import { NewsService } from '../news.service';

@Component({
  selector: 'app-news-details',
  templateUrl: './news-details.component.html',
  styleUrls: ['./news-details.component.scss'],
})
export class NewsDetailsComponent implements OnInit {
  news!: INews;
  id = this.activateRoute.snapshot.paramMap.get('id');

  constructor(
    private newsService: NewsService,
    private activateRoute: ActivatedRoute,
    private bcService: BreadcrumbService
  ) {
    this.bcService.set('@newsDetails', ' ');
  }

  ngOnInit(): void {
    this.getNews();
  }

  getNews() {
    this.newsService.getNews(this.id ? +this.id : 0).subscribe({
      next: (response: INews) => {
        this.news = response;
        this.bcService.set('@newsDetails', response.title);
      },
      error: (error: any) => {
        console.log(error);
      },
      complete: () => {},
    });
  }
}
