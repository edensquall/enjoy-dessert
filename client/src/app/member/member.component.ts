import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { BreadcrumbService } from 'xng-breadcrumb';

@Component({
  selector: 'app-member',
  templateUrl: './member.component.html',
  styleUrls: ['./member.component.scss'],
})
export class MemberComponent implements OnInit {
  breadcrumb$!: Observable<any[]>;

  constructor(private bcService: BreadcrumbService) {}

  ngOnInit(): void {
    this.breadcrumb$ = this.bcService.breadcrumbs$;
  }
}
