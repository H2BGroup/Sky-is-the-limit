import { Routes } from '@angular/router';
import { FlightBookDetailsComponent } from './flight/flight-book/flight-book-details/flight-book-details.component';
import { FlightsMainPageComponent } from './flight/flights-main-page/flights-main-page.component';
import { FlightBookSummaryComponent } from './flight/flight-book/flight-book-summary/flight-book-summary.component';
import { FlightBookConfirmComponent } from './flight/flight-book/flight-book-confirm/flight-book-confirm.component';
import { NotFoundComponent } from './not-found/not-found.component';

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
        path: 'details',
        component: FlightBookDetailsComponent,
        title: 'Sky is the limit - Details',
      },
      {
        path: 'summary',
        component: FlightBookSummaryComponent,
        title: 'Sky is the limit - Summary',
      },
      {
        path: 'confirm',
        component: FlightBookConfirmComponent,
        title: 'Sky is the limit - Confirmation',
      },
    ],
  },
  {
    path: '**',
    component: NotFoundComponent,
    title: 'Sky is the limit - Page Not Found',
  },
];
