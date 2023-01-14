import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, of } from 'rxjs';
import { environment } from 'src/environments/environment';
import { IPagination, Pagination } from '../shared/models/pagination';
import { IProduct } from '../shared/models/product';
import { IProductType } from '../shared/models/productType';
import { ShopParams } from '../shared/models/shopParams';

@Injectable({
  providedIn: 'root',
})
export class ShopService {
  baseUrl = environment.apiUrl;
  productTypes: IProductType[] = [];
  pagination = new Pagination<IProduct>();
  shopParams = new ShopParams();
  productCache = new Map<string, Pagination<IProduct>>();
  bestsellerCache!: IProduct[];

  constructor(private http: HttpClient) {}

  getProducts(useCache: boolean) {
    if (useCache === false) {
      this.productCache = new Map<string, Pagination<IProduct>>();
    }

    if (this.productCache.size > 0 && useCache === true) {
      if (this.productCache.has(JSON.stringify(this.shopParams))) {
        this.pagination =
          this.productCache.get(JSON.stringify(this.shopParams)) ||
          new Pagination<IProduct>();
        return of(this.pagination);
      }
    }

    let params = new HttpParams();

    if (this.shopParams.typeId !== 0) {
      params = params.append('typeId', this.shopParams.typeId);
    }

    params = params.append('pageIndex', this.shopParams.pageNumber);
    params = params.append('pageSize', this.shopParams.pageSize);

    return this.http
      .get<IPagination<IProduct>>(this.baseUrl + 'products', {
        params: params,
      })
      .pipe(
        map((response) => {
          this.productCache.set(JSON.stringify(this.shopParams), response);
          this.pagination = response;
          return response;
        })
      );
  }

  getProduct(id: number) {
    let product: IProduct | undefined;

    product = this.bestsellerCache?.find(p => p.id === id);
    if (product) {
      return of(product);
    }

    for (let [, products] of this.productCache) {
      product = products.data.find(p => p.id === id);
      if (product) {
        return of(product);
      }
    }

    return this.http.get<IProduct>(this.baseUrl + 'products/' + id);
  }

  getProductTypes() {
    if (this.productTypes.length > 0) {
      return of(this.productTypes);
    }
    return this.http.get<IProductType[]>(this.baseUrl + 'products/types').pipe(
      map((response) => {
        this.productTypes = response;
        return response;
      })
    );
  }

  getBestsellerProducts(useCache: boolean) {
    if (useCache === false) {
      this.bestsellerCache = [];
    }

    if (this.bestsellerCache && useCache === true) {
      return of(this.bestsellerCache);
    }
    return this.http.get<IProduct[]>(this.baseUrl + 'products/bestseller').pipe(
      map((response) => {
        this.bestsellerCache = response;
        return response;
      })
    );
  }

  setShopParams(params: ShopParams) {
    this.shopParams = params;
  }

  getShopParams() {
    return this.shopParams;
  }
}
