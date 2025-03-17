import { Component, inject } from '@angular/core';
import { FlightDetailsComponent } from '../shared/flight-details/flight-details.component';
import { BookingDetailsComponent } from '../shared/booking-details/booking-details.component';
import { PersonalDetailsComponent } from '../shared/personal-details/personal-details.component';
import { FlightService } from '../../flight.service';
import { ActivatedRoute } from '@angular/router';
import { Flight } from '../../flight.model';

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
export class FlightBookSummaryComponent {
  private flightService = inject(FlightService);
  private activatedRoute = inject(ActivatedRoute);

  flight?: Flight = this.flightService.getFlight(
    this.activatedRoute.snapshot.paramMap.get('id')!
  );

  onToConfirmation() {
    this.flightService.toBookingConfirmation(this.flight!.id);
  }
  onBackToBookInfo() {
    this.flightService.toPersonalDetailsForm(this.flight!.id);
  }
}
