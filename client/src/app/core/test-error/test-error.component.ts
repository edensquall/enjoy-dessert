import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-test-error',
  templateUrl: './test-error.component.html',
  styleUrls: ['./test-error.component.scss'],
})
export class TestErrorComponent implements OnInit {
  baseUrl = environment.apiUrl;
  validatoinErrors: any;

  constructor(private http: HttpClient) {}

  ngOnInit(): void {}

  get404Error() {
    this.http.get(this.baseUrl + 'products/42').subscribe({
      next: (response) => {
        console.log(response);
      },
      error: (error: any) => {
        console.log(error);
      },
      complete: () => {},
    });
  }

  get500Error() {
    this.http.get(this.baseUrl + 'buggy/servererror').subscribe({
      next: (response) => {
        console.log(response);
      },
      error: (error: any) => {
        console.log(error);
      },
      complete: () => {},
    });
  }

  get400Error() {
    this.http.get(this.baseUrl + 'buggy/badrequest').subscribe({
      next: (response) => {
        console.log(response);
      },
      error: (error: any) => {
        console.log(error);
      },
      complete: () => {},
    });
  }

  get400ValidationError() {
    this.http.get(this.baseUrl + 'products/fortytwo').subscribe({
      next: (response) => {
        console.log(response);
      },
      error: (error: any) => {
        console.log(error);
        this.validatoinErrors = error.errors;
      },
      complete: () => {},
    });
  }
}
