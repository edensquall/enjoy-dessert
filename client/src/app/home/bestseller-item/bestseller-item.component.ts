import { Component, Input, OnInit } from '@angular/core';
import { IProduct } from 'src/app/shared/models/product';

@Component({
  selector: 'app-bestseller-item',
  templateUrl: './bestseller-item.component.html',
  styleUrls: ['./bestseller-item.component.scss']
})
export class BestsellerItemComponent implements OnInit {
  @Input() bestseller!: IProduct;
  
  constructor() { }

  ngOnInit(): void {
  }

}
