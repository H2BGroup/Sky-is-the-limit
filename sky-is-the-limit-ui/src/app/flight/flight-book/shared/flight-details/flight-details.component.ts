import { Component, Inject, inject, Input, OnInit } from '@angular/core';
import { Flight } from '../../../flight.model';
import { formatDateTime } from '../../../shared/formatDateTime';
import { FlightService } from '../../../flight.service';
import { ActivatedRoute } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-flight-details',
  imports: [CommonModule],
  templateUrl: './flight-details.component.html',
  styleUrl: './flight-details.component.css',
})
export class FlightDetailsComponent implements OnInit {
  flightService = inject(FlightService);
  activatedRoute = inject(ActivatedRoute);

  flight?: Flight;

  ngOnInit(): void {
    const id = this.activatedRoute.snapshot.paramMap.get('id');
    if (id) {
      this.flightService.getFlight(id).subscribe((flight) => {
        this.flight = flight;
      });
    }
  }

  getFormattedDate(date: string): string {
    return formatDateTime(date);
  }
}
