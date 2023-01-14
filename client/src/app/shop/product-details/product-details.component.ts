import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BasketService } from 'src/app/basket/basket.service';
import { IProduct } from 'src/app/shared/models/product';
import { BreadcrumbService } from 'xng-breadcrumb';
import { ShopService } from '../shop.service';

@Component({
  selector: 'app-product-details',
  templateUrl: './product-details.component.html',
  styleUrls: ['./product-details.component.scss'],
})
export class ProductDetailsComponent implements OnInit {
  product!: IProduct;
  id = this.activateRoute.snapshot.paramMap.get('id');
  quantity: number = 1;

  constructor(
    private shopService: ShopService,
    private activateRoute: ActivatedRoute,
    private bcService: BreadcrumbService,
    private basketService: BasketService
  ) {
    this.bcService.set('@proudctDetails', ' ');
  }

  ngOnInit(): void {
    this.getProduct();
  }

  addItemToBasket() {
    this.basketService.addItemToBasket(this.product, this.quantity);
  }

  incrementQuantity() {
    this.quantity++;
  }

  decrementQuantity() {
    if (this.quantity > 1) {
      this.quantity--;
    }
  }

  getProduct() {
    this.shopService.getProduct(this.id ? +this.id : 0).subscribe({
      next: (response: IProduct) => {
        this.product = response;
        this.bcService.set('@proudctDetails', response.name);
      },
      error: (error: any) => {
        console.log(error);
      },
      complete: () => {},
    });
  }
}
