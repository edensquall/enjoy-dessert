import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { BreadcrumbService } from 'xng-breadcrumb';
import { AdminProductService } from '../admin-product.service';
import { Location } from '@angular/common';
import { IAdminProduct } from 'src/app/shared/models/adminProduct';
import { IProductType } from 'src/app/shared/models/productType';

@Component({
  selector: 'app-admin-product-edit',
  templateUrl: './admin-product-edit.component.html',
  styleUrls: ['./admin-product-edit.component.scss'],
})
export class AdminProductEditComponent implements OnInit {
  id = this.activateRoute.snapshot.paramMap.get('id');
  functionText!: string;
  editAdminProductForm!: FormGroup;
  adminProduct!: IAdminProduct;
  productTypes: IProductType[] = [];

  constructor(
    private location: Location,
    private activateRoute: ActivatedRoute,
    private bcService: BreadcrumbService,
    private fb: FormBuilder,
    private adminProductService: AdminProductService
  ) {}

  ngOnInit(): void {
    this.functionText = this.id === '0' ? '新增' : '修改';
    this.bcService.set('@editProduct', this.functionText + '產品');

    this.createEditAdminProductForm();
    this.getProductTypes();
    if (this.id !== '0') {
      this.getAdminProduct();
    }
  }

  getProductTypes() {
    this.adminProductService.getProductTypes().subscribe({
      next: (response: IProductType[]) => {
        this.productTypes = [{ id: '', name: '請選擇產品類別' }, ...response];
      },
      error: (error: any) => {
        console.log(error);
      },
      complete: () => {},
    });
  }

  getAdminProduct() {
    this.adminProductService.getAdminProduct(this.id ? +this.id : 0).subscribe({
      next: (response: IAdminProduct) => {
        this.adminProduct = response;
        this.editAdminProductForm?.patchValue(response);
      },
      error: (error: any) => {
        console.log(error);
      },
      complete: () => {},
    });
  }

  createEditAdminProductForm() {
    this.editAdminProductForm = this.fb.group({
      id: this.id,
      name: [null, [Validators.required]],
      description: [null],
      price: [null, [Validators.required]],
      isBestseller: [false],
      isShow: [false],
      isShowByDate: [false],
      startDate: [''],
      endDate: [''],
      productTypeId: ['', [Validators.required]],
      imageFilesIndex: [[]],
      imageFiles: [[]],
    });
  }

  onFileChange(event: any, index: number) {
    if (event.target.files.length > 0) {
      const file = event.target.files[0];
      this.editAdminProductForm
        .get('imageFilesIndex')
        ?.value.splice(index, 0, index);
      this.editAdminProductForm.get('imageFiles')?.value.splice(index, 0, file);
    }
  }

  onAdminProductSubmit() {
    var formData: FormData = new FormData();

    Object.keys(this.editAdminProductForm.value).forEach((key) => {
      if (
        (key === 'startDate' || key === 'endDate') &&
        this.editAdminProductForm.value[key] === null
      ) {
        formData.append(key, '');
      } else if (key === 'imageFiles') {
        const imageFiles = this.editAdminProductForm.value[key];
        for (let i = 0; i < imageFiles.length; i++) {
          formData.append('imageFiles', imageFiles[i]);
        }
      } else if (key === 'imageFilesIndex') {
        const imageFilesIndex = this.editAdminProductForm.value[key];
        for (let i = 0; i < imageFilesIndex.length; i++) {
          formData.append('imageFilesIndex', imageFilesIndex[i]);
        }
      } else {
        formData.append(key, this.editAdminProductForm.value[key]);
      }
    });

    if (this.id === '0') {
      this.adminProductService.createAdminProduct(formData).subscribe({
        next: (adminProduct: IAdminProduct) => {
          this.id = adminProduct.id.toString();
          this.location.go('/admin/product/' + this.id);
          this.ngOnInit();
        },
        error: (error: any) => {
          console.log(error);
        },
        complete: () => {},
      });
    } else {
      this.adminProductService.updateAdminProduct(formData).subscribe({
        next: (adminProduct: IAdminProduct) => {
          this.ngOnInit();
        },
        error: (error: any) => {
          console.log(error);
        },
        complete: () => {},
      });
    }
  }
}
