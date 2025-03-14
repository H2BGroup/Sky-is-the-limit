import { Component, inject } from '@angular/core';
import { FlightService } from '../../flight.service';
import { Flight } from '../../flight.model';
import { ActivatedRoute } from '@angular/router';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';

@Component({
  selector: 'app-flight-book-info',
  imports: [ReactiveFormsModule],
  templateUrl: './flight-book-info.component.html',
  styleUrl: './flight-book-info.component.css',
})
export class FlightBookInfoComponent {
  private flightService = inject(FlightService);
  private activatedRoute = inject(ActivatedRoute);

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

  OnBackToDetails() {
    this.flightService.toFlightDetails(this.flight!.id);
  }

  onViewSummary(flightId: string) {}
}
