import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AstronautDuty, CreateAstronautDutyRequest, Person } from '../interfaces/interfaces';
import { environment } from '../environments/environment.development';

@Injectable({ providedIn: 'root' })
export class AstronautDutyService {
  private readonly baseUrl = `${environment.apiBaseUrl}/AstronautDuty`;
  constructor(private http: HttpClient) {}

  getByPersonName(name: string): Observable<any> {
    return this.http.get<any>(
      `${this.baseUrl}/${encodeURIComponent(name)}`
    );
  }

  createDuty(duty: CreateAstronautDutyRequest): Observable<AstronautDuty> {
    return this.http.post<AstronautDuty>(this.baseUrl, duty);
  }

}
