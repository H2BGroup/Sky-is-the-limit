import { Component, inject, OnInit } from '@angular/core';
import { FlightService } from '../../../flight.service';
import { ActivatedRoute } from '@angular/router';
import { Flight } from '../../../flight.model';
import {
  CARRY_ON_BAGGAGE_PRICE,
  CHECKED_BAGGAGE_PRICE,
  FIRST_CLASS_PRICE_RATIO,
  INSURANCE_PRICE,
  PRIORITY_BOARDING_PRICE,
} from '../../price-constants';

@Component({
  selector: 'app-booking-details',
  imports: [],
  templateUrl: './booking-details.component.html',
  styleUrl: './booking-details.component.css',
})
export class BookingDetailsComponent implements OnInit {
  private flightService = inject(FlightService);
  private activatedRoute = inject(ActivatedRoute);

  protected selectedFirstClassSeats?: number;
  protected selectedEconomyClassSeats?: number;
  protected selectedCarryOnBaggages?: number;
  protected selectedCheckedBaggages?: number;
  protected selectedPriorityBoarding?: boolean;
  protected selectedInsurance?: boolean;
  protected totalPrice?: number;

  pricingDetails: string[] = [];

  flight?: Flight = this.flightService.getFlight(
    this.activatedRoute.snapshot.paramMap.get('id')!
  );

  ngOnInit(): void {
    this.setSelectedBookingDetails();
    this.showDetailedPrice();
  }

  setSelectedBookingDetails() {
    const savedBookForm = sessionStorage.getItem('bookFormData')
      ? JSON.parse(sessionStorage.getItem('bookFormData')!)
      : {};

    this.selectedFirstClassSeats =
      savedBookForm.classSelection?.firstClass ?? 0;
    this.selectedEconomyClassSeats =
      savedBookForm.classSelection?.economyClass ?? 0;
    this.selectedCarryOnBaggages =
      savedBookForm.baggageSelection?.carryOnBaggage ?? 0;
    this.selectedCheckedBaggages =
      savedBookForm.baggageSelection?.checkedBaggage ?? 0;
    this.selectedPriorityBoarding = savedBookForm.priorityBoarding ?? false;
    this.selectedInsurance = savedBookForm.insurance ?? false;
    this.totalPrice = sessionStorage.getItem(
      'totalPrice'
    )! as unknown as number;
  }

  showDetailedPrice() {
    let basePrice = this.flight!.price;
    this.pricingDetails = [];

    if (this.selectedFirstClassSeats) {
      this.pricingDetails.push(
        `First Class Seats (${this.selectedFirstClassSeats}): ${
          this.selectedFirstClassSeats * basePrice * FIRST_CLASS_PRICE_RATIO
        } zł`
      );
    }
    if (this.selectedEconomyClassSeats) {
      this.pricingDetails.push(
        `Economy Class Seats (${this.selectedEconomyClassSeats}): ${
          this.selectedEconomyClassSeats * basePrice
        } zł`
      );
    }
    if (this.selectedCarryOnBaggages) {
      this.pricingDetails.push(
        `Carry-on Baggages (${this.selectedCarryOnBaggages}): ${
          this.selectedCarryOnBaggages * CARRY_ON_BAGGAGE_PRICE
        } zł`
      );
    }
    if (this.selectedCheckedBaggages) {
      this.pricingDetails.push(
        `Checked Baggages (${this.selectedCheckedBaggages}): ${
          this.selectedCheckedBaggages * CHECKED_BAGGAGE_PRICE
        } zł`
      );
    }
    if (this.selectedPriorityBoarding) {
      this.pricingDetails.push(
        `Priority Boarding: ${PRIORITY_BOARDING_PRICE} zł`
      );
    }
    if (this.selectedInsurance) {
      this.pricingDetails.push(`Insurance: ${INSURANCE_PRICE} zł`);
    }
  }
}
