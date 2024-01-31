import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, of } from 'rxjs';
import { environment } from 'src/environments/environment';
import { IAdminNews } from 'src/app/shared/models/adminNews';
import { IPagination, Pagination } from 'src/app/shared/models/pagination';
import { IAdminProduct } from 'src/app/shared/models/adminProduct';
import { AdminProductParams } from 'src/app/shared/models/adminProductParams';
import { IProductType } from 'src/app/shared/models/productType';

@Injectable({
  providedIn: 'root',
})
export class AdminProductService {
  baseUrl = environment.apiUrl;
  pagination = new Pagination<IAdminProduct>();
  adminProductParams: AdminProductParams = new AdminProductParams();

  constructor(private http: HttpClient) {}

  getAdminProducts() {
    let params = new HttpParams();

    params = params.append('pageIndex', this.adminProductParams.pageNumber);
    params = params.append('pageSize', this.adminProductParams.pageSize);

    return this.http
      .get<IPagination<IAdminProduct>>(this.baseUrl + 'admin/products', {
        params: params,
      })
      .pipe(
        map((response) => {
          this.pagination = response;
          return response;
        })
      );
  }

  getAdminProduct(id: number) {
    return this.http.get<IAdminProduct>(this.baseUrl + 'admin/products/' + id);
  }

  setAdminProductParams(params: AdminProductParams) {
    this.adminProductParams = params;
  }

  getAdminProductParams() {
    return this.adminProductParams;
  }

  createAdminProduct(adminProduct: FormData) {
    return this.http.post<IAdminProduct>(this.baseUrl + 'admin/products', adminProduct);
  }

  updateAdminProduct(adminProduct: FormData) {
    return this.http.put<IAdminProduct>(this.baseUrl + 'admin/products', adminProduct);
  }

  deleteAdminProduct(id: string | number) {
    return this.http.delete<boolean>(this.baseUrl + 'admin/products?id=' + id);
  }

  getProductTypes() {
    return this.http.get<IProductType[]>(this.baseUrl + 'products/types').pipe(
      map((response) => {
        return response;
      })
    );
  }
}
