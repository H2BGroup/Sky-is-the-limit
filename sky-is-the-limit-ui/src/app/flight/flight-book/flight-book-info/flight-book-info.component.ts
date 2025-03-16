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
import { FlightBookService } from '../flight-book.service';

import { PersonalDetails } from '../personal-details.model';

@Component({
  selector: 'app-flight-book-info',
  imports: [ReactiveFormsModule],
  templateUrl: './flight-book-info.component.html',
  styleUrl: './flight-book-info.component.css',
})
export class FlightBookInfoComponent {
  private flightService = inject(FlightService);
  private flightBookService = inject(FlightBookService);
  private activatedRoute = inject(ActivatedRoute);

  pricingDetails: string[] = [];

  flight?: Flight = this.flightService.getFlight(
    this.activatedRoute.snapshot.paramMap.get('id')!
  );

  personalInfoForm = new FormGroup({
    nameAndSurname: new FormControl(
      this.flightBookService.getPersonalDetails()?.nameAndSurname ?? '',
      [Validators.required]
    ),
    dateOfBirth: new FormControl(
      this.flightBookService.getPersonalDetails()?.dateOfBirth ?? '',
      [Validators.required, Validators.pattern(/^\d{4}-\d{2}-\d{2}$/)]
    ),
    idDocumentNumber: new FormControl(
      this.flightBookService.getPersonalDetails()?.idDocumentNumber ?? '',
      [Validators.required, Validators.pattern(/^[A-Z0-9]{5,15}$/)]
    ),
    email: new FormControl(
      this.flightBookService.getPersonalDetails()?.email ?? '',
      [Validators.required, Validators.email]
    ),
    phone: new FormControl(
      this.flightBookService.getPersonalDetails()?.phone ?? '',
      [Validators.required, Validators.pattern(/^\+?[0-9\s\-]{7,15}$/)]
    ),
    address: new FormControl(
      this.flightBookService.getPersonalDetails()?.address ?? '',
      [Validators.required]
    ),
    country: new FormControl(
      this.flightBookService.getPersonalDetails()?.country ?? '',
      [Validators.required]
    ),
    city: new FormControl(
      this.flightBookService.getPersonalDetails()?.city ?? '',
      [Validators.required]
    ),
    postalCode: new FormControl(
      this.flightBookService.getPersonalDetails()?.postalCode ?? '',
      [Validators.required, Validators.pattern(/^\d{2}-\d{3}$/)]
    ),
  });

  OnBackToDetails() {
    this.setPersonalDetailsFromForm();
    this.flightService.toFlightDetails(this.flight!.id);
  }

  onViewSummary() {
    this.setPersonalDetailsFromForm();
    this.flightService.toBookingSummary(this.flight!.id);
  }

  setPersonalDetailsFromForm() {
    if (this.personalInfoForm) {
      let personalDetails: PersonalDetails = {
        nameAndSurname:
          this.personalInfoForm.get('nameAndSurname')?.value ?? '',
        dateOfBirth: this.personalInfoForm.get('dateOfBirth')?.value ?? '',
        idDocumentNumber:
          this.personalInfoForm.get('idDocumentNumber')?.value ?? '',
        email: this.personalInfoForm.get('email')?.value ?? '',
        phone: this.personalInfoForm.get('phone')?.value ?? '',
        address: this.personalInfoForm.get('address')?.value ?? '',
        city: this.personalInfoForm.get('city')?.value ?? '',
        country: this.personalInfoForm.get('country')?.value ?? '',
        postalCode: this.personalInfoForm.get('postalCode')?.value ?? '',
      };
      this.flightBookService.setPersonalDetails(personalDetails);
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
}
