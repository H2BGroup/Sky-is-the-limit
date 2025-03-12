import { Component, inject, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
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

  filtersForm = new FormGroup({
    departure: new FormControl('', []),
    arrival: new FormControl('', []),
    departureDate: new FormControl('', [
      Validators.pattern(/^\d{2}\/\d{2}\/\d{4}$/),
    ]),
    arrivalDate: new FormControl('', [
      Validators.pattern(/^\d{2}\/\d{2}\/\d{4}$/),
    ]),
    passengers: new FormControl(1, [Validators.min(1), Validators.max(10)]),
    price: new FormControl(1000, [Validators.min(0), Validators.max(2000)]),
  });

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
      //console.log('Flights: ', this.flightService.getFlights.length);
    } else {
      console.log('Form is invalid');
    }
  }

  getFilterValues() {
    return {
      departure: this.filtersForm.get('departure')?.value || '',
      arrival: this.filtersForm.get('arrival')?.value || '',
      departureDate: this.filtersForm.get('departureDate')?.value || '',
      arrivalDate: this.filtersForm.get('arrivalDate')?.value || '',
      passengers: this.filtersForm.get('passengers')?.value || 1,
      price: this.filtersForm.get('price')?.value || 1000,
    };
  }
}
