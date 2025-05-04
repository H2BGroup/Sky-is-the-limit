import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class PaymentService {
  private apiUrl = 'http://localhost:5000/api/payment';

  constructor(private http: HttpClient) {}

  createPayment(id: string): Observable<any> {
    return this.http.put(`${this.apiUrl}/${id}`, {});
  }

  getPayment(id: string): Observable<any> {
    return this.http.get(`${this.apiUrl}/${id}`);
  }
}
