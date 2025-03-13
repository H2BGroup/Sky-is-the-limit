import { Component, inject, OnInit } from '@angular/core';
import { FlightService } from '../flight.service';
import { Flight } from '../flight.model';
import { ActivatedRoute } from '@angular/router';
import { formatDateTime } from '../shared/formatDateTime';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';

@Component({
  selector: 'app-flight-book',
  imports: [ReactiveFormsModule],
  templateUrl: './flight-book.component.html',
  styleUrl: './flight-book.component.css',
})
export class FlightBookComponent implements OnInit {
  private flightService = inject(FlightService);
  private activatedRoute = inject(ActivatedRoute);
  protected totalPrice: number = 0;

  flight?: Flight = this.flightService.getFlight(
    this.activatedRoute.snapshot.paramMap.get('id')!
  );

  bookForm = new FormGroup({
    classSelection: new FormGroup({
      firstClass: new FormControl(0, [Validators.min(0), Validators.max(5)]),
      economyClass: new FormControl(1, [Validators.min(0), Validators.max(5)]),
    }),
    baggageSelection: new FormGroup({
      carryOnBaggage: new FormControl(0, [
        Validators.min(0),
        Validators.max(2),
      ]),
      checkedBaggage: new FormControl(0, [
        Validators.min(0),
        Validators.max(2),
      ]),
    }),
    priorityBoarding: new FormControl(false, []),
    insurance: new FormControl(false, []),
  });

  ngOnInit(): void {
    if (this.flight) {
      this.totalPrice = this.flight.price;
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
}
