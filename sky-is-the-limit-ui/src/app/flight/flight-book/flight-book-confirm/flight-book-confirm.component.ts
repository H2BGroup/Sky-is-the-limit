import { Component, inject, OnDestroy, OnInit } from '@angular/core';
import { FlightService } from '../../flight.service';
import Swal from 'sweetalert2';
import { clearFormData } from '../../shared/clearFormData';
import { v4 as uuidv4 } from 'uuid';
import { PaymentService } from '../payment.service';

@Component({
  selector: 'app-flight-book-confirm',
  imports: [],
  templateUrl: './flight-book-confirm.component.html',
  styleUrl: './flight-book-confirm.component.css',
})
export class FlightBookConfirmComponent implements OnInit, OnDestroy {
  private flightService = inject(FlightService);
  private paymentService = inject(PaymentService);

  private readonly SECONDS = 60;
  private remainingSeconds: number = this.SECONDS;
  private timerInterval: any;
  private paymentId: string = uuidv4();

  private timeoutErrorMessage: string =
    'Payment could not be completed because the time limit has been exceeded.';
  private paymentErrorMessage: string =
    'Something went wrong with the payment process. Please try again.';

  protected displayTime: string = '01:00';
  protected isLoading = false;

  ngOnInit(): void {
    this.startTimer();
  }

  ngOnDestroy(): void {
    clearInterval(this.timerInterval);
  }

  onPayment() {
    this.isLoading = true;

    this.paymentService.createPayment(this.paymentId, {}).subscribe({
      // TODO: add payment data
      next: () => {
        this.checkPaymentStatus(this.paymentId);
      },
      error: () => {
        this.isLoading = false;
        this.showPaymentErrorAlert(this.paymentErrorMessage);
      },
    });
  }

  private checkPaymentStatus(paymentId: string) {
    const interval = setInterval(() => {
      this.paymentService.getPayment(paymentId).subscribe({
        next: (payment) => {
          if (payment.status === 'OK') {
            clearInterval(interval);
            this.isLoading = false;
            this.showPaymentSuccessAlert();
          }
        },
        error: () => {
          clearInterval(interval);
          this.isLoading = false;
          this.showPaymentErrorAlert(this.paymentErrorMessage);
        },
      });
    }, 1000);
  }

  private showPaymentSuccessAlert(): void {
    Swal.fire({
      title: 'Payment Accepted!',
      text: 'Return to the homepage?',
      icon: 'success',
      showCancelButton: false,
      allowOutsideClick: false,
      allowEscapeKey: false,
      allowEnterKey: false,
      confirmButtonText: 'Yes',
      willClose: () => {
        this.flightService.toFlightList();
        clearFormData();
      },
    });
  }

  private showPaymentErrorAlert(errorMessage: string): void {
    Swal.fire({
      title: 'Payment Failed!',
      text: errorMessage,
      icon: 'error',
      showCancelButton: false,
      confirmButtonText: 'OK',
      allowOutsideClick: false,
      allowEscapeKey: false,
      allowEnterKey: false,
      willClose: () => {
        if (errorMessage === this.timeoutErrorMessage) {
          this.flightService.toFlightList();
          clearFormData();
        }
      },
    });
  }

  private startTimer(): void {
    this.timerInterval = setInterval(() => {
      this.remainingSeconds--;
      this.updateDisplayTime();

      if (this.remainingSeconds <= 0) {
        clearInterval(this.timerInterval);
        this.displayTime = '00:00';
        this.showPaymentErrorAlert(this.timeoutErrorMessage);
      }
    }, 1000);
  }

  private updateDisplayTime(): void {
    const minutes = Math.floor(this.remainingSeconds / this.SECONDS);
    const seconds = this.remainingSeconds % this.SECONDS;
    this.displayTime = `${minutes.toString().padStart(2, '0')}:${seconds
      .toString()
      .padStart(2, '0')}`;
  }
}
