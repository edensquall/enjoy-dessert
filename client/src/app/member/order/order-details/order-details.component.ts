import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { IOrder } from 'src/app/shared/models/order';
import { BreadcrumbService } from 'xng-breadcrumb';
import { OrderService } from '../order.service';

@Component({
  selector: '[app-order-details]',
  templateUrl: './order-details.component.html',
  styleUrls: ['./order-details.component.scss'],
})
export class OrderDetailsComponent implements OnInit {
  id = this.activateRoute.snapshot.paramMap.get('id');
  order!: IOrder;

  constructor(
    private route: ActivatedRoute,
    private breadcrumbService: BreadcrumbService,
    private orderService: OrderService,
    private activateRoute: ActivatedRoute
  ) {
    this.breadcrumbService.set('@OrderDetailed', ' ');
  }

  ngOnInit(): void {
    this.getOrder(this.id ? +this.id : 0);
  }

  getOrder(id: number) {
    this.orderService.getOrder(id).subscribe({
      next: (order: IOrder) => {
        this.order = order;
        this.breadcrumbService.set(
          '@OrderDetailed',
          `訂單 #${order.id} - ${order.status}`
        );
      },
      error: (error: any) => {
        console.log(error);
      },
      complete: () => {},
    });
  }
}
