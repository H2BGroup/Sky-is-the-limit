import { Component } from '@angular/core';
import { Flight } from '../flight.model';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatTooltipModule } from '@angular/material/tooltip';
import { FLIGHTS } from '../flights';

@Component({
  selector: 'app-flight-list',
  imports: [MatExpansionModule, MatTooltipModule],
  templateUrl: './flight-list.component.html',
  styleUrl: './flight-list.component.css',
})
export class FlightListComponent {
  protected flights: Flight[] = FLIGHTS;

  createFlightSummary(flight: Flight): string {
    return `${flight.departure} ---> ${flight.arrival} for ${flight.price} zł`;
  }
}
