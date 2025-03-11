import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-search-filter',
  imports: [ReactiveFormsModule],
  templateUrl: './search-filter.component.html',
  styleUrl: './search-filter.component.css',
})
export class SearchFilterComponent implements OnInit {
  filtersForm = new FormGroup({
    departure: new FormControl('', []),
    arrival: new FormControl('', []),
    price: new FormControl(1000, [Validators.min(0), Validators.max(2000)]),
  });

  ngOnInit(): void {
    const priceInput = document.getElementById('price') as HTMLInputElement;
    const priceValue = document.getElementById(
      'price-value'
    ) as HTMLSpanElement;

    priceValue.textContent = priceInput.value;

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
