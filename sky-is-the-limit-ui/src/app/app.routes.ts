import { Routes } from '@angular/router';
import { FlightBookComponent } from './flight/flight-book/flight-book.component';
import { FlightsMainPageComponent } from './flight/flights-main-page/flights-main-page.component';

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
    component: FlightBookComponent,
    title: 'Sky is the limit',
  },
];
