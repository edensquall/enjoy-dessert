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
export class AdminProductTypeService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  getProductTypes() {
    return this.http
      .get<IProductType[]>(this.baseUrl + 'admin/productTypes')
      .pipe(
        map((response) => {
          return response;
        })
      );
  }

  getProductType(id: number) {
    return this.http.get<IProductType>(
      this.baseUrl + 'admin/productTypes/' + id
    );
  }

  createProductType(productType: IProductType) {
    return this.http.post<IProductType>(
      this.baseUrl + 'admin/productTypes',
      productType
    );
  }

  updateProductType(productType: IProductType) {
    return this.http.put<IProductType>(
      this.baseUrl + 'admin/productTypes',
      productType
    );
  }

  deleteProductType(id: number) {
    return this.http.delete<boolean>(
      this.baseUrl + 'admin/productTypes?id=' + id
    );
  }
}
