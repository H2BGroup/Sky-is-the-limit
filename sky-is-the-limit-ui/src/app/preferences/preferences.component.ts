import { Component, inject, OnInit } from '@angular/core';
import { NotificationsService } from '../notifications.service';
import { FlightService } from '../flight/flight.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-preferences',
  imports: [CommonModule],
  templateUrl: './preferences.component.html',
  styleUrl: './preferences.component.css',
})
export class PreferencesComponent implements OnInit {
  private notifcationsService = inject(NotificationsService);
  private flightService = inject(FlightService);

  protected offersPurchased: any[] = [];

  protected averagePrice: number = 0;
  protected averageBaggage: number = 0;
  protected averageSeats: number = 0;
  protected insurancePercentage: number = 0;
  protected priorityBoardingPercentage: number = 0;

  ngOnInit() {
    this.notifcationsService.startConnection();
    this.notifcationsService.receiveBookingConfirmed((data) =>
      this.onMessageReceived(data)
    );
  }

  onMessageReceived(data: any) {
    this.offersPurchased.push(data);
    this.calculateAverages();
  }

  calculateAverages() {
    let sumPrice = 0;
    let sumBaggage = 0;
    let sumSeats = 0;
    let insuranceCount = 0;
    let priorityBoardingCount = 0;
    for (const offer of this.offersPurchased) {
      sumSeats += offer.firstClassSeats + offer.secondClassSeats;
      sumBaggage += offer.registeredBaggage + offer.carryOnBaggage;
      sumPrice += offer.price;
      if (offer.insurance) {
        insuranceCount++;
      }
      if (offer.priorityBoarding) {
        priorityBoardingCount++;
      }
    }
    if (this.offersPurchased.length > 0) {
      this.averagePrice = sumPrice / this.offersPurchased.length;
      this.averageBaggage = sumBaggage / this.offersPurchased.length;
      this.averageSeats = sumSeats / this.offersPurchased.length;
      this.insurancePercentage =
        (insuranceCount / this.offersPurchased.length) * 100;
      this.priorityBoardingPercentage =
        (priorityBoardingCount / this.offersPurchased.length) * 100;
    }
  }
}
