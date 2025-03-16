import { Component, inject, OnDestroy, OnInit } from '@angular/core';
import { FlightService } from '../../flight.service';
import { Flight } from '../../flight.model';
import { ActivatedRoute } from '@angular/router';
import {
  AbstractControl,
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  ValidationErrors,
  ValidatorFn,
  Validators,
} from '@angular/forms';
import { Subscription } from 'rxjs';
import { FlightBookService } from '../flight-book.service';
import { BookingDetails } from '../booking-details.model';
import {
  CARRY_ON_BAGGAGE_PRICE,
  CHECKED_BAGGAGE_PRICE,
  FIRST_CLASS_PRICE_RATIO,
  INSURANCE_PRICE,
  PRIORITY_BOARDING_PRICE,
} from '../price-constants';
import { FlightDetailsComponent } from '../shared/flight-details/flight-details.component';

@Component({
  selector: 'app-flight-book-details',
  imports: [ReactiveFormsModule, FlightDetailsComponent],
  templateUrl: './flight-book-details.component.html',
  styleUrl: './flight-book-details.component.css',
})
export class FlightBookDetailsComponent implements OnInit, OnDestroy {
  private flightService = inject(FlightService);
  private activatedRoute = inject(ActivatedRoute);
  private flightBookService = inject(FlightBookService);
  protected totalPrice: number = 0;
  private basePrice: number = 0;
  private formSubscription!: Subscription;

  private MAX_BAGGAGE: number = 2;
  private MAX_SEATS: number = 10;

  flight?: Flight = this.flightService.getFlight(
    this.activatedRoute.snapshot.paramMap.get('id')!
  );

  bookForm = new FormGroup(
    {
      classSelection: new FormGroup({
        firstClass: new FormControl(
          this.flightBookService.getBookingDetails()?.firstClassSeats ?? 0,
          [Validators.min(0)]
        ),
        economyClass: new FormControl(
          this.flightBookService.getBookingDetails()?.economyClassSeats ?? 1,
          [Validators.min(0)]
        ),
      }),
      baggageSelection: new FormGroup({
        carryOnBaggage: new FormControl(
          this.flightBookService.getBookingDetails()?.carryOnBaggages ?? 0,
          [Validators.min(0)]
        ),
        checkedBaggage: new FormControl(
          this.flightBookService.getBookingDetails()?.checkedBaggages ?? 0,
          [Validators.min(0)]
        ),
      }),
      priorityBoarding: new FormControl(
        this.flightBookService.getBookingDetails()?.priorityBoarding ?? false,
        []
      ),
      insurance: new FormControl(
        this.flightBookService.getBookingDetails()?.insurance ?? false,
        []
      ),
    },
    { validators: this.seatsValidator() }
  );

  ngOnInit(): void {
    if (this.flight) {
      this.basePrice = this.flight.price;
    }
    this.calculateTotalPrice();
    this.formSubscription = this.bookForm.valueChanges.subscribe(() => {
      this.calculateTotalPrice();
    });
  }

  ngOnDestroy(): void {
    if (this.formSubscription) {
      this.formSubscription.unsubscribe();
    }
  }

  OnBackToList() {
    this.flightService.toFlightList();
  }

  onProceedWithBooking() {
    if (this.bookForm) {
      let bookingDetails: BookingDetails = {
        firstClassSeats: this.bookForm.get('classSelection.firstClass')?.value!,
        economyClassSeats: this.bookForm.get('classSelection.economyClass')
          ?.value!,
        carryOnBaggages: this.bookForm.get('baggageSelection.carryOnBaggage')
          ?.value!,
        checkedBaggages: this.bookForm.get('baggageSelection.checkedBaggage')
          ?.value!,
        priorityBoarding: this.bookForm.get('priorityBoarding')?.value!,
        insurance: this.bookForm.get('insurance')?.value!,
        price: this.totalPrice,
      };
      this.flightBookService.setBookingDetails(bookingDetails);
    }
    this.flightService.toPersonalDetailsForm(this.flight!.id);
  }

  seatsValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const group = control as FormGroup;
      const firstClass = group.get('classSelection.firstClass')?.value;
      const economyClass = group.get('classSelection.economyClass')?.value;
      const totalSeats = firstClass + economyClass;

      if (firstClass > this.flight!.seatsFirstClass) {
        return {
          notEnoughFirstClassSeatsLeft: `There is/are only ${
            this.flight!.seatsFirstClass
          } first class seats left.`,
        };
      }

      if (economyClass > this.flight!.seatsEconomy) {
        return {
          notEnoughEconomySeatsLeft: `There is/are only ${
            this.flight!.seatsEconomy
          } economy class seats left.`,
        };
      }

      if (totalSeats === 0) {
        return { minSeatsRequired: 'You must select at least one seat.' };
      }

      if (totalSeats > this.MAX_SEATS) {
        return {
          maxSeatsExceeded: `You can select a maximum of ${this.MAX_SEATS} seats.`,
        };
      }

      const carryOnBaggage = group.get(
        'baggageSelection.carryOnBaggage'
      )?.value;
      const checkedBaggage = group.get(
        'baggageSelection.checkedBaggage'
      )?.value;

      if (carryOnBaggage > totalSeats * this.MAX_BAGGAGE) {
        return {
          maxCarryOnBaggagesExceeded:
            'You can select a maximum of 2 carry on baggages per person.',
        };
      }

      if (checkedBaggage > totalSeats * this.MAX_BAGGAGE) {
        return {
          maxCheckedBaggagesExceeded:
            'You can select a maximum of 2 checked baggages per person.',
        };
      }

      return null;
    };
  }

  calculateTotalPrice(): void {
    this.totalPrice = 0;

    if (this.flight) {
      const classSelection = this.bookForm.value.classSelection;
      const baggageSelection = this.bookForm.value.baggageSelection;
      const priorityBoarding = this.bookForm.value.priorityBoarding;
      const insurance = this.bookForm.value.insurance;

      if (classSelection && classSelection.firstClass) {
        this.totalPrice +=
          classSelection.firstClass * this.basePrice * FIRST_CLASS_PRICE_RATIO;
      }
      if (classSelection && classSelection.economyClass) {
        this.totalPrice += classSelection.economyClass * this.basePrice;
      }
      if (baggageSelection && baggageSelection.carryOnBaggage) {
        this.totalPrice +=
          baggageSelection.carryOnBaggage * CARRY_ON_BAGGAGE_PRICE;
      }
      if (baggageSelection && baggageSelection.checkedBaggage) {
        this.totalPrice +=
          baggageSelection.checkedBaggage * CHECKED_BAGGAGE_PRICE;
      }

      if (priorityBoarding) {
        this.totalPrice += PRIORITY_BOARDING_PRICE;
      }
      if (insurance) {
        this.totalPrice += INSURANCE_PRICE;
      }
    }
  }
}
