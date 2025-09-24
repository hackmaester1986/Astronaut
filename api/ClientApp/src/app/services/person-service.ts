import { inject, Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../environments/environment.development';


@Injectable({ providedIn: 'root' })
export class PersonService {
  private readonly baseUrl = `${environment.apiBaseUrl}/Person`;
  private http = inject(HttpClient);

  getPeople(): Observable<any> {               
    return this.http.get<any>(this.baseUrl);
  }

  getPersonByName(name: string): Observable<any> { 
    const url = `${this.baseUrl}/${encodeURIComponent(name)}`;
    return this.http.get<any>(url);
  }

  createPerson(name: string): Observable<any> {
    const headers = new HttpHeaders().set('Content-Type', 'application/json');
    return this.http.post<any>(this.baseUrl, JSON.stringify(name), { headers });
  }
}

