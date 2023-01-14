import { Component, OnInit } from '@angular/core';
import {
  AbstractControlOptions,
  FormBuilder,
  FormGroup,
  Validators,
} from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { Observable, of } from 'rxjs';
import { AccountService } from 'src/app/account/account.service';
import { IPassword } from 'src/app/shared/models/password';
import { IUser } from 'src/app/shared/models/user';
import { IUserInfo } from 'src/app/shared/models/userInfo';
import { ConfirmValidator } from 'src/app/shared/validators/confirmValidator';
import { emailNotTakenValidator } from 'src/app/shared/validators/emailNotTakenValidator';
import { emailValidator } from 'src/app/shared/validators/emailValidator';
import { PasswordValidator } from 'src/app/shared/validators/passwordValidator';
import { phoneValidator } from 'src/app/shared/validators/phoneValidator';

@Component({
  selector: '[app-personal-info] .col-12 .col-sm-8 .col-md-9',
  templateUrl: './personal-info.component.html',
  styleUrls: ['./personal-info.component.scss'],
})
export class PersonalInfoComponent implements OnInit {
  basicInfoForm!: FormGroup;
  newPasswordForm!: FormGroup;
  newPasswordFormErrors$!: Observable<string[]>;

  constructor(
    private fb: FormBuilder,
    private accountService: AccountService,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.createBasicInfoForm();
    this.createNewPasswordForm();
    this.getUserInfoFormValues();
  }

  getUserInfoFormValues() {
    this.accountService.getUserInfo().subscribe({
      next: (userInfo: IUserInfo) => {
        this.basicInfoForm?.patchValue(userInfo);
        this.setEmailAsyncValidator(userInfo.email);
      },
      error: (error: any) => {
        console.log(error);
      },
      complete: () => {},
    });
  }

  createBasicInfoForm() {
    this.basicInfoForm = this.fb.group({
      displayName: [null, [Validators.required]],
      phoneNumber: [null, [Validators.required, phoneValidator()]],
      email: [null, [Validators.required, emailValidator()]],
    });
  }

  setEmailAsyncValidator(currentUserEmail: string) {
    const email = this.basicInfoForm.get('email');
    email?.setAsyncValidators([
      emailNotTakenValidator(this.accountService, currentUserEmail),
    ]);
    email?.updateValueAndValidity();
  }

  createNewPasswordForm() {
    this.newPasswordForm = this.fb.group(
      {
        password: [null, [Validators.required]],
        newPassword: [null, [Validators.required, PasswordValidator()]],
        newPasswordConfirm: [null, [Validators.required]],
      },
      {
        validator: [ConfirmValidator('newPassword', 'newPasswordConfirm')],
      } as AbstractControlOptions
    );
  }

  onBasicInfoSubmit() {
    this.accountService.updateUserInfo(this.basicInfoForm?.value).subscribe({
      next: (userInfo: IUserInfo) => {
        this.toastr.success('個人基本資料已儲存');
        this.basicInfoForm?.reset(userInfo);
      },
      error: (error: any) => {
        console.log(error);
      },
      complete: () => {},
    });
  }

  onNewPasswordSubmit() {
    this.accountService.updatePassword(this.newPasswordForm?.value).subscribe({
      next: () => {
        this.toastr.success('密碼已儲存');
        this.newPasswordFormErrors$ = of();
        this.newPasswordForm?.reset();
      },
      error: (error: any) => {
        console.log(error);
        this.newPasswordFormErrors$ = of(error.errors);
        this.newPasswordForm?.reset();
      },
      complete: () => {},
    });
  }
}
