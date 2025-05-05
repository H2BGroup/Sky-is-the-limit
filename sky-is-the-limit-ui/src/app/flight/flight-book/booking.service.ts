import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class BookingService {
  constructor(private http: HttpClient) {}

  createBooking(id: string, data: any): Observable<any> {
    return this.http.put(`/api/booking/${id}`, data);
  }

  getBooking(id: string): Observable<any> {
    return this.http.get(`/api/booking/${id}`);
  }
}
