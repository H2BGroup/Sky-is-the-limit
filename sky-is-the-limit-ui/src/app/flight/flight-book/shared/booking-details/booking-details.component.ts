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
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-booking-details',
  imports: [CommonModule],
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

  flight?: Flight;

  ngOnInit(): void {
    const id = this.activatedRoute.snapshot.paramMap.get('id');
    if (id) {
      this.flightService.getFlight(id).subscribe((flight) => {
        this.flight = flight;
        this.setSelectedBookingDetails();
        this.showDetailedPrice();
      });
    }
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
      const firstClassPrice = (
        this.selectedFirstClassSeats *
        basePrice *
        FIRST_CLASS_PRICE_RATIO
      ).toFixed(2);
      this.pricingDetails.push(
        `First Class Seats (${this.selectedFirstClassSeats}): ${firstClassPrice} zł`
      );
    }
    if (this.selectedEconomyClassSeats) {
      const economyClassPrice = (
        this.selectedEconomyClassSeats * basePrice
      ).toFixed(2);
      this.pricingDetails.push(
        `Economy Class Seats (${this.selectedEconomyClassSeats}): ${economyClassPrice} zł`
      );
    }
    if (this.selectedCarryOnBaggages) {
      const carryOnBaggagePrice = (
        this.selectedCarryOnBaggages * CARRY_ON_BAGGAGE_PRICE
      ).toFixed(2);
      this.pricingDetails.push(
        `Carry-on Baggages (${this.selectedCarryOnBaggages}): ${carryOnBaggagePrice} zł`
      );
    }
    if (this.selectedCheckedBaggages) {
      const checkedBaggagePrice = (
        this.selectedCheckedBaggages * CHECKED_BAGGAGE_PRICE
      ).toFixed(2);
      this.pricingDetails.push(
        `Checked Baggages (${this.selectedCheckedBaggages}): ${checkedBaggagePrice} zł`
      );
    }
    if (this.selectedPriorityBoarding) {
      const priorityBoardingPrice = PRIORITY_BOARDING_PRICE.toFixed(2);
      this.pricingDetails.push(
        `Priority Boarding: ${priorityBoardingPrice} zł`
      );
    }
    if (this.selectedInsurance) {
      const insurancePrice = INSURANCE_PRICE.toFixed(2);
      this.pricingDetails.push(`Insurance: ${insurancePrice} zł`);
    }
  }
}
