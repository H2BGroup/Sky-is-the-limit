import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class PaymentService {
  constructor(private http: HttpClient) {}

  createPayment(id: string): Observable<any> {
    return this.http.put(`/api/payment/${id}`, {});
  }

  getPayment(id: string): Observable<any> {
    return this.http.get(`/api/payment/${id}`);
  }
}
