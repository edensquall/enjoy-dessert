import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-paging-info',
  templateUrl: './paging-info.component.html',
  styleUrls: ['./paging-info.component.scss'],
})
export class PagingInfoComponent implements OnInit {
  @Input() pageNumber: number = 0;
  @Input() pageSize: number = 0;
  @Input() totalCount: number = 0;

  constructor() {}

  ngOnInit(): void {}
}
