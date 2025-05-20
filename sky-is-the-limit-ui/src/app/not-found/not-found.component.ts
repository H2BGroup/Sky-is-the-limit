import { Component, inject } from '@angular/core';
import { FlightService } from '../flight/flight.service';

@Component({
  selector: 'app-not-found',
  imports: [],
  templateUrl: './not-found.component.html',
  styleUrl: './not-found.component.css',
})
export class NotFoundComponent {
  flightService = inject(FlightService);

  onToMainPage() {
    this.flightService.toFlightList();
  }
}
