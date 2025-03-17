import { Component, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HeaderComponent } from './header/header.component';
import { LoginComponent } from './login/login.component';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, HeaderComponent, LoginComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
})
export class AppComponent implements OnInit {
  protected loggedIn: boolean = false;

  ngOnInit(): void {
    this.loggedIn = sessionStorage.getItem('isLoggedIn') === 'true';
  }

  checkIfLoggedIn(isLogged: boolean) {
    this.loggedIn = isLogged;
  }
}
