import { Component } from '@angular/core';
import { FlightDetailsComponent } from '../shared/flight-details/flight-details.component';
import { BookingDetailsComponent } from '../shared/booking-details/booking-details.component';
import { PersonalDetailsComponent } from '../shared/personal-details/personal-details.component';

@Component({
  selector: 'app-flight-book-summary',
  imports: [
    FlightDetailsComponent,
    BookingDetailsComponent,
    PersonalDetailsComponent,
  ],
  templateUrl: './flight-book-summary.component.html',
  styleUrl: './flight-book-summary.component.css',
})
export class FlightBookSummaryComponent {}
