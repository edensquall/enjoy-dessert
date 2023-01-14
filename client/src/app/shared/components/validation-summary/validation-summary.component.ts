import { Component, Input, OnInit } from '@angular/core';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-validation-summary',
  templateUrl: './validation-summary.component.html',
  styleUrls: ['./validation-summary.component.scss'],
})
export class ValidationSummaryComponent implements OnInit {
  @Input() errors$!: Observable<string[]>;

  constructor() {}

  ngOnInit(): void {}
}
