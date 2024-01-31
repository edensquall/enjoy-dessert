import { Component, OnInit, TemplateRef } from '@angular/core';
import { AdminProductTypeService } from './admin-product-type.service';
import { IProductType } from 'src/app/shared/models/productType';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-admin-product-type',
  templateUrl: './admin-product-type.component.html',
  styleUrls: ['./admin-product-type.component.scss'],
})
export class AdminProductTypeComponent implements OnInit {
  productType!: IProductType;
  productTypes: IProductType[] = [];
  modalRef?: BsModalRef;
  editProductTypeForm!: FormGroup;
  functionText!: string;

  constructor(
    private adminProductTypeService: AdminProductTypeService,
    private modalService: BsModalService,
    private fb: FormBuilder
  ) {}

  ngOnInit(): void {
    this.createEditProductTypeForm();
    this.getProductTypes();
  }

  createEditProductTypeForm() {
    this.editProductTypeForm = this.fb.group({
      id: [null, [Validators.required]],
      name: [null, [Validators.required]],
    });
  }

  getProductTypes() {
    this.adminProductTypeService.getProductTypes().subscribe({
      next: (response: IProductType[]) => {
        this.productTypes = response;
      },
      error: (error: any) => {
        console.log(error);
      },
      complete: () => {},
    });
  }

  deleteProductType(id: number) {
    this.adminProductTypeService.deleteProductType(id).subscribe({
      next: (response: boolean) => {
        this.getProductTypes();
      },
      error: (error: any) => {
        console.log(error);
      },
      complete: () => {},
    });
  }

  onProductTypeSubmit() {
    if (this.productType.id == 0) {
      this.adminProductTypeService
        .createProductType(this.editProductTypeForm?.value)
        .subscribe({
          next: (productType: IProductType) => {
            this.editProductTypeForm?.reset(productType);
            this.modalRef?.hide();
            this.getProductTypes();
          },
          error: (error: any) => {
            console.log(error);
          },
          complete: () => {},
        });
    } else {
      this.adminProductTypeService
        .updateProductType(this.editProductTypeForm?.value)
        .subscribe({
          next: (productType: IProductType) => {
            this.editProductTypeForm?.reset(productType);
            this.modalRef?.hide();
            this.getProductTypes();
          },
          error: (error: any) => {
            console.log(error);
          },
          complete: () => {},
        });
    }
  }

  openModal(template: TemplateRef<any>, data: IProductType) {
    this.functionText = data.id === 0 ? '新增' : '修改';
    this.productType = data;
    this.editProductTypeForm?.patchValue(this.productType);
    this.modalRef = this.modalService.show(template, this.productType);
  }
}
