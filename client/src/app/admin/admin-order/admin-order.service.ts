import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs';
import { IAdminOrder } from 'src/app/shared/models/adminOrder';
import { IOrder } from 'src/app/shared/models/order';
import { OrderParams } from 'src/app/shared/models/orderParams';
import { IPagination } from 'src/app/shared/models/pagination';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class AdminOrderService {
  baseUrl = environment.apiUrl;
  orders: IAdminOrder[] = [];

  constructor(private http: HttpClient) {}

  getAdminOrders(orderParams?: OrderParams) {
    let params = new HttpParams();

    if (orderParams) {
      params = params.append('pageIndex', orderParams.pageNumber);
      params = params.append('pageSize', orderParams.pageSize);
    }

    return this.http
      .get<IPagination<IAdminOrder>>(this.baseUrl + 'admin/orders', {
        params: params,
      })
      .pipe(
        map((response) => {
          this.orders = response.data;
          return response;
        })
      );
  }

  getAdminOrder(id: number) {
    return this.http.get<IAdminOrder>(this.baseUrl + 'admin/orders/' + id);
  }

  updateAdminOrder(order: IOrder) {
    return this.http.put<IOrder>(this.baseUrl + 'admin/orders', order);
  }
}
