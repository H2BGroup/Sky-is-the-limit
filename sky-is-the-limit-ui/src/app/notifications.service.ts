import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';

@Injectable({
  providedIn: 'root',
})
export class NotificationsService {
  private hubConnection!: signalR.HubConnection;

  public startConnection() {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('/notifications')
      .withAutomaticReconnect()
      .build();

    this.hubConnection
      .start()
      .then(() => console.log('Connection started'))
      .catch((err) => console.log('Error while starting connection: ' + err));
  }

  public receiveBookingConfirmed(callback: (data: any) => void) {
    this.hubConnection.on('BookingConfirmed', callback);
  }
}
