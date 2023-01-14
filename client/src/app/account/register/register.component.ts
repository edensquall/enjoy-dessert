import { Component, OnInit } from '@angular/core';
import {
  AbstractControlOptions, FormBuilder,
  FormGroup,
  Validators
} from '@angular/forms';
import { Router } from '@angular/router';
import { ConfirmValidator } from 'src/app/shared/validators/confirmValidator';
import { emailNotTakenValidator } from 'src/app/shared/validators/emailNotTakenValidator';
import { emailValidator } from 'src/app/shared/validators/emailValidator';
import { PasswordValidator } from 'src/app/shared/validators/passwordValidator';
import { phoneValidator } from 'src/app/shared/validators/phoneValidator';
import { userNameNotTakenValidator } from 'src/app/shared/validators/userNameNotTakenValidator';
import { AccountService } from '../account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss'],
})
export class RegisterComponent implements OnInit {
  registerForm!: FormGroup;
  errors: string[] = [];

  constructor(
    private fb: FormBuilder,
    private accountService: AccountService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.createRegisterForm();
  }

  createRegisterForm() {
    this.registerForm = this.fb.group(
      {
        userName: [
          null,
          [Validators.required],
          [userNameNotTakenValidator(this.accountService)],
        ],
        password: [null, [Validators.required, PasswordValidator()]],
        passwordConfirm: [null, [Validators.required]],
        displayName: [null, [Validators.required]],
        phoneNumber: [null, [Validators.required, phoneValidator()]],
        email: [
          null,
          [Validators.required, emailValidator()],
          [emailNotTakenValidator(this.accountService)],
        ],
      },
      {
        validator: [ConfirmValidator('password', 'passwordConfirm')],
      } as AbstractControlOptions
    );
  }

  onSubmit() {
    this.accountService.register(this.registerForm.value).subscribe({
      next: () => {
        this.router.navigateByUrl('/member/member-center');
      },
      error: (error: any) => {
        console.log(error);
        this.errors = error.errors;
      },
      complete: () => {},
    });
  }
}
