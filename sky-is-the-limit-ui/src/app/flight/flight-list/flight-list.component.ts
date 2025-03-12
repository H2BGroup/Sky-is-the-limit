import { Component, inject } from '@angular/core';
import { Flight } from '../flight.model';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatTooltipModule } from '@angular/material/tooltip';
import { FlightService } from '../flight.service';
import { Observable } from 'rxjs';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-flight-list',
  imports: [MatExpansionModule, MatTooltipModule, CommonModule],
  templateUrl: './flight-list.component.html',
  styleUrl: './flight-list.component.css',
})
export class FlightListComponent {
  private flightService = inject(FlightService);

  protected flights$: Observable<Flight[]> = this.flightService.getFlights;

  createFlightSummary(flight: Flight): string {
    return `✈️ ${flight.departure} → ${flight.arrival} for ${flight.price} zł`;
  }

  onBookFlight(flightId: string) {
    this.flightService.startBooking(flightId);
  }
}
