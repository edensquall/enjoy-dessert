import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { IBasketItem } from '../../models/basket';
import { IOrderItem } from '../../models/order';

@Component({
  selector: 'app-basket-summary',
  templateUrl: './basket-summary.component.html',
  styleUrls: ['./basket-summary.component.scss'],
})
export class BasketSummaryComponent implements OnInit {
  @Output() decrement: EventEmitter<IBasketItem> =
    new EventEmitter<IBasketItem>();
  @Output() increment: EventEmitter<IBasketItem> =
    new EventEmitter<IBasketItem>();
  @Output() remove: EventEmitter<IBasketItem> = new EventEmitter<IBasketItem>();
  @Input() isBasket = true;
  @Input() items: (IBasketItem | IOrderItem)[] = [];
  @Input() isOrder = false;

  constructor() {}

  ngOnInit(): void {}

  decrementItemQuantity(item: any) {
    this.decrement.emit(item);
  }

  incrementItemQuantity(item: any) {
    this.increment.emit(item);
  }

  removeBasketItem(item: any) {
    this.remove.emit(item);
  }
}
