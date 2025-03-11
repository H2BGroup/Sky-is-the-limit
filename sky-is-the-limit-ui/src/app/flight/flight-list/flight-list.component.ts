import { Component, inject } from '@angular/core';
import { Flight } from '../flight.model';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatTooltipModule } from '@angular/material/tooltip';
import { FLIGHTS } from '../flights';
import { FlightService } from '../flight.service';

@Component({
  selector: 'app-flight-list',
  imports: [MatExpansionModule, MatTooltipModule],
  templateUrl: './flight-list.component.html',
  styleUrl: './flight-list.component.css',
})
export class FlightListComponent {
  private flightService = inject(FlightService);

  protected flights: Flight[] = this.flightService.getFlights;

  createFlightSummary(flight: Flight): string {
    return `${flight.departure} ---> ${flight.arrival} for ${flight.price} zł`;
  }

  onBookFlight(flightId: string) {
    this.flightService.startBooking(flightId);
  }
}
