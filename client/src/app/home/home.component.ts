import { Component, OnInit } from '@angular/core';
import { IProduct } from '../shared/models/product';
import { ISlide } from '../shared/models/slide';
import { ShopService } from '../shop/shop.service';
import { HomeService } from './home.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
})
export class HomeComponent implements OnInit {
  bestsellers: IProduct[] = [];
  slides: ISlide[] = [];

  constructor(private homeService: HomeService, private shopService: ShopService) {}

  ngOnInit(): void {
    this.getBestsellerProduct(true);
    this.getSlide(true);
  }

  getBestsellerProduct(useCache = false) {
    this.shopService.getBestsellerProducts(useCache).subscribe({
      next: (response: IProduct[]) => {
        this.bestsellers = response;
      },
      error: (error: any) => {
        console.log(error);
      },
      complete: () => {},
    });
  }

  getSlide(useCache = false) {
    this.homeService.getSlides(useCache).subscribe({
      next: (response: ISlide[]) => {
        this.slides = response;
      },
      error: (error: any) => {
        console.log(error);
      },
      complete: () => {},
    });
  }
}
