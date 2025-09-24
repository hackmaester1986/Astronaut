import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AstronautDuty, Person } from '../interfaces/interfaces';
import { environment } from '../environments/environment.development';

@Injectable({ providedIn: 'root' })
export class AstronautDutyService {
  private readonly baseUrl = `${environment.apiBaseUrl}/AstronautDuty`;
  constructor(private http: HttpClient) {}

  /*getByPersonName(name: string): Observable<Person> {
    return this.http.get<Person>(
      `${this.baseUrl}/${encodeURIComponent(name)}`
    );
  }

  getByPersonId(personId: number): Observable<AstronautDuty[]> {
    return this.http.get<AstronautDuty[]>(
      `${this.baseUrl}/person/${personId}`
    );
  }

  createDuty(duty: Omit<AstronautDuty, 'id'>): Observable<any> {
    return this.http.post<BaseResponse>(this.baseUrl, duty);
  }

  endDuty(id: number, endDate: string): Observable<BaseResponse> {
    return this.http.patch<BaseResponse>(`${this.baseUrl}/${id}/end`, { endDate });
  }

  getCurrentDuty(personId: number): Observable<AstronautDuty | null> {
    return this.http.get<AstronautDuty | null>(
      `${this.baseUrl}/current/${personId}`
    );
  }*/
}
