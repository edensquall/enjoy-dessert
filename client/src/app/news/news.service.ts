import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, of } from 'rxjs';
import { environment } from 'src/environments/environment';
import { INews } from '../shared/models/news';
import { NewsParams } from '../shared/models/newsParams';
import { IPagination, Pagination } from '../shared/models/pagination';

@Injectable({
  providedIn: 'root',
})
export class NewsService {
  baseUrl = environment.apiUrl;
  pagination = new Pagination<INews>();
  newsParams: NewsParams = new NewsParams();
  newsCache = new Map<string, Pagination<INews>>();

  constructor(private http: HttpClient) {}

  getNewsAll(useCache: boolean) {
    if (useCache === false) {
      this.newsCache = new Map<string, Pagination<INews>>();
    }

    if (this.newsCache.size > 0 && useCache === true) {
      if (this.newsCache.has(JSON.stringify(this.newsParams))) {
        this.pagination =
          this.newsCache.get(JSON.stringify(this.newsParams)) ||
          new Pagination<INews>();
        return of(this.pagination);
      }
    }

    let params = new HttpParams();

    params = params.append('pageIndex', this.newsParams.pageNumber);
    params = params.append('pageSize', this.newsParams.pageSize);

    return this.http
      .get<IPagination<INews>>(this.baseUrl + 'news', {
        params: params,
      })
      .pipe(
        map((response) => {
          this.newsCache.set(JSON.stringify(this.newsParams), response);
          this.pagination = response;
          return response;
        })
      );
  }

  getNews(id: number) {
    let news: INews | undefined;

    for (let [, newsAll] of this.newsCache) {
      news = newsAll.data.find((p) => p.id === id);
      if (news) {
        return of(news);
      }
    }

    return this.http.get<INews>(this.baseUrl + 'news/' + id);
  }

  setNewsParams(params: NewsParams) {
    this.newsParams = params;
  }

  getNewsParams() {
    return this.newsParams;
  }
}
