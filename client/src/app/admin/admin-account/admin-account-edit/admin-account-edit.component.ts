import { Component, OnInit } from '@angular/core';
import {
  AbstractControlOptions,
  FormBuilder,
  FormGroup,
  Validators,
} from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { PasswordValidator } from 'src/app/shared/validators/passwordValidator';
import { BreadcrumbService } from 'xng-breadcrumb';
import { AdminAccountService } from '../admin-account.service';
import { IAdminUserInfo } from 'src/app/shared/models/adminUserInfo';
import { ConfirmValidator } from 'src/app/shared/validators/confirmValidator';
import { Location } from '@angular/common';

@Component({
  selector: 'app-admin-account-edit',
  templateUrl: './admin-account-edit.component.html',
  styleUrls: ['./admin-account-edit.component.scss'],
})
export class AdminAccountEditComponent implements OnInit {
  editAdminAccountForm!: FormGroup;
  adminUserInfo!: IAdminUserInfo;
  functionText!: string;
  id = this.activateRoute.snapshot.paramMap.get('id');
  gradeType = [
    { id: '', name: '請選擇會員等級' },
    { id: 'RegularMember', name: '一般會員' },
    { id: 'GoldMember', name: '黃金會員' },
    { id: 'DiamondMember', name: '鑽石會員' },
  ];

  constructor(
    private location: Location,
    private activateRoute: ActivatedRoute,
    private bcService: BreadcrumbService,
    private fb: FormBuilder,
    private adminAccountService: AdminAccountService
  ) {}

  ngOnInit(): void {
    this.functionText =
      this.id === '00000000-0000-0000-0000-000000000000' ? '新增' : '修改';
    this.bcService.set('@editAccount', this.functionText + '會員');

    this.createEditAdminAccountForm();
    this.getAdminAccount();
  }

  getAdminAccount() {
    if (this.id !== '00000000-0000-0000-0000-000000000000') {
      this.adminAccountService
        .getAdminAccount(this.id ? this.id : '')
        .subscribe({
          next: (response: IAdminUserInfo) => {
            this.adminUserInfo = response;
            this.editAdminAccountForm?.patchValue(response);
          },
          error: (error: any) => {
            console.log(error);
          },
          complete: () => {},
        });
    }
  }

  createEditAdminAccountForm() {
    this.editAdminAccountForm = this.fb.group(
      {
        userName: ['', [Validators.required]],
        password: [
          '',
          this.id === '00000000-0000-0000-0000-000000000000'
            ? [Validators.required, PasswordValidator()]
            : [PasswordValidator()],
        ],
        passwordConfirm: [''],
        displayName: ['', [Validators.required]],
        phoneNumber: ['', [Validators.required]],
        email: ['', [Validators.required]],
        grade: ['', [Validators.required]],
        isAdmin: [false, [Validators.required]],
      },
      {
        validator: [ConfirmValidator('password', 'passwordConfirm')],
      } as AbstractControlOptions
    );
  }

  onAdminAccountSubmit() {
    if (this.id === '00000000-0000-0000-0000-000000000000') {
      this.adminAccountService
        .createAdminAccount(this.editAdminAccountForm?.value)
        .subscribe({
          next: (adminUserInfo: IAdminUserInfo) => {
            this.id = adminUserInfo.id;
            this.location.go('/admin/account/' + this.id);
            this.ngOnInit();
          },
          error: (error: any) => {
            console.log(error);
          },
          complete: () => {},
        });
    } else {
      this.adminAccountService
        .updateAdminAccount(this.editAdminAccountForm?.value)
        .subscribe({
          next: (adminUserInfo: IAdminUserInfo) => {
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
