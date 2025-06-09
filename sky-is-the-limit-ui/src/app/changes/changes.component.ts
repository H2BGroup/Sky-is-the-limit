import { Component, inject, OnInit } from '@angular/core';
import { NotificationsService } from '../notifications.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-changes',
  imports: [CommonModule],
  templateUrl: './changes.component.html',
  styleUrl: './changes.component.css',
})
export class ChangesComponent implements OnInit {
  private notifcationsService = inject(NotificationsService);

  protected recentChanges: any[] = [];

  ngOnInit() {
    this.notifcationsService.startConnection();
    this.notifcationsService.receiveOfferUpdated((data) =>
      this.onMessageReceived(data)
    );
  }

  onMessageReceived(data: any) {
    if (this.recentChanges.length >= 10) {
      this.recentChanges.pop();
    }
    this.recentChanges.unshift(data);
  }
}
