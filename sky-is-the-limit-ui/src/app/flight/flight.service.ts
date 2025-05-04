import { inject, Injectable } from '@angular/core';
import { Flight } from './flight.model';
import { Router } from '@angular/router';
import { Filters } from './flight-search-filter/filters.model';
import { BehaviorSubject, map, Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { FLIGHTS } from './flights';

@Injectable({ providedIn: 'root' })
export class FlightService {
  private flightsSubject = new BehaviorSubject<Flight[]>([]);

  public filteredFlights$: BehaviorSubject<Flight[]> = new BehaviorSubject<
    Flight[]
  >([]);
  private router = inject(Router);

  constructor(private http: HttpClient) {}

  fetchAndStoreFlights(): void {
    this.getFlights.subscribe((flights) => {
      this.flightsSubject.next(flights);
      this.filteredFlights$.next(flights);
    });
  }

  get getFlights(): Observable<Flight[]> {
    return this.http
      .get<{ offers: Flight[] }>('http://localhost:5000/api/offer')
      .pipe(map((response) => response.offers));
  }

  getFlightDetails(flightId: string): Observable<Partial<Flight>> {
    return this.http.get<Partial<Flight>>(
      `http://localhost:5000/api/offer/${flightId}`
    );
  }

  toFlightList() {
    this.router.navigate(['/flights']);
  }

  toFlightDetails(flightId: string) {
    this.router.navigate(['/flights', flightId, 'book', 'details']);
  }

  toBookingSummary(flightId: string) {
    this.router.navigate(['/flights', flightId, 'book', 'summary']);
  }

  toBookingConfirmation(flightId: string) {
    this.router.navigate(['/flights', flightId, 'book', 'confirm']);
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
        (!filters.price || filters.price >= flight.price)
      );
    });
    this.filteredFlights$.next(filteredFlights);
    console.log('Filtered flights:', filteredFlights.length);
  }

  getFlight(flightId: string): Observable<Flight> {
    const localFlight = this.flightsSubject
      .getValue()
      .find((flight) => flight.id === flightId);

    return this.getFlightDetails(flightId).pipe(
      map((detail: Partial<Flight>) => {
        if (!detail) throw new Error('Brak szczegółów lotu z API');
        return { ...localFlight, ...detail } as Flight;
      })
    );
  }
}
