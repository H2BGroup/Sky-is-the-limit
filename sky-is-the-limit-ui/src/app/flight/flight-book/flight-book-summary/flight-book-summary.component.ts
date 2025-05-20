import { Component, inject, OnInit } from '@angular/core';
import { FlightDetailsComponent } from '../shared/flight-details/flight-details.component';
import { BookingDetailsComponent } from '../shared/booking-details/booking-details.component';
import { FlightService } from '../../flight.service';
import { ActivatedRoute } from '@angular/router';
import { Flight } from '../../flight.model';
import { BookingService } from '../booking.service';
import { BookingDetails } from '../booking-details.model';
import { v4 as uuidv4 } from 'uuid';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-flight-book-summary',
  imports: [FlightDetailsComponent, BookingDetailsComponent, CommonModule],
  templateUrl: './flight-book-summary.component.html',
  styleUrl: './flight-book-summary.component.css',
})
export class FlightBookSummaryComponent implements OnInit {
  private flightService = inject(FlightService);
  private bookingService = inject(BookingService);
  private activatedRoute = inject(ActivatedRoute);
  private selectedBookingDetails?: BookingDetails;

  flight?: Flight;

  private bookingId: string = uuidv4();
  protected isLoading = false;

  ngOnInit(): void {
    const id = this.activatedRoute.snapshot.paramMap.get('id');
    if (id) {
      this.flightService.getFlight(id).subscribe((flight) => {
        this.flight = flight;
        this.setSelectedBookingDetails();
      });
    }
  }

  setSelectedBookingDetails() {
    const savedBookForm = sessionStorage.getItem('bookFormData')
      ? JSON.parse(sessionStorage.getItem('bookFormData')!)
      : {};

    let totalPrice = sessionStorage.getItem('totalPrice')! as unknown as number;

    this.selectedBookingDetails = {
      firstClassSeats: savedBookForm.classSelection?.firstClass ?? 0,
      economyClassSeats: savedBookForm.classSelection?.economyClass ?? 0,
      carryOnBaggages: savedBookForm.baggageSelection?.carryOnBaggage ?? 0,
      checkedBaggages: savedBookForm.baggageSelection?.checkedBaggage ?? 0,
      priorityBoarding: savedBookForm.priorityBoarding ?? false,
      insurance: savedBookForm.insurance ?? false,
      price: totalPrice,
    };
  }

  onToConfirmation() {
    this.isLoading = true;
    console.log('Booking ID:', this.bookingId);
    console.log('Offer ID:', this.flight!.id);

    this.bookingService
      .createBooking(this.bookingId, {
        offerId: this.flight!.id,
        userId: sessionStorage.getItem('userId'),
        firstClassSeats: this.selectedBookingDetails?.firstClassSeats,
        secondClassSeats: this.selectedBookingDetails?.economyClassSeats,
        registeredBaggage: this.selectedBookingDetails?.checkedBaggages,
        carryOnBaggage: this.selectedBookingDetails?.carryOnBaggages,
        priorityBoarding: this.selectedBookingDetails?.priorityBoarding,
        insurance: this.selectedBookingDetails?.insurance,
        price: this.selectedBookingDetails?.price,
      })
      .subscribe({
        next: () => {
          const interval = setInterval(() => {
            this.bookingService.getBooking(this.bookingId).subscribe({
              next: (booking) => {
                if (booking.status === 0) {
                  // Booking is still pending, do nothing
                } else if (booking.status === 1) {
                  clearInterval(interval);
                  sessionStorage.setItem('bookingId', this.bookingId);
                  this.flightService.toBookingConfirmation(this.flight!.id);
                  this.isLoading = false;
                }
                console.log('Booking status:', booking.status);
              },
              error: (error) => {
                console.error('Error fetching booking status:', error);
                this.isLoading = false;
              },
            });
            console.log('Still checking booking status...');
          }, 1000);
        },
        error: (error) => {
          console.error('Error creating booking:', error);
          this.isLoading = false;
        },
      });
  }
  onBackToBookInfo() {
    this.flightService.toFlightDetails(this.flight!.id);
  }
}
