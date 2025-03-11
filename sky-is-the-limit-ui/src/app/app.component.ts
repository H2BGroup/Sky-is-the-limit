import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HeaderComponent } from './header/header.component';
import { SearchFilterComponent } from './search-filter/search-filter.component';
import { LoginComponent } from './login/login.component';

@Component({
  selector: 'app-root',
  imports: [
    RouterOutlet,
    HeaderComponent,
    SearchFilterComponent,
    LoginComponent,
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
