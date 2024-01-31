import { Component, OnInit } from '@angular/core';
import { AdminNewsService } from '../admin-news.service';
import { ActivatedRoute } from '@angular/router';
import { BreadcrumbService } from 'xng-breadcrumb';
import { IAdminNews } from 'src/app/shared/models/adminNews';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { environment } from 'src/environments/environment';
import { v4 as uuidv4 } from 'uuid';
import { Location } from '@angular/common';

@Component({
  selector: 'app-admin-news-edit',
  templateUrl: './admin-news-edit.component.html',
  styleUrls: ['./admin-news-edit.component.scss'],
})
export class AdminNewsEditComponent implements OnInit {
  id = this.activateRoute.snapshot.paramMap.get('id');
  tempId!: string;
  functionText!: string;
  tinyInit = {};
  editAdminNewsForm!: FormGroup;
  adminNews!: IAdminNews;


  constructor(
    private location: Location,
    private activateRoute: ActivatedRoute,
    private bcService: BreadcrumbService,
    private fb: FormBuilder,
    private adminNewsService: AdminNewsService
  ) {
  }

  ngOnInit(): void {
    this.functionText = this.id === '0' ? '新增' : '修改';
    this.bcService.set('@editNews', this.functionText + '最新消息');

    this.tempId = uuidv4();
    this.createEditAdminNewsForm();
    if (this.id !== '0') {
      this.getAdminNews();
    } 
    this.initTinyMce();
  }

  initTinyMce() {
    this.tinyInit = {
      images_upload_url: `${environment.apiUrl}admin/news/uploadNewsDetailImage/${this.id !== '0' ? this.id : this.tempId}`,
      convert_urls: false,
      height: 800,
      plugins: 'lists link image table code help wordcount',
    };
  }

  getAdminNews() {
    this.adminNewsService.getAdminNews(this.id ? +this.id : 0).subscribe({
      next: (response: IAdminNews) => {
        this.adminNews = response;
        this.editAdminNewsForm?.patchValue(response);
      },
      error: (error: any) => {
        console.log(error);
      },
      complete: () => {},
    });
  }

  createEditAdminNewsForm() {
    this.editAdminNewsForm = this.fb.group({
      id: this.id,
      tempId: this.tempId,
      title: [null, [Validators.required]],
      thumbnailFile: [null],
      caption: [null, [Validators.required]],
      content: [null, [Validators.required]],
      isShow: [false],
      isShowByDate: [false],
      startDate: ['', [Validators.required]],
      endDate: [''],
    });
  }

  onFileChange(event: any) {
    if (event.target.files.length > 0) {
      const file = event.target.files[0];
      this.editAdminNewsForm.patchValue({
        thumbnailFile: file,
      });
    }
  }

  onAdminNewsSubmit() {
    var formData: FormData = new FormData();

    Object.keys(this.editAdminNewsForm.value).forEach((key) => {
      if (key === 'endDate' && this.editAdminNewsForm.value[key] === null)
      {
        formData.append(key, '');
      }
      formData.append(key, this.editAdminNewsForm.value[key]);
    });

    if (this.id === '0') {
      this.adminNewsService.createAdminNews(formData).subscribe({
        next: (adminNews: IAdminNews) => {
          this.id = adminNews.id.toString();
          this.location.go('/admin/news/' + this.id);
          this.ngOnInit();
        },
        error: (error: any) => {
          console.log(error);
        },
        complete: () => {},
      });
    } else {
      this.adminNewsService
        .updateAdminNews(formData).subscribe({
          next: (adminNews: IAdminNews) => {
            this.ngOnInit();
          },
          error: (error: any) => {
            console.log(error);
          },
          complete: () => {},
        });
    }
  }
}
