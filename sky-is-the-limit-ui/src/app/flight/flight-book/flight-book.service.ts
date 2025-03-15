import { Injectable } from '@angular/core';
import { BookingDetails } from './booking-details.model';
import { PersonalDetails } from './personal-details.model';

@Injectable({ providedIn: 'root' })
export class FlightBookService {
  private bookingDetails?: BookingDetails;
  private personalDetails?: PersonalDetails;

  setBookingDetails(bookingDetails: BookingDetails) {
    this.bookingDetails = bookingDetails;
  }

  getBookingDetails(): BookingDetails {
    return this.bookingDetails!;
  }

  setPersonalDetails(personalDetails: PersonalDetails) {
    this.personalDetails = personalDetails;
  }

  getPersonalDetails(): PersonalDetails {
    return this.personalDetails!;
  }
}
