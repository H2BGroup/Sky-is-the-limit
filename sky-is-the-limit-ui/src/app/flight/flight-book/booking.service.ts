import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ConfigService } from '../../config.service';

@Injectable({
  providedIn: 'root',
})
export class BookingService {
  constructor(private http: HttpClient, private config: ConfigService) {}

  createBooking(id: string, data: any): Observable<any> {
    return this.http.put(`${this.config.bookingUrl}/${id}`, data);
  }

  getBooking(id: string): Observable<any> {
    return this.http.get(`${this.config.bookingUrl}/${id}`);
  }
}
