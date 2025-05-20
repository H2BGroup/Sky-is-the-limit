import { Component, EventEmitter, inject, Output } from '@angular/core';
import { Router } from '@angular/router';
import { clearFormData } from '../flight/shared/clearFormData';

@Component({
  selector: 'app-header',
  imports: [],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css',
})
export class HeaderComponent {
  @Output() loggedIn = new EventEmitter<boolean>();
  private router = inject(Router);
  logOut() {
    sessionStorage.removeItem('isLoggedIn');
    this.loggedIn.emit(false);
    this.router.navigate(['/flights']);
    clearFormData();
  }
}
