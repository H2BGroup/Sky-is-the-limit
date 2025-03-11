import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-flight-search-filter',
  imports: [ReactiveFormsModule],
  templateUrl: './flight-search-filter.component.html',
  styleUrl: './flight-search-filter.component.css',
})
export class FlightSearchFilterComponent implements OnInit {
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
      const formValues = this.filtersForm.value;
      console.log('Searching for flights with values:', formValues);
    } else {
      console.log('Form is invalid');
    }
  }
}
