import { Component, OnInit } from '@angular/core';
import { AccountService } from 'src/app/account/account.service';

@Component({
  selector: '[app-member-menu]',
  templateUrl: './member-menu.component.html',
  styleUrls: ['./member-menu.component.scss'],
})
export class MemberMenuComponent implements OnInit {
  constructor(private accountService: AccountService) {}

  ngOnInit(): void {}
  
  logout() {
    this.accountService.logout();
  }
}
