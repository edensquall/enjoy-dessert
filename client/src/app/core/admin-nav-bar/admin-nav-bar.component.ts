import { Component, ElementRef, OnInit } from '@angular/core';

@Component({
  selector: 'app-admin-nav-bar',
  templateUrl: './admin-nav-bar.component.html',
  styleUrls: ['./admin-nav-bar.component.scss']
})
export class AdminNavBarComponent implements OnInit {
  isCollapsed = true;

  constructor(private elem: ElementRef) { }

  ngOnInit(): void {
  }

  onLinkClick() {
    if (window.innerWidth < 768) {
      this.elem.nativeElement.querySelector('.btn-close').click();
    }
  }

}
