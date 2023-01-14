import { Component, Input, OnInit } from '@angular/core';
import { INews } from 'src/app/shared/models/news';

@Component({
  selector: 'app-news-item',
  templateUrl: './news-item.component.html',
  styleUrls: ['./news-item.component.scss'],
})
export class NewsItemComponent implements OnInit {
  @Input() news!: INews;

  constructor() {}

  ngOnInit(): void {}
}
