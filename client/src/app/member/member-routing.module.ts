import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MemberCenterComponent } from './member-center/member-center.component';
import { MemberComponent } from './member.component';
import { PersonalInfoComponent } from './personal-info/personal-info.component';

const routes: Routes = [
  {
    path: '',
    component: MemberComponent,
    children: [
      {
        path: '',
        redirectTo: 'member-center',
        pathMatch: 'full',
      },
      {
        path: 'member-center',
        component: MemberCenterComponent,
        data: { breadcrumb: '會員中心' },
      },
      {
        path: 'personal-info',
        component: PersonalInfoComponent,
        data: { breadcrumb: '個人資料' },
      },
      {
        path: 'order',
        loadChildren: () =>
          import('./order/order.module').then((mod) => mod.OrderModule),
        data: { breadcrumb: '訂單查詢' },
      },
    ],
  },
];

@NgModule({
  declarations: [],
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class MemberRoutingModule {}
