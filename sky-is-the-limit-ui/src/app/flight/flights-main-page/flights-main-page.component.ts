import { Component } from '@angular/core';
import { FlightSearchFilterComponent } from '../flight-search-filter/flight-search-filter.component';
import { FlightListComponent } from '../flight-list/flight-list.component';

@Component({
  selector: 'app-flights-main-page',
  imports: [FlightSearchFilterComponent, FlightListComponent],
  templateUrl: './flights-main-page.component.html',
  styleUrl: './flights-main-page.component.css',
})
export class FlightsMainPageComponent {}
