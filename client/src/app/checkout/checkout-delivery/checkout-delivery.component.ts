import { Component, Input, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { BasketService } from 'src/app/basket/basket.service';
import { IDeliveryMethod } from 'src/app/shared/models/deliveryMethod';
import { HelperService } from 'src/app/shared/services/helper.service';
import { CheckoutService } from '../checkout.service';

@Component({
  selector: '[app-checkout-delivery]',
  templateUrl: './checkout-delivery.component.html',
  styleUrls: ['./checkout-delivery.component.scss'],
})
export class CheckoutDeliveryComponent implements OnInit {
  @Input() checkoutForm!: FormGroup;
  deliveryMethods!: IDeliveryMethod[];

  constructor(
    private checkoutService: CheckoutService,
    private basketService: BasketService,
    private helperService: HelperService
  ) {}

  ngOnInit(): void {
    this.getDeliveryMethod();
  }

  getDeliveryMethod() {
    this.checkoutService.getDeliveryMethod().subscribe({
      next: (dm: IDeliveryMethod[]) => {
        this.deliveryMethods = dm;
      },
      error: (error: any) => {
        console.log(error);
      },
      complete: () => {},
    });
  }

  setShippingPrice(deliveryMethod: IDeliveryMethod) {
    this.basketService.setShippingPrice(deliveryMethod);
  }

  scrollToTop() {
    this.helperService.scrollToTop();
  }
}
