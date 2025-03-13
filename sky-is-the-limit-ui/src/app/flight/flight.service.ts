import { inject, Injectable } from '@angular/core';
import { Flight } from './flight.model';
import { FLIGHTS } from './flights';
import { Router } from '@angular/router';
import { Filters } from './flight-search-filter/filters.model';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class FlightService {
  private flightsSubject = new BehaviorSubject<Flight[]>(FLIGHTS);

  public filteredFlights$: BehaviorSubject<Flight[]> = new BehaviorSubject<
    Flight[]
  >(FLIGHTS);
  private router = inject(Router);

  get getFlights(): Observable<Flight[]> {
    return this.filteredFlights$.asObservable();
  }

  startBooking(flightId: string) {
    this.router.navigate(['/flights', flightId, 'book']);
  }

  backToList() {
    this.router.navigate(['/flights']);
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
        //(!filters.departureDate || flight.departureDate === filters.departureDate) &&
        //(!filters.arrivalDate || flight.arrivalDate === filters.arrivalDate) &&
        //(!filters.passengers <= flight.passengers) &&
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
