import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../environments/environment.development';
import { CreateAstronautDetailRequest, CreateNewAstronautRequest } from '../interfaces/interfaces';


@Injectable({ providedIn: 'root' })
export class AstronautDetailService {

  private readonly baseUrl = `${environment.apiBaseUrl}/AstronautDetail`;

  constructor(private http: HttpClient) {}

  createAstronautDetail(req: CreateNewAstronautRequest): Observable<void> {
    return this.http.post<void>(this.baseUrl, req);
  }

  updateAstronautDetail(req: CreateAstronautDetailRequest): Observable<void> {
    return this.http.put<void>(this.baseUrl+'/update', req);
  }


  getDetailByName(name: string): Observable<any> {
    return this.http.get<any>(
      `${this.baseUrl}/${encodeURIComponent(name)}`
    );
  }
}
