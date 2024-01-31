import { Component, OnInit } from '@angular/core';
import { IAdminNews } from 'src/app/shared/models/adminNews';
import { AdminNewsParams } from 'src/app/shared/models/adminNewsParams';
import { AdminNewsService } from './admin-news.service';
import { IPagination } from 'src/app/shared/models/pagination';

@Component({
  selector: 'app-admin-news',
  templateUrl: './admin-news.component.html',
  styleUrls: ['./admin-news.component.scss'],
})
export class AdminNewsComponent implements OnInit {
  adminNewsAll: IAdminNews[] = [];
  adminNewsParams: AdminNewsParams;
  totalCount: number = 0;

  constructor(private adminNewsService: AdminNewsService) {
    this.adminNewsParams = this.adminNewsService.adminNewsParams;
  }

  ngOnInit(): void {
    this.getAdminNewsAll();
  }

  getAdminNewsAll() {
    this.adminNewsService.getAdminNewsAll().subscribe({
      next: (response: IPagination<IAdminNews>) => {
        this.adminNewsAll = response.data;
        this.adminNewsParams.pageNumber = response.pageIndex;
        this.adminNewsParams.pageSize = response.pageSize;
        this.totalCount = response.count;
      },
      error: (error: any) => {
        console.log(error);
      },
      complete: () => {},
    });
  }

  deleteAdminNews(id: string | number) {
    this.adminNewsService.deleteAdminNews(id).subscribe({
      next: (response: boolean) => {
        this.getAdminNewsAll();
      },
      error: (error: any) => {
        console.log(error);
      },
      complete: () => {},
    });
  }

  onPageChanged(event: any) {
    const params = this.adminNewsService.getAdminNewsParams();
    if (this.adminNewsParams.pageNumber !== event) {
      this.adminNewsParams.pageNumber = event;
      this.adminNewsService.setAdminNewsParams(params);
      this.getAdminNewsAll();
    }
  }
}
