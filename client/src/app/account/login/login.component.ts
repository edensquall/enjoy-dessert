import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable, of } from 'rxjs';
import { AccountService } from '../account.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginComponent implements OnInit {
  loginForm!: FormGroup;
  returnUrl!: string;
  loginFormErrors$!: Observable<string[]>;

  constructor(
    private accountService: AccountService,
    private router: Router,
    private activatedRoute: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.returnUrl =
      this.activatedRoute.snapshot.queryParams['returnUrl'] ||
      '/member/member-center';
    this.createLoginForm();
  }

  createLoginForm() {
    this.loginForm = new FormGroup({
      userName: new FormControl('', Validators.required),
      password: new FormControl('', Validators.required),
    });
  }

  onSubmit() {
    this.accountService.login(this.loginForm.value).subscribe({
      next: () => {
        this.router.navigateByUrl(this.returnUrl);
      },
      error: (error: any) => {
        console.log(error);
        this.loginFormErrors$ = of(error.errors);
        this.loginForm?.reset();
      },
      complete: () => {},
    });
  }
}
