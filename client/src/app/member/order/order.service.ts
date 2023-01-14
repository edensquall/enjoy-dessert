import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs';
import { IOrder } from 'src/app/shared/models/order';
import { OrderParams } from 'src/app/shared/models/OrderParams';
import { IPagination } from 'src/app/shared/models/pagination';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class OrderService {
  baseUrl = environment.apiUrl;
  orders: IOrder[] = [];

  constructor(private http: HttpClient) {}

  getOrders(orderParams?: OrderParams) {
    let params = new HttpParams();

    if (orderParams) {
      params = params.append('pageIndex', orderParams.pageNumber);
      params = params.append('pageSize', orderParams.pageSize);
    }

    return this.http
      .get<IPagination<IOrder>>(this.baseUrl + 'orders', {
        params: params,
      })
      .pipe(
        map((response) => {
          this.orders = response.data;
          return response;
        })
      );
  }

  getOrder(id: number) {
    return this.http.get<IOrder>(this.baseUrl + 'orders/' + id);
  }

  getLastOrder() {
    return this.http.get<IOrder>(this.baseUrl + 'orders/lastOrder');
  }
}
