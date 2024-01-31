import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  {
    path: 'account',
    loadChildren: () =>
      import('./admin-account/admin-account.module').then((mod) => mod.AdminAccountModule),
    data: { breadcrumb: '會員管理' },
  },
  {
    path: 'news',
    loadChildren: () =>
      import('./admin-news/admin-news.module').then((mod) => mod.AdminNewsModule),
    data: { breadcrumb: '最新消息' },
  },
  {
    path: 'order',
    loadChildren: () =>
      import('./admin-order/admin-order.module').then((mod) => mod.AdminOrderModule),
    data: { breadcrumb: '訂單管理' },
  },
  {
    path: 'product',
    loadChildren: () =>
      import('./admin-product/admin-product.module').then((mod) => mod.AdminProductModule),
    data: { breadcrumb: '產品' },
  },
  {
    path: 'producttype',
    loadChildren: () =>
      import('./admin-product-type/admin-product-type.module').then((mod) => mod.AdminProductTypeModule),
    data: { breadcrumb: '產品類別' },
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminRoutingModule { }
