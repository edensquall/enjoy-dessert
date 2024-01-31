import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from './core/guards/auth.guard';
import { NotFoundComponent } from './core/not-found/not-found.component';
import { ServerErrorComponent } from './core/server-error/server-error.component';
import { TestErrorComponent } from './core/test-error/test-error.component';

const routes: Routes = [
  {
    path: '',
    loadChildren: () =>
      import('./home/home.module').then((mod) => mod.HomeModule),
    data: { breadcrumb: '首頁' },
  },
  {
    path: 'test-error',
    component: TestErrorComponent,
    data: { breadcrumb: '錯誤測試' },
  },
  {
    path: 'server-error',
    component: ServerErrorComponent,
    data: { breadcrumb: '伺服器錯誤' },
  },
  {
    path: 'not-found',
    component: NotFoundComponent,
    data: { breadcrumb: '無法找到伺服器' },
  },
  {
    path: 'shop',
    loadChildren: () =>
      import('./shop/shop.module').then((mod) => mod.ShopModule),
    data: { breadcrumb: '線上訂購' },
  },
  {
    path: 'contact',
    loadChildren: () =>
      import('./contact/contact.module').then((mod) => mod.ContactModule),
    data: { breadcrumb: '聯絡我們' },
  },
  {
    path: 'news',
    loadChildren: () =>
      import('./news/news.module').then((mod) => mod.NewsModule),
    data: { breadcrumb: '最新消息' },
  },
  {
    path: 'account',
    loadChildren: () =>
      import('./account/account.module').then((mod) => mod.AccountModule),
    data: { breadcrumb: { skip: true } },
  },
  {
    path: 'basket',
    loadChildren: () =>
      import('./basket/basket.module').then((mod) => mod.BasketModule),
    data: { breadcrumb: '購物籃' },
  },
  {
    path: 'checkout',
    canActivate: [AuthGuard],
    loadChildren: () =>
      import('./checkout/checkout.module').then((mod) => mod.CheckoutModule),
    data: { breadcrumb: '結帳' },
  },
  {
    path: 'member',
    canActivate: [AuthGuard],
    loadChildren: () =>
      import('./member/member.module').then((mod) => mod.MemberModule),
    data: { breadcrumb: '會員' },
  },
  {
    path: 'admin',
    canActivate: [AuthGuard],
    loadChildren: () =>
      import('./admin/admin.module').then((mod) => mod.AdminModule),
    data: { breadcrumb: { skip: true } },
  },
  { path: '**', redirectTo: 'not-found', pathMatch: 'full' },
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes, {
      scrollPositionRestoration: 'top',
    }),
  ],
  exports: [RouterModule],
})
export class AppRoutingModule {}
