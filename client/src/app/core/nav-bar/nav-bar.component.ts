import { Component, ElementRef, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { BasketService } from 'src/app/basket/basket.service';
import { IBasket } from 'src/app/shared/models/basket';

@Component({
  selector: 'app-nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrls: ['./nav-bar.component.scss'],
})
export class NavBarComponent implements OnInit {
  basket$!: Observable<IBasket | null>;
  isCollapsed = true;

  constructor(private basketService: BasketService, private elem: ElementRef) {}

  ngOnInit(): void {
    this.basket$ = this.basketService.basket$;
  }

  onLinkClick() {
    if (window.innerWidth < 768) {
      this.elem.nativeElement.querySelector('.btn-close').click();
    }
  }
}
