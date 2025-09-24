import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../environments/environment.development';
import { CreateAstronautDetailRequest } from '../interfaces/interfaces';


@Injectable({ providedIn: 'root' })
export class AstronautDetailService {

  private readonly baseUrl = `${environment.apiBaseUrl}/AstronautDetail`;

  constructor(private http: HttpClient) {}

  createAstronautDetail(req: CreateAstronautDetailRequest): Observable<void> {
    return this.http.post<void>(this.baseUrl, req);
  }
}
