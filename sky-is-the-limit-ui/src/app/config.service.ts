import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class ConfigService {
  private config: any;

  loadConfig(): Promise<void> {
    return fetch('/assets/config/config.json')
      .then((res) => res.json())
      .then((config) => (this.config = config));
  }

  get bookingUrl(): string {
    return this.config?.bookingUrl;
  }

  get paymentUrl(): string {
    return this.config?.paymentUrl;
  }

  get userUrl(): string {
    return this.config?.userUrl;
  }

  get offerUrl(): string {
    return this.config?.offerUrl;
  }
}
