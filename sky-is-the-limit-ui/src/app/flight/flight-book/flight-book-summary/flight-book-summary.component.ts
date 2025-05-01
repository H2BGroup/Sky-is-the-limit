import { Component, inject, OnInit } from '@angular/core';
import { FlightDetailsComponent } from '../shared/flight-details/flight-details.component';
import { BookingDetailsComponent } from '../shared/booking-details/booking-details.component';
import { FlightService } from '../../flight.service';
import { ActivatedRoute } from '@angular/router';
import { Flight } from '../../flight.model';

@Component({
  selector: 'app-flight-book-summary',
  imports: [FlightDetailsComponent, BookingDetailsComponent],
  templateUrl: './flight-book-summary.component.html',
  styleUrl: './flight-book-summary.component.css',
})
export class FlightBookSummaryComponent implements OnInit {
  private flightService = inject(FlightService);
  private activatedRoute = inject(ActivatedRoute);

  flight?: Flight;

  ngOnInit(): void {
    const id = this.activatedRoute.snapshot.paramMap.get('id');
    if (id) {
      this.flightService.getFlight(id).subscribe((flight) => {
        this.flight = flight;
      });
    }
  }

  onToConfirmation() {
    this.flightService.toBookingConfirmation(this.flight!.id);
  }
  onBackToBookInfo() {
    this.flightService.toFlightDetails(this.flight!.id);
  }
}
