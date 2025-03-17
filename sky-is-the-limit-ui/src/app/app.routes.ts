import { Routes } from '@angular/router';
import { FlightBookDetailsComponent } from './flight/flight-book/flight-book-details/flight-book-details.component';
import { FlightsMainPageComponent } from './flight/flights-main-page/flights-main-page.component';
import { FlightBookInfoComponent } from './flight/flight-book/flight-book-info/flight-book-info.component';
import { FlightBookSummaryComponent } from './flight/flight-book/flight-book-summary/flight-book-summary.component';
import { FlightBookConfirmComponent } from './flight/flight-book/flight-book-confirm/flight-book-confirm.component';

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
      {
        path: 'info',
        component: FlightBookInfoComponent,
        title: 'Sky is the limit - flight details',
      },
      {
        path: 'details',
        component: FlightBookDetailsComponent,
        title: 'Sky is the limit - personal details',
      },
      {
        path: 'summary',
        component: FlightBookSummaryComponent,
        title: 'Sky is the limit - summary',
      },
      {
        path: 'confirm',
        component: FlightBookConfirmComponent,
        title: 'Sky is the limit - confirmation',
      },
    ],
  },
];
