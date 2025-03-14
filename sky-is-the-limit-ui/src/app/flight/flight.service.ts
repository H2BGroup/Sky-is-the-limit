import { inject, Injectable } from '@angular/core';
import { Flight } from './flight.model';
import { FLIGHTS } from './flights';
import { ActivatedRoute, Router } from '@angular/router';
import { Filters } from './flight-search-filter/filters.model';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class FlightService {
  private flightsSubject = new BehaviorSubject<Flight[]>(FLIGHTS);

  public filteredFlights$: BehaviorSubject<Flight[]> = new BehaviorSubject<
    Flight[]
  >(FLIGHTS);
  private router = inject(Router);
  private activatedRoute = inject(ActivatedRoute);

  get getFlights(): Observable<Flight[]> {
    return this.filteredFlights$.asObservable();
  }

  toFlightDetails(flightId: string) {
    this.router.navigate(['/flights', flightId, 'book', 'details']);
  }

  proceedWithBooking(flightId: string) {
    this.router.navigate(['/flights', flightId, 'book', 'info']);
  }

  backToList() {
    this.router.navigate(['/flights']);
  }

  toBookingSummary(flightId: string) {
    this.router.navigate(['/flights', flightId, 'book', 'summary']);
  }

  filterFlights(filters: Filters) {
    const filteredFlights = this.flightsSubject.getValue().filter((flight) => {
      return (
        (!filters.departure ||
          flight.departure
            .toLowerCase()
            .includes(filters.departure.toLowerCase())) &&
        (!filters.arrival ||
          flight.arrival
            .toLowerCase()
            .includes(filters.arrival.toLowerCase())) &&
        (!filters.fromDate || filters.fromDate <= flight.datetime) &&
        (!filters.toDate || filters.toDate >= flight.datetime) &&
        (!filters.passengers ||
          filters.passengers <= flight.seatsEconomy + flight.seatsFirstClass) &&
        (!filters.price || filters.price >= flight.price)
      );
    });
    this.filteredFlights$.next(filteredFlights);
  }

  getFlight(flightId: string) {
    return this.flightsSubject
      .getValue()
      .find((flight) => flight.id === flightId);
  }
}
