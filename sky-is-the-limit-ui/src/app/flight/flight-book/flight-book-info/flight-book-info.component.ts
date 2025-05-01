import { Component, inject, OnDestroy, OnInit } from '@angular/core';
import { FlightService } from '../../flight.service';
import { Flight } from '../../flight.model';
import { ActivatedRoute } from '@angular/router';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';

import { Subscription } from 'rxjs';

@Component({
  selector: 'app-flight-book-info',
  imports: [ReactiveFormsModule],
  templateUrl: './flight-book-info.component.html',
  styleUrl: './flight-book-info.component.css',
})
export class FlightBookInfoComponent implements OnInit, OnDestroy {
  private flightService = inject(FlightService);
  private activatedRoute = inject(ActivatedRoute);

  personalInfoForm!: FormGroup;
  private formSubscription!: Subscription;

  pricingDetails: string[] = [];

  flight?: Flight;

  ngOnInit(): void {
    const id = this.activatedRoute.snapshot.paramMap.get('id');
    if (id) {
      this.flightService.getFlight(id).subscribe((flight) => {
        this.flight = flight;
      });
    }

    this.setPersonalInfoForm();

    this.formSubscription = this.personalInfoForm.valueChanges.subscribe(
      (values) => {
        sessionStorage.setItem('personalInfoFormData', JSON.stringify(values));
      }
    );
  }

  ngOnDestroy(): void {
    if (this.formSubscription) {
      this.formSubscription.unsubscribe();
    }
  }

  setPersonalInfoForm() {
    const savedPersonalInfoForm = sessionStorage.getItem('personalInfoFormData')
      ? JSON.parse(sessionStorage.getItem('personalInfoFormData')!)
      : {};
    this.personalInfoForm = new FormGroup({
      nameAndSurname: new FormControl(
        savedPersonalInfoForm?.nameAndSurname ?? '',
        [Validators.required]
      ),
      dateOfBirth: new FormControl(savedPersonalInfoForm?.dateOfBirth ?? '', [
        Validators.required,
        Validators.pattern(/^\d{4}-\d{2}-\d{2}$/),
      ]),
      idDocumentNumber: new FormControl(
        savedPersonalInfoForm?.idDocumentNumber ?? '',
        [Validators.required, Validators.pattern(/^[A-Z0-9]{5,15}$/)]
      ),
      email: new FormControl(savedPersonalInfoForm?.email ?? '', [
        Validators.required,
        Validators.email,
      ]),
      phone: new FormControl(savedPersonalInfoForm?.phone ?? '', [
        Validators.required,
        Validators.pattern(/^\+?[0-9\s\-]{7,15}$/),
      ]),
      address: new FormControl(savedPersonalInfoForm?.address ?? '', [
        Validators.required,
      ]),
      country: new FormControl(savedPersonalInfoForm?.country ?? '', [
        Validators.required,
      ]),
      city: new FormControl(savedPersonalInfoForm?.city ?? '', [
        Validators.required,
      ]),
      postalCode: new FormControl(savedPersonalInfoForm?.postalCode ?? '', [
        Validators.required,
        Validators.pattern(/^\d{2}-\d{3}$/),
      ]),
    });
  }

  OnBackToDetails() {
    this.flightService.toFlightDetails(this.flight!.id);
  }

  onViewSummary() {
    sessionStorage.setItem(
      'personalInfoFormData',
      JSON.stringify(this.personalInfoForm.value)
    );
    this.flightService.toBookingSummary(this.flight!.id);
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
