import { Component, OnInit } from '@angular/core';
import { IPagination } from '../shared/models/pagination';
import { IProduct } from '../shared/models/product';
import { IProductType } from '../shared/models/productType';
import { ShopParams } from '../shared/models/shopParams';
import { ShopService } from './shop.service';

@Component({
  selector: 'app-shop',
  templateUrl: './shop.component.html',
  styleUrls: ['./shop.component.scss'],
})
export class ShopComponent implements OnInit {
  products: IProduct[] = [];
  productTypes: IProductType[] = [];
  shopParams: ShopParams;
  totalCount: number = 0;

  constructor(private shopService: ShopService) {
    this.shopParams = shopService.getShopParams();
  }

  ngOnInit(): void {
    this.getProducts(true);
    this.getProductTypes();
  }

  getProducts(useCache = false) {
    this.shopService.getProducts(useCache).subscribe({
      next: (response: IPagination<IProduct>) => {
        this.products = response.data;
        this.totalCount = response.count;
      },
      error: (error: any) => {
        console.log(error);
      },
      complete: () => {},
    });
  }

  getProductTypes() {
    this.shopService.getProductTypes().subscribe({
      next: (response: IProductType[]) => {
        this.productTypes = [{ id: 0, name: '全部' }, ...response];
      },
      error: (error: any) => {
        console.log(error);
      },
      complete: () => {},
    });
  }

  onTypeSelected(typeId: number) {
    const params = this.shopService.getShopParams();
    params.typeId = typeId;
    params.pageNumber = 1;
    this.shopService.setShopParams(params);
    this.getProducts(true);
  }

  onPageChanged(event: any) {
    const params = this.shopService.getShopParams();
    if (params.pageNumber !== event) {
      params.pageNumber = event;
      this.shopService.setShopParams(params);
      this.getProducts(true);
    }
  }
}
