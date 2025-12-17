import {
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest,
} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { NavigationExtras, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { BehaviorSubject, Observable, throwError } from 'rxjs';
import { catchError, filter, switchMap, take } from 'rxjs/operators';
import { AccountService } from 'src/app/account/account.service';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
  private isRefreshing = false;
  private refreshTokenSubject = new BehaviorSubject<string | null>(null);

  constructor(
    private router: Router,
    private toaster: ToastrService,
    private accountService: AccountService
  ) {}

  intercept(
    req: HttpRequest<unknown>,
    next: HttpHandler
  ): Observable<HttpEvent<unknown>> {
    return next.handle(req).pipe(
      catchError((error) => {
        if (error) {
          if (error.status === 400) {
            if (error.error.errors) {
              throw error.error;
            } else {
              this.toaster.error(error.error.message, error.error.statusCode);
            }
          }
          if (error.status === 401) {
            return this.handle401Error(req, next, error);
          }
          if (error.status === 404) {
            this.router.navigateByUrl('/not-found');
          }
          if (error.status === 500) {
            const navigationExtras: NavigationExtras = {
              state: { error: error.error },
            };
            this.router.navigateByUrl('/server-error', navigationExtras);
          }
        }
        return throwError(() => error);
      })
    );
  }

  private handle401Error(
    req: HttpRequest<any>,
    next: HttpHandler,
    error: any
  ): Observable<HttpEvent<any>> {
    if (!this.isRefreshing) {
      this.isRefreshing = true;
      this.refreshTokenSubject.next(null);

      return this.accountService.refreshToken().pipe(
        switchMap((res) => {
          this.isRefreshing = false;

          localStorage.setItem('token', res.token);

          this.refreshTokenSubject.next(res.token);

          return next.handle(req);
        }),
        catchError((err) => {
          this.isRefreshing = false;
          this.accountService.logout();
          return throwError(() => err);
        })
      );
    }

    return this.refreshTokenSubject.pipe(
      filter((token) => token !== null),
      take(1),
      switchMap(() => next.handle(req))
    );
  }
}
