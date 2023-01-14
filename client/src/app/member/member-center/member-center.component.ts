import { Component, OnInit } from '@angular/core';
import { UserInfo } from 'os';
import { AccountService } from 'src/app/account/account.service';
import { IOrder } from 'src/app/shared/models/order';
import { IUserInfo } from 'src/app/shared/models/userInfo';
import { OrderService } from '../order/order.service';

@Component({
  selector: '[app-member-center] .col-12 .col-sm-8 .col-md-9',
  templateUrl: './member-center.component.html',
  styleUrls: ['./member-center.component.scss'],
})
export class MemberCenterComponent implements OnInit {
  userInfo!: IUserInfo;
  order!: IOrder;

  constructor(
    private accountService: AccountService,
    private orderService: OrderService
  ) {}

  ngOnInit(): void {
    this.getUserInfo();
    this.getLastOrder();
  }

  getUserInfo() {
    this.accountService.getUserInfo().subscribe({
      next: (userInfo: IUserInfo) => {
        this.userInfo = userInfo;
      },
      error: (error: any) => {
        console.log(error);
      },
      complete: () => {},
    });
  }

  getLastOrder() {
    this.orderService.getLastOrder().subscribe({
      next: (order: IOrder) => {
        this.order = order;
      },
      error: (error: any) => {
        console.log(error);
      },
      complete: () => {},
    });
  }

  logout() {
    this.accountService.logout();
  }
}
