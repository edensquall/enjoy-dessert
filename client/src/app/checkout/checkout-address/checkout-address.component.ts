import { Component, Input, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from 'src/app/account/account.service';
import { IAddress } from 'src/app/shared/models/address';
import { HelperService } from 'src/app/shared/services/helper.service';

@Component({
  selector: '[app-checkout-address]',
  templateUrl: './checkout-address.component.html',
  styleUrls: ['./checkout-address.component.scss'],
})
export class CheckoutAddressComponent implements OnInit {
  @Input() checkoutForm!: FormGroup;

  constructor(
    private accountService: AccountService,
    private toastr: ToastrService,
    private helperService: HelperService
  ) {}

  ngOnInit(): void {}

  saveUserAddress() {
    this.accountService
      .updateUserAddress(this.checkoutForm.get('addressForm')?.value)
      .subscribe({
        next: (address: IAddress) => {
          this.toastr.success('Address saved');
          this.checkoutForm.get('addressForm')?.reset(address);
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
