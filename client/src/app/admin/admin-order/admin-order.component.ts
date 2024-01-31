import { Component, OnInit } from '@angular/core';
import { IOrder } from 'src/app/shared/models/order';
import { OrderParams } from 'src/app/shared/models/orderParams';
import { AdminOrderService } from './admin-order.service';
import { IPagination } from 'src/app/shared/models/pagination';
import { OrderStatus } from 'src/app/shared/models/orderStatus';
import { IAdminOrder } from 'src/app/shared/models/adminOrder';

@Component({
  selector: 'app-admin-order',
  templateUrl: './admin-order.component.html',
  styleUrls: ['./admin-order.component.scss'],
})
export class AdminOrderComponent implements OnInit {
  readonly OrderStatus = OrderStatus;
  orders!: IAdminOrder[];
  orderParams: OrderParams = new OrderParams();
  totalCount: number = 0;

  constructor(private adminOrderService: AdminOrderService) {}

  ngOnInit(): void {
    this.getAdminOrders();
  }

  getAdminOrders() {
    this.adminOrderService.getAdminOrders(this.orderParams).subscribe({
      next: (response: IPagination<IAdminOrder>) => {
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
      this.getAdminOrders();
    }
  }
}
