import { Component, inject, OnInit } from '@angular/core';
import {
  AbstractControl,
  FormControl,
  FormGroup,
  ValidationErrors,
  ValidatorFn,
  Validators,
} from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms';
import { FlightService } from '../flight.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-flight-search-filter',
  imports: [ReactiveFormsModule],
  templateUrl: './flight-search-filter.component.html',
  styleUrl: './flight-search-filter.component.css',
})
export class FlightSearchFilterComponent implements OnInit {
  flightService = inject(FlightService);

  filtersForm = new FormGroup(
    {
      departure: new FormControl('', []),
      arrival: new FormControl('', []),
      fromDate: new FormControl('', [
        Validators.pattern(/^\d{4}-\d{2}-\d{2}$/),
      ]),
      toDate: new FormControl('', [Validators.pattern(/^\d{4}-\d{2}-\d{2}$/)]),
      passengers: new FormControl(1, [Validators.min(1), Validators.max(10)]),
      price: new FormControl(1000, [Validators.min(0), Validators.max(2000)]),
    },
    { validators: this.datesValidator() }
  );

  constructor(private router: Router) {}

  ngOnInit(): void {
    this.setInitialPriceValue();
  }

  setInitialPriceValue() {
    const priceInput = document.getElementById('price') as HTMLInputElement;
    const priceValue = document.getElementById(
      'price-value'
    ) as HTMLSpanElement;

    priceValue.textContent = this.filtersForm.get('price')
      ?.value as unknown as string;

    priceInput.addEventListener('input', function () {
      priceValue.textContent = priceInput.value;
    });
  }

  onSearch() {
    if (this.filtersForm.valid) {
      const filters = this.getFilterValues();
      this.flightService.filterFlights(filters);
      this.router.navigate([], { queryParamsHandling: 'merge' }).then(() => {
        this.router.navigateByUrl(this.router.url);
      });
    } else {
      console.log('Form is invalid');
    }
  }

  getFilterValues() {
    return {
      departure: this.filtersForm.get('departure')?.value || '',
      arrival: this.filtersForm.get('arrival')?.value || '',
      fromDate: this.filtersForm.get('fromDate')?.value || '',
      toDate: this.filtersForm.get('toDate')?.value || '',
      passengers: this.filtersForm.get('passengers')?.value || 1,
      price: this.filtersForm.get('price')?.value || 1000,
    };
  }

  datesValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const group = control as FormGroup;
      const fromDate = group.get('fromDate')?.value;
      const toDate = group.get('toDate')?.value;

      if (toDate && fromDate && toDate <= fromDate) {
        return { invalidDates: 'To date must be greater than from date.' };
      }

      return null;
    };
  }
}
