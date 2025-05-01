import { Component, inject, OnInit } from '@angular/core';
import { Flight } from '../flight.model';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatTooltipModule } from '@angular/material/tooltip';
import { FlightService } from '../flight.service';
import { Observable } from 'rxjs';
import { CommonModule } from '@angular/common';
import { formatDateTime } from '../shared/formatDateTime';
import { MatPaginator, PageEvent } from '@angular/material/paginator';

@Component({
  selector: 'app-flight-list',
  imports: [MatExpansionModule, MatTooltipModule, CommonModule, MatPaginator],
  templateUrl: './flight-list.component.html',
  styleUrl: './flight-list.component.css',
})
export class FlightListComponent implements OnInit {
  private flightService = inject(FlightService);
  private currentPage = 0;

  protected pageSize = 10;
  protected flights$: Observable<Flight[]> =
    this.flightService.filteredFlights$;
  protected paginatedFlights: Flight[] = [];

  ngOnInit() {
    this.flightService.fetchAndStoreFlights();

    this.flights$.subscribe((flights) => {
      this.updatePaginatedFlights(flights);
    });
  }

  getFormattedDate(date: string): string {
    return formatDateTime(date);
  }

  createFlightSummary(flight: Flight): string {
    const formattedPrice = flight.price.toFixed(2);
    return `✈️ ${flight.departure} → ${flight.arrival} from just ${formattedPrice} zł`;
  }

  onBookFlight(flightId: string) {
    this.flightService.toFlightDetails(flightId);
  }

  onPageChange(event: PageEvent) {
    this.pageSize = event.pageSize;
    this.currentPage = event.pageIndex;

    this.flights$.subscribe((flights) => {
      this.updatePaginatedFlights(flights);
    });
  }

  updatePaginatedFlights(flights: Flight[]) {
    const startIndex = this.currentPage * this.pageSize;
    const endIndex = startIndex + this.pageSize;
    this.paginatedFlights = flights.slice(startIndex, endIndex);
  }
}
