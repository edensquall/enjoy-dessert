import { Component, OnInit } from '@angular/core';
import { BasketService } from './basket/basket.service';
import { AccountService } from './account/account.service';
import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
import { Observable, map, of } from 'rxjs';
import { ChatService } from './chat/chat.service';

@Component({
  selector: '[app-root]',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent implements OnInit {
  title = 'Welcome to Enjoy Dessert';
  isBackstage!: boolean;

  constructor(
    private basketService: BasketService,
    private accountService: AccountService,
    private chatService: ChatService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadBasket();
    this.loadCurrentUser();
    this.checkIsBackstage();
  }

  checkIsBackstage() {
    this.router.events.subscribe((event) => {
      if (event instanceof NavigationEnd) {
        this.isBackstage = event.url.indexOf('admin') > -1;
      }
    });
  }

  loadCurrentUser() {
    const token = localStorage.getItem('token');
    this.accountService.loadCurrentUser(token).subscribe({
      next: () => {
        console.log('loaded user');
      },
      error: (error: any) => {
        console.log(error);
      },
      complete: () => {},
    });
  }

  loadBasket() {
    const basketId = localStorage.getItem('basket_id');
    if (basketId) {
      this.basketService.getBasket(basketId).subscribe({
        next: () => {
          console.log('initialised basket');
        },
        error: (error: any) => {
          console.log(error);
        },
        complete: () => {},
      });
    }
  }
}
