import { Component, OnInit } from '@angular/core';
import { IOrder } from 'src/app/shared/models/order';
import { OrderParams } from 'src/app/shared/models/orderParams';
import { IPagination } from 'src/app/shared/models/pagination';
import { OrderService } from './order.service';

@Component({
  selector: '[app-order] .col-12 .col-sm-8 .col-md-9',
  templateUrl: './order.component.html',
  styleUrls: ['./order.component.scss'],
})
export class OrderComponent implements OnInit {
  orders!: IOrder[];
  orderParams: OrderParams = new OrderParams();
  totalCount: number = 0;

  constructor(private orderService: OrderService) {}

  ngOnInit(): void {
    this.getOrders();
  }

  getOrders() {
    this.orderService.getOrders(this.orderParams).subscribe({
      next: (response: IPagination<IOrder>) => {
        this.orders = response.data;
        this.orderParams.pageNumber = response.pageIndex;
        this.orderParams.pageSize = response.pageSize;
        this.totalCount = response.count;
      },
      error: (error: any) => {
        console.log(error);
      },
      complete: () => {},
    });
  }

  onPageChanged(event: any) {
    if (this.orderParams.pageNumber !== event) {
      this.orderParams.pageNumber = event;
      this.getOrders();
    }
  }
}
