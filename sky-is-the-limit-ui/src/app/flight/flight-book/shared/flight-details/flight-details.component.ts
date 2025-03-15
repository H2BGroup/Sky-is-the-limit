import { Component, Inject, inject, Input } from '@angular/core';
import { Flight } from '../../../flight.model';
import { formatDateTime } from '../../../shared/formatDateTime';
import { FlightService } from '../../../flight.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-flight-details',
  imports: [],
  templateUrl: './flight-details.component.html',
  styleUrl: './flight-details.component.css',
})
export class FlightDetailsComponent {
  flightService = inject(FlightService);
  activatedRoute = inject(ActivatedRoute);

  flight?: Flight = this.flightService.getFlight(
    this.activatedRoute.snapshot.paramMap.get('id')!
  );

  getFormattedDate(date: string): string {
    return formatDateTime(date);
  }
}
