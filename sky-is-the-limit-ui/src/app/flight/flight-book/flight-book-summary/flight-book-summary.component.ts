import { Component } from '@angular/core';
import { FlightDetailsComponent } from '../shared/flight-details/flight-details.component';

@Component({
  selector: 'app-flight-book-summary',
  imports: [FlightDetailsComponent],
  templateUrl: './flight-book-summary.component.html',
  styleUrl: './flight-book-summary.component.css',
})
export class FlightBookSummaryComponent {}
