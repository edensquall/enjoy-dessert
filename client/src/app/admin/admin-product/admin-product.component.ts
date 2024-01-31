import { Component, OnInit } from '@angular/core';
import { AdminProductParams } from 'src/app/shared/models/adminProductParams';
import { IAdminProduct } from 'src/app/shared/models/adminProduct';
import { AdminProductService } from './admin-product.service';
import { IPagination } from 'src/app/shared/models/pagination';

@Component({
  selector: 'app-admin-product',
  templateUrl: './admin-product.component.html',
  styleUrls: ['./admin-product.component.scss'],
})
export class AdminProductComponent implements OnInit {
  adminProducts: IAdminProduct[] = [];
  adminProductParams: AdminProductParams;
  totalCount: number = 0;

  constructor(private adminProductService: AdminProductService) {
    this.adminProductParams = this.adminProductService.adminProductParams;
  }

  ngOnInit(): void {
    this.getAdminProducts();
  }

  getAdminProducts() {
    this.adminProductService.getAdminProducts().subscribe({
      next: (response: IPagination<IAdminProduct>) => {
        this.adminProducts = response.data;
        this.adminProductParams.pageNumber = response.pageIndex;
        this.adminProductParams.pageSize = response.pageSize;
        this.totalCount = response.count;
      },
      error: (error: any) => {
        console.log(error);
      },
      complete: () => {},
    });
  }

  deleteAdminProduct(id: string | number) {
    this.adminProductService.deleteAdminProduct(id).subscribe({
      next: (response: boolean) => {
        this.getAdminProducts();
      },
      error: (error: any) => {
        console.log(error);
      },
      complete: () => {},
    });
  }

  onPageChanged(event: any) {
    const params = this.adminProductService.getAdminProductParams();
    if (this.adminProductParams.pageNumber !== event) {
      this.adminProductParams.pageNumber = event;
      this.adminProductService.setAdminProductParams(params);
      this.getAdminProducts();
    }
  }
}
