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
import { BookingDetails } from '../../booking-details.model';

@Component({
  selector: 'app-booking-details',
  imports: [CommonModule],
  templateUrl: './booking-details.component.html',
  styleUrl: './booking-details.component.css',
})
export class BookingDetailsComponent implements OnInit {
  private flightService = inject(FlightService);
  private activatedRoute = inject(ActivatedRoute);

  protected selectedBookingDetails?: BookingDetails;

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

  showDetailedPrice() {
    let basePrice = this.flight!.price;
    this.pricingDetails = [];

    let selectedFirstClassSeats = this.selectedBookingDetails?.firstClassSeats;
    if (selectedFirstClassSeats) {
      const firstClassPrice = (
        selectedFirstClassSeats *
        basePrice *
        FIRST_CLASS_PRICE_RATIO
      ).toFixed(2);
      this.pricingDetails.push(
        `First Class Seats (${selectedFirstClassSeats}): ${firstClassPrice} zł`
      );
    }

    let selectedEconomyClassSeats =
      this.selectedBookingDetails?.economyClassSeats;
    if (selectedEconomyClassSeats) {
      const economyClassPrice = (selectedEconomyClassSeats * basePrice).toFixed(
        2
      );
      this.pricingDetails.push(
        `Economy Class Seats (${selectedEconomyClassSeats}): ${economyClassPrice} zł`
      );
    }

    let selectedCarryOnBaggages = this.selectedBookingDetails?.carryOnBaggages;
    if (selectedCarryOnBaggages) {
      const carryOnBaggagePrice = (
        selectedCarryOnBaggages * CARRY_ON_BAGGAGE_PRICE
      ).toFixed(2);
      this.pricingDetails.push(
        `Carry-on Baggages (${selectedCarryOnBaggages}): ${carryOnBaggagePrice} zł`
      );
    }

    let selectedCheckedBaggages = this.selectedBookingDetails?.checkedBaggages;
    if (selectedCheckedBaggages) {
      const checkedBaggagePrice = (
        selectedCheckedBaggages * CHECKED_BAGGAGE_PRICE
      ).toFixed(2);
      this.pricingDetails.push(
        `Checked Baggages (${selectedCheckedBaggages}): ${checkedBaggagePrice} zł`
      );
    }

    let selectedPriorityBoarding =
      this.selectedBookingDetails?.priorityBoarding;
    if (selectedPriorityBoarding) {
      const priorityBoardingPrice = PRIORITY_BOARDING_PRICE.toFixed(2);
      this.pricingDetails.push(
        `Priority Boarding: ${priorityBoardingPrice} zł`
      );
    }

    let selectedInsurance = this.selectedBookingDetails?.insurance;
    if (selectedInsurance) {
      const insurancePrice = INSURANCE_PRICE.toFixed(2);
      this.pricingDetails.push(`Insurance: ${insurancePrice} zł`);
    }
  }
}
