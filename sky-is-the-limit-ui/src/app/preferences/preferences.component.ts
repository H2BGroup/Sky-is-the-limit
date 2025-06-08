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
  protected firstClassPercentage: number = 0;
  protected mostUsedAirlines: { airline: string; count: number }[] = [];
  protected mostUsedRoutes: { route: string; count: number }[] = [];
  protected animate: boolean = false;

  ngOnInit() {
    this.flightService.fetchAndStoreFlights();
    this.notifcationsService.startConnection();
    this.notifcationsService.receiveBookingConfirmed((data) =>
      this.onMessageReceived(data)
    );
  }

  onMessageReceived(data: any) {
    this.offersPurchased.push(data);
    this.calculateAverages();
    this.triggerAnimation();

    const allOffers = this.flightService.filteredFlights$.getValue();

    this.mostUsedAirlines = this.getMostUsedAirlines(
      this.offersPurchased,
      allOffers
    );
    this.mostUsedRoutes = this.getMostUsedRoutes(
      this.offersPurchased,
      allOffers
    );
  }

  calculateAverages() {
    let sumPrice = 0;
    let sumBaggage = 0;
    let sumSeats = 0;
    let insuranceCount = 0;
    let priorityBoardingCount = 0;
    let sumFirstClass = 0;
    for (const offer of this.offersPurchased) {
      sumSeats += offer.firstClassSeats + offer.secondClassSeats;
      sumBaggage += offer.registeredBaggage + offer.carryOnBaggage;
      sumPrice += offer.price;
      sumFirstClass += offer.firstClassSeats;
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
      this.firstClassPercentage = (sumFirstClass / sumSeats) * 100;
    }
  }

  getMostFrequentOffers(data: any) {
    const frequencyMap: Record<string, number> = {};

    for (const item of data) {
      frequencyMap[item.offerId] = (frequencyMap[item.offerId] || 0) + 1;
    }

    const sorted = Object.entries(frequencyMap)
      .map(([offerId, count]) => ({ offerId, count }))
      .sort((a, b) => b.count - a.count);

    console.log('Most Frequent Offers:', sorted);
    return sorted;
  }

  getMostUsedAirlines(data: any, allOffers: any) {
    const usageMap: Record<string, number> = {};
    const airlineCountMap: Record<string, number> = {};

    const mostFrequentOffers = this.getMostFrequentOffers(data);

    for (const { offerId, count } of mostFrequentOffers) {
      usageMap[offerId] = count;
    }

    for (const offer of allOffers) {
      const usageCount = usageMap[offer.id];
      if (usageCount) {
        airlineCountMap[offer.airline] =
          (airlineCountMap[offer.airline] || 0) + usageCount;
      }
    }

    const sortedAirlines = Object.entries(airlineCountMap)
      .map(([airline, count]) => ({ airline, count }))
      .sort((a, b) => b.count - a.count);

    console.log('Most Used Airlines:', sortedAirlines);
    return sortedAirlines;
  }

  getMostUsedRoutes(purchasedOffers: any[], allOffers: any[]) {
    const usageMap: Record<string, number> = {};
    const routeCountMap: Record<string, number> = {};

    const mostFrequentOffers = this.getMostFrequentOffers(purchasedOffers);

    for (const { offerId, count } of mostFrequentOffers) {
      usageMap[offerId] = count;
    }

    for (const offer of allOffers) {
      const usageCount = usageMap[offer.id];
      if (usageCount) {
        const routeKey = `${offer.departure} → ${offer.arrival}`;
        routeCountMap[routeKey] = (routeCountMap[routeKey] || 0) + usageCount;
      }
    }

    const sortedRoutes = Object.entries(routeCountMap)
      .map(([route, count]) => ({ route, count }))
      .sort((a, b) => b.count - a.count);

    console.log('Most Frequent Routes:', sortedRoutes);
    return sortedRoutes;
  }

  triggerAnimation() {
    this.animate = false;
    setTimeout(() => {
      this.animate = true;
    }, 0);
  }

  onAnimationEnd() {
    this.animate = false;
  }
}
