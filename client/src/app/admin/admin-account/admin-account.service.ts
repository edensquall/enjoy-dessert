import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, of } from 'rxjs';
import { environment } from 'src/environments/environment';
import { AdminNewsParams } from 'src/app/shared/models/adminNewsParams';
import { IPagination, Pagination } from 'src/app/shared/models/pagination';
import { IAdminUserInfo } from 'src/app/shared/models/adminUserInfo';
import { AdminAccountParams } from 'src/app/shared/models/adminAccountParams';

@Injectable({
  providedIn: 'root',
})
export class AdminAccountService {
  baseUrl = environment.apiUrl;
  pagination = new Pagination<IAdminUserInfo>();
  adminAccountParams: AdminAccountParams = new AdminAccountParams();

  constructor(private http: HttpClient) {}

  getAdminAccounts() {
    let params = new HttpParams();

    params = params.append('pageIndex', this.adminAccountParams.pageNumber);
    params = params.append('pageSize', this.adminAccountParams.pageSize);

    return this.http
      .get<IPagination<IAdminUserInfo>>(this.baseUrl + 'admin/account', {
        params: params,
      })
      .pipe(
        map((response) => {
          this.pagination = response;
          return response;
        })
      );
  }

  getAdminAccount(id: string) {
    return this.http.get<IAdminUserInfo>(this.baseUrl + 'admin/account/' + id);
  }

  setAdminAccountParams(params: AdminAccountParams) {
    this.adminAccountParams = params;
  }

  getAdminAccountParams() {
    return this.adminAccountParams;
  }

  createAdminAccount(admin: IAdminUserInfo) {
    return this.http.post<IAdminUserInfo>(
      this.baseUrl + 'admin/account',
      admin
    );
  }

  updateAdminAccount(admin: IAdminUserInfo) {
    return this.http.put<IAdminUserInfo>(
      this.baseUrl + 'admin/account',
      admin
    );
  }

  deleteAdminAccount(id: string) {
    return this.http.delete<boolean>(
      this.baseUrl + 'admin/account?id=' + id
    );
  }
}
