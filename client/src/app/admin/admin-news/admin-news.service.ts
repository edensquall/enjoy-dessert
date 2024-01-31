import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, of } from 'rxjs';
import { environment } from 'src/environments/environment';
import { IAdminNews } from 'src/app/shared/models/adminNews';
import { AdminNewsParams } from 'src/app/shared/models/adminNewsParams';
import { IPagination, Pagination } from 'src/app/shared/models/pagination';

@Injectable({
  providedIn: 'root',
})
export class AdminNewsService {
  baseUrl = environment.apiUrl;
  adminNewsParams: AdminNewsParams = new AdminNewsParams();

  constructor(private http: HttpClient) {}

  getAdminNewsAll() {
    let params = new HttpParams();

    params = params.append('pageIndex', this.adminNewsParams.pageNumber);
    params = params.append('pageSize', this.adminNewsParams.pageSize);

    return this.http
      .get<IPagination<IAdminNews>>(this.baseUrl + 'admin/news', {
        params: params,
      })
      .pipe(
        map((response) => {
          return response;
        })
      );
  }

  getAdminNews(id: number) {
    return this.http.get<IAdminNews>(this.baseUrl + 'admin/news/' + id);
  }

  setAdminNewsParams(params: AdminNewsParams) {
    this.adminNewsParams = params;
  }

  getAdminNewsParams() {
    return this.adminNewsParams;
  }

  createAdminNews(adminNews: FormData) {
    return this.http.post<IAdminNews>(this.baseUrl + 'admin/news', adminNews);
  }

  updateAdminNews(adminNews: FormData) {
    return this.http.put<IAdminNews>(this.baseUrl + 'admin/news', adminNews);
  }

  deleteAdminNews(id: string | number) {
    return this.http.delete<boolean>(this.baseUrl + 'admin/news?id=' + id);
  }
}
