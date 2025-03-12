import { Component, EventEmitter, Output } from '@angular/core';

@Component({
  selector: 'app-header',
  imports: [],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css',
})
export class HeaderComponent {
  @Output() loggedIn = new EventEmitter<boolean>();

  logOut() {
    localStorage.removeItem('isLoggedIn');
    this.loggedIn.emit(false);
  }
}
