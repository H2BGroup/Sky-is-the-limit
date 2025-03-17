import { Component, inject, OnDestroy, OnInit } from '@angular/core';
import { FlightService } from '../../flight.service';
import Swal from 'sweetalert2';
import { clearFormData } from '../../shared/clearFormData';

@Component({
  selector: 'app-flight-book-confirm',
  imports: [],
  templateUrl: './flight-book-confirm.component.html',
  styleUrl: './flight-book-confirm.component.css',
})
export class FlightBookConfirmComponent implements OnInit, OnDestroy {
  private flightService = inject(FlightService);
  private remainingSeconds: number = 60;
  private timerInterval: any;

  private readonly SECONDS = 60;

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

    setTimeout(() => {
      this.isLoading = false;
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
    }, 2000);
  }

  private startTimer(): void {
    this.timerInterval = setInterval(() => {
      this.remainingSeconds--;
      this.updateDisplayTime();

      if (this.remainingSeconds <= 0) {
        clearInterval(this.timerInterval);
        this.displayTime = '00:00';
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
