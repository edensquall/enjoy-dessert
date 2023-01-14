import { CdkStepper } from '@angular/cdk/stepper';
import { Component, Input, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { BasketService } from 'src/app/basket/basket.service';
import { IBasket, IBasketTotals } from 'src/app/shared/models/basket';

@Component({
  selector: '[app-checkout-review]',
  templateUrl: './checkout-review.component.html',
  styleUrls: ['./checkout-review.component.scss'],
})
export class CheckoutReviewComponent implements OnInit {
  @Input() appStepper!: CdkStepper;
  basket$!: Observable<IBasket | null>;
  basketTotals$!: Observable<IBasketTotals | null>;

  constructor(private basketService: BasketService) {}

  ngOnInit(): void {
    this.basket$ = this.basketService.basket$;
    this.basketTotals$ = this.basketService.basketTotal$;
  }

  createPaymentIntent() {
    return this.basketService.createPaymentIntent().subscribe({
      next: (response: any) => {
        this.appStepper.next();
      },
      error: (error: any) => {
        console.log(error);
      },
      complete: () => {},
    });
  }
}
