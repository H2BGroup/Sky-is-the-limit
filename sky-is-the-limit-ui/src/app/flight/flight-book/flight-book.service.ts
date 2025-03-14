import { Injectable } from '@angular/core';
import { BookingDetails } from './booking-details.model';

@Injectable({ providedIn: 'root' })
export class FlightBookService {
  private bookingDetails?: BookingDetails;

  setBookingDetails(bookingDetails: BookingDetails) {
    this.bookingDetails = bookingDetails;
  }

  getBookingDetails(): BookingDetails {
    return this.bookingDetails!;
  }
}
