import { Component, EventEmitter, inject, Output } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms';
import { LoginService } from './login.service';

@Component({
  selector: 'app-login',
  imports: [ReactiveFormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css',
})
export class LoginComponent {
  @Output() loggedIn = new EventEmitter<boolean>();

  private loginService = inject(LoginService);

  loginForm = new FormGroup({
    username: new FormControl('', [
      Validators.required,
      Validators.minLength(3),
    ]),
    password: new FormControl('', [
      Validators.required,
      Validators.minLength(6),
    ]),
  });

  logIn() {
    if (this.loginForm.valid) {
      this.loginService
        .login({
          login: this.loginForm.value.username!,
          password: this.loginForm.value.password!,
        })
        .subscribe({
          next: (response) => {
            sessionStorage.setItem('isLoggedIn', 'true');
            sessionStorage.setItem('userId', response.id);
            this.loggedIn.emit(true);
          },
          error: () => {
            this.loginForm.setErrors({ invalidLogin: true });
          },
        });
    }
  }
}
