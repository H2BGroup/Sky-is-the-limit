import { Component, inject } from '@angular/core';
import { FlightBookService } from '../../flight-book.service';

@Component({
  selector: 'app-personal-details',
  imports: [],
  templateUrl: './personal-details.component.html',
  styleUrl: './personal-details.component.css',
})
export class PersonalDetailsComponent {
  private flightBookService = inject(FlightBookService);

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
    let personalDetails = this.flightBookService.getPersonalDetails();
    this.providedNameAndSurname = personalDetails.nameAndSurname;
    this.providedDateOfBirth = personalDetails.dateOfBirth;
    this.providedIdDocumentNumber = personalDetails.idDocumentNumber;
    this.providedEmail = personalDetails.email;
    this.providedPhone = personalDetails.phone;
    this.providedAddress = personalDetails.address;
    this.providedCity = personalDetails.city;
    this.providedCountry = personalDetails.country;
    this.providedPostalCode = personalDetails.postalCode;
  }
}
