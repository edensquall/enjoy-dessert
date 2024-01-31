import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminNewsComponent } from './admin-news.component';
import { AdminNewsEditComponent } from './admin-news-edit/admin-news-edit.component';

const routes: Routes = [
  { path: '', component: AdminNewsComponent },
  {
    path: ':id',
    component: AdminNewsEditComponent,
    data: { breadcrumb: { alias: 'editNews' } },
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AdminNewsRoutingModule {}
