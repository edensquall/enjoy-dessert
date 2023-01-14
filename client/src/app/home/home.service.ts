import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, of } from 'rxjs';
import { environment } from 'src/environments/environment';
import { ISlide } from '../shared/models/slide';

@Injectable({
  providedIn: 'root',
})
export class HomeService {
  baseUrl = environment.apiUrl;
  slideCache!: ISlide[];

  constructor(private http: HttpClient) {}

  getSlides(useCache: boolean) {
    if (useCache === false) {
      this.slideCache = [];
    }

    if (this.slideCache && useCache === true) {
      return of(this.slideCache);
    }
    return this.http.get<ISlide[]>(this.baseUrl + 'slides').pipe(
      map((response) => {
        this.slideCache = response;
        return response;
      })
    );
  }
}
