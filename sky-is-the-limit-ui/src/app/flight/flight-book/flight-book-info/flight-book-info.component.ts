import { Component, inject, OnInit } from '@angular/core';
import { FlightService } from '../../flight.service';
import { Flight } from '../../flight.model';
import { ActivatedRoute } from '@angular/router';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { FlightBookService } from '../flight-book.service';
import {
  CARRY_ON_BAGGAGE_PRICE,
  CHECKED_BAGGAGE_PRICE,
  FIRST_CLASS_PRICE_RATIO,
  INSURANCE_PRICE,
  PRIORITY_BOARDING_PRICE,
} from '../price-constants';

@Component({
  selector: 'app-flight-book-info',
  imports: [ReactiveFormsModule],
  templateUrl: './flight-book-info.component.html',
  styleUrl: './flight-book-info.component.css',
})
export class FlightBookInfoComponent implements OnInit {
  private flightService = inject(FlightService);
  private flightBookService = inject(FlightBookService);
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

  personalInfoForm = new FormGroup({
    nameAndSurname: new FormControl('', [Validators.required]),
    dateOfBirth: new FormControl('', [
      Validators.required,
      Validators.pattern(/^\d{4}-\d{2}-\d{2}$/),
    ]),
    idDocumentNumber: new FormControl('', [
      Validators.required,
      Validators.pattern(/^[A-Z0-9]{5,15}$/),
    ]),
    email: new FormControl('', [Validators.required, Validators.email]),
    phone: new FormControl('', [
      Validators.required,
      Validators.pattern(/^\+?[0-9\s\-]{7,15}$/),
    ]),
    address: new FormControl('', [Validators.required]),
    city: new FormControl('', [Validators.required]),
    country: new FormControl('', [Validators.required]),
    postalCode: new FormControl('', [
      Validators.required,
      Validators.pattern(/^\d{2}-\d{3}$/),
    ]),
  });

  ngOnInit(): void {
    this.setSelectedBookingDetails();
    this.showDetailedPrice();
  }

  setSelectedBookingDetails() {
    let bookingDetails = this.flightBookService.getBookingDetails();
    this.selectedFirstClassSeats = bookingDetails.firstClassSeats;
    this.selectedEconomyClassSeats = bookingDetails.economyClassSeats;
    this.selectedCarryOnBaggages = bookingDetails.carryOnBaggages;
    this.selectedCheckedBaggages = bookingDetails.checkedBaggages;
    this.selectedPriorityBoarding = bookingDetails.priorityBoarding;
    this.selectedInsurance = bookingDetails.insurance;
    this.totalPrice = bookingDetails.price;
  }

  OnBackToDetails() {
    this.flightService.toFlightDetails(this.flight!.id);
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

  everyFieldTouched() {
    if (
      this.personalInfoForm.get('nameAndSurname')?.touched &&
      this.personalInfoForm.get('dateOfBirth')?.touched &&
      this.personalInfoForm.get('idDocumentNumber')?.touched &&
      this.personalInfoForm.get('email')?.touched &&
      this.personalInfoForm.get('phone')?.touched &&
      this.personalInfoForm.get('address')?.touched &&
      this.personalInfoForm.get('city')?.touched &&
      this.personalInfoForm.get('country')?.touched &&
      this.personalInfoForm.get('postalCode')?.touched
    ) {
      return true;
    }
    return false;
  }

  onViewSummary(flightId: string) {
    this.flightService.toBookingSummary(flightId);
  }
}
