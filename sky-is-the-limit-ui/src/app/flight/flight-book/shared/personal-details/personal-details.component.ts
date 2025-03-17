import { Component } from '@angular/core';

@Component({
  selector: 'app-personal-details',
  imports: [],
  templateUrl: './personal-details.component.html',
  styleUrl: './personal-details.component.css',
})
export class PersonalDetailsComponent {
  protected providedNameAndSurname?: string;
  protected providedDateOfBirth?: string;
  protected providedIdDocumentNumber?: string;
  protected providedEmail?: string;
  protected providedPhone?: string;
  protected providedAddress?: string;
  protected providedCity?: string;
  protected providedCountry?: string;
  protected providedPostalCode?: string;

  ngOnInit(): void {
    this.setProvidedPersonalDetails();
  }

  setProvidedPersonalDetails() {
    const savedPersonalInfoForm = sessionStorage.getItem('personalInfoFormData')
      ? JSON.parse(sessionStorage.getItem('personalInfoFormData')!)
      : {};

    this.providedNameAndSurname = savedPersonalInfoForm.nameAndSurname;
    this.providedDateOfBirth = savedPersonalInfoForm.dateOfBirth;
    this.providedIdDocumentNumber = savedPersonalInfoForm.idDocumentNumber;
    this.providedEmail = savedPersonalInfoForm.email;
    this.providedPhone = savedPersonalInfoForm.phone;
    this.providedAddress = savedPersonalInfoForm.address;
    this.providedCity = savedPersonalInfoForm.city;
    this.providedCountry = savedPersonalInfoForm.country;
    this.providedPostalCode = savedPersonalInfoForm.postalCode;
  }
}
