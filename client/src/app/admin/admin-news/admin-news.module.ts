import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AdminNewsRoutingModule } from './admin-news-routing.module';
import { AdminNewsComponent } from './admin-news.component';
import { AdminNewsEditComponent } from './admin-news-edit/admin-news-edit.component';
import { CoreModule } from 'src/app/core/core.module';
import { SharedModule } from 'src/app/shared/shared.module';
import { EditorModule, TINYMCE_SCRIPT_SRC } from '@tinymce/tinymce-angular';


@NgModule({
  declarations: [
    AdminNewsComponent,
    AdminNewsEditComponent
  ],
  imports: [
    CommonModule,
    AdminNewsRoutingModule,
    CoreModule,
    SharedModule,
    EditorModule
  ],
  providers: [
    { provide: TINYMCE_SCRIPT_SRC, useValue: 'tinymce/tinymce.min.js' }
  ]
})
export class AdminNewsModule { }
