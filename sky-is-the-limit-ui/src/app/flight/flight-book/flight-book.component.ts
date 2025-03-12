import { Component, inject } from '@angular/core';
import { FlightService } from '../flight.service';

@Component({
  selector: 'app-flight-book',
  imports: [],
  templateUrl: './flight-book.component.html',
  styleUrl: './flight-book.component.css',
})
export class FlightBookComponent {
  flightService = inject(FlightService);

  OnBackToList() {
    this.flightService.backToList();
  }
}
