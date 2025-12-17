import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { map, Observable, of, ReplaySubject } from 'rxjs';
import { environment } from 'src/environments/environment';
import { IAddress } from '../shared/models/address';
import { IPassword } from '../shared/models/password';
import { IUser } from '../shared/models/user';
import { IUserInfo } from '../shared/models/userInfo';

@Injectable({
  providedIn: 'root',
})
export class AccountService {
  baseUrl = environment.apiUrl;
  private currentUserSource = new ReplaySubject<IUser | null>(1);
  currentUser$ = this.currentUserSource.asObservable();

  constructor(private http: HttpClient, private router: Router) {}

  loadCurrentUser(token: string | null) {
    if (token === null) {
      this.currentUserSource.next(null);
      return of();
    }

    let headers = new HttpHeaders();
    headers = headers.set('Authorization', `Bearer ${token}`);

    return this.http.get<IUser>(this.baseUrl + 'account', { headers }).pipe(
      map((user: IUser) => {
        if (user) {
          localStorage.setItem('token', user.token);
          this.currentUserSource.next(user);
        }
      })
    );
  }

  login(values: any) {
    return this.http.post<IUser>(this.baseUrl + 'account/login', values, { withCredentials: true }).pipe(
      map((user: IUser) => {
        if (user) {
          localStorage.setItem('token', user.token);
          this.currentUserSource.next(user);
        }
      })
    );
  }

  register(values: any) {
    return this.http
      .post<IUser>(this.baseUrl + 'account/register', values, { withCredentials: true })
      .pipe(
        map((user: IUser) => {
          if (user) {
            localStorage.setItem('token', user.token);
            this.currentUserSource.next(user);
          }
        })
      );
  }

  refreshToken(): Observable<{ token: string }> {
    return this.http.post<{ token: string }>(
      this.baseUrl + 'account/refreshtoken',
      {},
      { withCredentials: true }
    );
  }

  private clearClientState() {
    localStorage.removeItem('token');
    this.currentUserSource.next(null);
    this.router.navigateByUrl('/');
  }

  logout() {
    this.http.post(
      this.baseUrl + 'account/logout',
      {}, { withCredentials: true })
      .subscribe({
      next: () => this.clearClientState(),
      error: () => this.clearClientState()
    });
  }

  checkEmailExists(email: string) {
    return this.http.get(this.baseUrl + 'account/emailexists?email=' + email);
  }
  checkUserNameExists(userName: string) {
    return this.http.get(
      this.baseUrl + 'account/usernameexists?userName=' + userName
    );
  }

  getUserInfo() {
    return this.http.get<IUserInfo>(this.baseUrl + 'account/userinfo');
  }

  updateUserInfo(userInfo: IUserInfo) {
    return this.http.put<IUserInfo>(
      this.baseUrl + 'account/userinfo',
      userInfo
    );
  }

  updatePassword(password: IPassword) {
    return this.http.put(this.baseUrl + 'account/password', password);
  }

  getUserAddress() {
    return this.http.get<IAddress>(this.baseUrl + 'account/address');
  }

  updateUserAddress(address: IAddress) {
    return this.http.put<IAddress>(this.baseUrl + 'account/address', address);
  }
}
