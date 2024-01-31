import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminAccountComponent } from './admin-account.component';
import { AdminAccountEditComponent } from './admin-account-edit/admin-account-edit.component';

const routes: Routes = [
  { path: '', component: AdminAccountComponent },
  {
    path: ':id',
    component: AdminAccountEditComponent,
    data: { breadcrumb: { alias: 'editAccount' } },
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AdminAccountRoutingModule {}
