import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BreadcrumbService } from 'xng-breadcrumb';
import { AdminOrderService } from '../admin-order.service';
import { IOrder } from 'src/app/shared/models/order';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-admin-order-edit',
  templateUrl: './admin-order-edit.component.html',
  styleUrls: ['./admin-order-edit.component.scss']
})
export class AdminOrderEditComponent implements OnInit {
  id = this.activateRoute.snapshot.paramMap.get('id');
  order!: IOrder;
  orderForm!: FormGroup;

  constructor(
    private route: ActivatedRoute,
    private breadcrumbService: BreadcrumbService,
    private adminOrderService: AdminOrderService,
    private activateRoute: ActivatedRoute,
    private fb: FormBuilder,
  ) {
    this.breadcrumbService.set('@orderEdit', ' ');
  }

  ngOnInit(): void {
    this.createOrderForm();
    this.getAdminOrder(this.id ? +this.id : 0);
  }

  createOrderForm() {
    this.orderForm = this.fb.group({
      id: [this.id],
      status: [null, [Validators.required]],
    });
  }

  getAdminOrder(id: number) {
    this.adminOrderService.getAdminOrder(id).subscribe({
      next: (order: IOrder) => {
        this.orderForm?.patchValue(order);
        this.order = order;
        this.breadcrumbService.set(
          '@orderEdit',
          `訂單 #${order.id}`
        );
      },
      error: (error: any) => {
        console.log(error);
      },
      complete: () => {},
    });
  }

  onOrderSubmit() {
    this.adminOrderService.updateAdminOrder(this.orderForm?.value).subscribe({
      next: (order: IOrder) => {
        this.orderForm?.reset(order);
      },
      error: (error: any) => {
        console.log(error);
      },
      complete: () => {},
    });
  }

}
