import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HeaderComponent } from './header/header.component';
import { FlightSearchFilterComponent } from './flight/flight-search-filter/flight-search-filter.component';
import { LoginComponent } from './login/login.component';
import { FlightListComponent } from './flight/flight-list/flight-list.component';

@Component({
  selector: 'app-root',
  imports: [
    RouterOutlet,
    HeaderComponent,
    FlightSearchFilterComponent,
    LoginComponent,
    FlightListComponent,
  ],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
})
export class AppComponent {
  protected loggedIn: boolean = false;

  checkIfLoggedIn(isLogged: boolean) {
    this.loggedIn = isLogged;
    console.log(isLogged);
  }
}
