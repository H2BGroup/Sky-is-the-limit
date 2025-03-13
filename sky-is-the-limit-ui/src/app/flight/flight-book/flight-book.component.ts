import { Component, inject, OnDestroy, OnInit } from '@angular/core';
import { FlightService } from '../flight.service';
import { Flight } from '../flight.model';
import { ActivatedRoute } from '@angular/router';
import { formatDateTime } from '../shared/formatDateTime';
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

@Component({
  selector: 'app-flight-book',
  imports: [ReactiveFormsModule],
  templateUrl: './flight-book.component.html',
  styleUrl: './flight-book.component.css',
})
export class FlightBookComponent implements OnInit, OnDestroy {
  private flightService = inject(FlightService);
  private activatedRoute = inject(ActivatedRoute);
  protected totalPrice: number = 0;
  private basePrice: number = 0;
  private formSubscription!: Subscription;

  private MAX_BAGGAGE: number = 2;
  private MAX_SEATS: number = 10;
  private FIRST_CLASS_PRICE_RATIO: number = 3;
  private CHECKED_BAGGAGE_PRICE: number = 200;
  private CARRY_ON_BAGGAGE_PRICE: number = 100;
  private PRIORITY_BOARDING_PRICE: number = 30;
  private INSURANCE_PRICE: number = 50;

  flight?: Flight = this.flightService.getFlight(
    this.activatedRoute.snapshot.paramMap.get('id')!
  );

  bookForm = new FormGroup(
    {
      classSelection: new FormGroup({
        firstClass: new FormControl(0, [Validators.min(0)]),
        economyClass: new FormControl(1, [Validators.min(0)]),
      }),
      baggageSelection: new FormGroup({
        carryOnBaggage: new FormControl(0, [Validators.min(0)]),
        checkedBaggage: new FormControl(0, [Validators.min(0)]),
      }),
      priorityBoarding: new FormControl(false, []),
      insurance: new FormControl(false, []),
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

  getFormattedDate(date: string): string {
    return formatDateTime(date);
  }

  OnBackToList() {
    this.flightService.backToList();
  }

  onProceed() {
    console.log(this.bookForm.value);
  }

  seatsValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const group = control as FormGroup;
      const firstClass = group.get('classSelection.firstClass')?.value;
      const economyClass = group.get('classSelection.economyClass')?.value;
      const totalSeats = firstClass + economyClass;

      if (totalSeats === 0) {
        return { minSeatsRequired: 'You must select at least one seat.' };
      }

      if (totalSeats > this.MAX_SEATS) {
        return { maxSeatsExceeded: 'You can select a maximum of 10 seats.' };
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
          classSelection.firstClass *
          this.basePrice *
          this.FIRST_CLASS_PRICE_RATIO;
      }
      if (classSelection && classSelection.economyClass) {
        this.totalPrice += classSelection.economyClass * this.basePrice;
      }
      if (baggageSelection && baggageSelection.carryOnBaggage) {
        this.totalPrice +=
          baggageSelection.carryOnBaggage * this.CARRY_ON_BAGGAGE_PRICE;
      }
      if (baggageSelection && baggageSelection.checkedBaggage) {
        this.totalPrice +=
          baggageSelection.checkedBaggage * this.CHECKED_BAGGAGE_PRICE;
      }

      if (priorityBoarding) {
        this.totalPrice += this.PRIORITY_BOARDING_PRICE;
      }
      if (insurance) {
        this.totalPrice += this.INSURANCE_PRICE;
      }
    }
  }
}
