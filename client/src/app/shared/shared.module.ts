import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { PagingInfoComponent } from './components/paging-info/paging-info.component';
import { PagerComponent } from './components/pager/pager.component';
import { OrderTotalsComponent } from './components/order-totals/order-totals.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TextInputComponent } from './components/text-input/text-input.component';
import { CdkStepperModule } from '@angular/cdk/stepper';
import { StepperComponent } from './components/stepper/stepper.component';
import { BasketSummaryComponent } from './components/basket-summary/basket-summary.component';
import { ValidationSummaryComponent } from './components/validation-summary/validation-summary.component';
import { SafeHtmlPipe } from './pipes/safe-html.pipe';

@NgModule({
  declarations: [
    PagingInfoComponent,
    PagerComponent,
    OrderTotalsComponent,
    TextInputComponent,
    StepperComponent,
    BasketSummaryComponent,
    ValidationSummaryComponent,
    SafeHtmlPipe,
  ],
  imports: [
    CommonModule,
    PaginationModule.forRoot(),
    ReactiveFormsModule,
    FormsModule,
    CdkStepperModule,
  ],
  exports: [
    PaginationModule,
    PagingInfoComponent,
    PagerComponent,
    OrderTotalsComponent,
    ReactiveFormsModule,
    FormsModule,
    TextInputComponent,
    CdkStepperModule,
    StepperComponent,
    BasketSummaryComponent,
    ValidationSummaryComponent,
    SafeHtmlPipe,
  ],
})
export class SharedModule {}
