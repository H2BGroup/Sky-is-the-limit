import { inject, Injectable } from '@angular/core';
import { Flight } from './flight.model';
import { FLIGHTS } from './flights';
import { Router } from '@angular/router';

@Injectable({ providedIn: 'root' })
export class FlightService {
  private flights: Flight[] = FLIGHTS;
  private router = inject(Router);

  get getFlights() {
    return this.flights;
  }

  startBooking(flightId: string) {
    this.router.navigate(['/flights', flightId, 'book']);
  }
}
