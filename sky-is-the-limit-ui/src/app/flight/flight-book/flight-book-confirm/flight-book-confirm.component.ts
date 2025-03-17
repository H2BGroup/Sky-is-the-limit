import { Component, OnDestroy, OnInit } from '@angular/core';

@Component({
  selector: 'app-flight-book-confirm',
  imports: [],
  templateUrl: './flight-book-confirm.component.html',
  styleUrl: './flight-book-confirm.component.css',
})
export class FlightBookConfirmComponent implements OnInit, OnDestroy {
  protected displayTime: string = '01:00';
  private remainingSeconds: number = 60;
  private timerInterval: any;

  private readonly SECONDS = 60;

  ngOnInit(): void {
    this.startTimer();
  }

  ngOnDestroy(): void {
    clearInterval(this.timerInterval);
  }

  onPayment() {}

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
