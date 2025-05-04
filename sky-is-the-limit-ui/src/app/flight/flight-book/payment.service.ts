import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ConfigService } from '../../config.service';

@Injectable({
  providedIn: 'root',
})
export class PaymentService {
  constructor(private http: HttpClient, private config: ConfigService) {}

  createPayment(id: string): Observable<any> {
    return this.http.put(`${this.config.paymentUrl}/${id}`, {});
  }

  getPayment(id: string): Observable<any> {
    return this.http.get(`${this.config.paymentUrl}/${id}`);
  }
}
