import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Observable } from 'rxjs';
import { AccountService } from '../account/account.service';
import { BasketService } from '../basket/basket.service';
import { IAddress } from '../shared/models/address';
import { IBasketTotals } from '../shared/models/basket';

@Component({
  selector: 'app-checkout',
  templateUrl: './checkout.component.html',
  styleUrls: ['./checkout.component.scss'],
})
export class CheckoutComponent implements OnInit {
  basketTotals$!: Observable<IBasketTotals | null>;
  checkoutForm!: FormGroup;

  constructor(
    private fb: FormBuilder,
    private accountService: AccountService,
    private basketService: BasketService
  ) {}

  ngOnInit(): void {
    this.createCheckoutForm();
    this.getAddressFormValues();
    this.getDeliveryMethodValue();
    this.basketTotals$ = this.basketService.basketTotal$;
  }

  createCheckoutForm() {
    this.checkoutForm = this.fb.group({
      deliveryForm: this.fb.group({
        deliveryMethod: [null, Validators.required],
      }),
      addressForm: this.fb.group({
        firstName: [null, Validators.required],
        lastName: [null, Validators.required],
        phoneNumber: [null, Validators.required],
        zipCode: [null, Validators.required],
        county: [null, Validators.required],
        city: [null, Validators.required],
        street: [null, Validators.required],
      }),
      paymentForm: this.fb.group({
        nameOnCard: [null, Validators.required],
      }),
    });
  }

  getAddressFormValues() {
    this.accountService.getUserAddress().subscribe({
      next: (address: IAddress) => {
        this.checkoutForm.get('addressForm')?.patchValue(address);
      },
      error: (error: any) => {
        console.log(error);
      },
      complete: () => {},
    });
  }

  getDeliveryMethodValue() {
    const basket = this.basketService.getCurrentBasketValue();
    if (basket.deliveryMethodId) {
      this.checkoutForm?.get('deliveryForm')?.get('deliveryMethod')?.patchValue(basket.deliveryMethodId.toString());
    }
  }
}
