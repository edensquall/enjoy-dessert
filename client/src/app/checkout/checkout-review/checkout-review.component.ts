import { CdkStepper } from '@angular/cdk/stepper';
import { Component, Input, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { BasketService } from 'src/app/basket/basket.service';
import { IBasket, IBasketTotals } from 'src/app/shared/models/basket';
import { HelperService } from 'src/app/shared/services/helper.service';

@Component({
  selector: '[app-checkout-review]',
  templateUrl: './checkout-review.component.html',
  styleUrls: ['./checkout-review.component.scss'],
})
export class CheckoutReviewComponent implements OnInit {
  @Input() appStepper!: CdkStepper;
  basket$!: Observable<IBasket | null>;
  basketTotals$!: Observable<IBasketTotals | null>;

  constructor(private basketService: BasketService, private helperService: HelperService) {}

  ngOnInit(): void {
    this.basket$ = this.basketService.basket$;
    this.basketTotals$ = this.basketService.basketTotal$;
  }

  createPaymentIntent() {
    return this.basketService.createPaymentIntent().subscribe({
      next: (response: any) => {
        this.scrollToTop();
        this.appStepper.next();
      },
      error: (error: any) => {
        console.log(error);
      },
      complete: () => {},
    });
  }

  scrollToTop() {
    this.helperService.scrollToTop();
  }
}
