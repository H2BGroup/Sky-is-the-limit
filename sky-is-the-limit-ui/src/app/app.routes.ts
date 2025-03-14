import { Routes } from '@angular/router';
import { FlightBookDetailsComponent } from './flight/flight-book/flight-book-details/flight-book-details.component';
import { FlightsMainPageComponent } from './flight/flights-main-page/flights-main-page.component';
import { FlightBookInfoComponent } from './flight/flight-book/flight-book-info/flight-book-info.component';

export const routes: Routes = [
  {
    path: '',
    component: FlightsMainPageComponent,
    title: 'Sky is the limit',
  },
  {
    path: 'flights',
    component: FlightsMainPageComponent,
    title: 'Sky is the limit',
  },
  {
    path: 'flights/:id/book',
    children: [
      { path: 'info', component: FlightBookInfoComponent },
      { path: 'details', component: FlightBookDetailsComponent },
    ],
  },
];
