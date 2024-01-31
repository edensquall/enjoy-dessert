import { Component, OnInit } from '@angular/core';
import { AdminAccountService } from './admin-account.service';
import { IAdminUserInfo } from 'src/app/shared/models/adminUserInfo';
import { AdminAccountParams } from 'src/app/shared/models/adminAccountParams';
import { IPagination } from 'src/app/shared/models/pagination';
import { RoleType } from 'src/app/shared/models/roleType';

@Component({
  selector: 'app-admin-account',
  templateUrl: './admin-account.component.html',
  styleUrls: ['./admin-account.component.scss']
})
export class AdminAccountComponent implements OnInit {
  readonly RoleType = RoleType;
  adminUserInfos: IAdminUserInfo[] = [];
  adminAccountParams: AdminAccountParams;
  totalCount: number = 0;

  constructor(private adminAccountService: AdminAccountService) { 
    this.adminAccountParams = this.adminAccountService.adminAccountParams;
  }

  ngOnInit(): void {
    this.getAdminAccounts();
  }

  getAdminAccounts() {
    this.adminAccountService.getAdminAccounts().subscribe({
      next: (response: IPagination<IAdminUserInfo>) => {
        this.adminUserInfos = response.data;
        this.adminAccountParams.pageNumber = response.pageIndex;
        this.adminAccountParams.pageSize = response.pageSize;
        this.totalCount = response.count;
      },
      error: (error: any) => {
        console.log(error);
      },
      complete: () => {},
    });
  }

  deleteAdminAccount(id: string) {
    this.adminAccountService.deleteAdminAccount(id).subscribe({
      next: (response: boolean) => {
        this.getAdminAccounts();
      },
      error: (error: any) => {
        console.log(error);
      },
      complete: () => {},
    });
  }

  onPageChanged(event: any) {
    const params = this.adminAccountService.getAdminAccountParams();
    if (this.adminAccountParams.pageNumber !== event) {
      this.adminAccountParams.pageNumber = event;
      this.adminAccountService.setAdminAccountParams(params);
      this.getAdminAccounts();
    }
  }

}
