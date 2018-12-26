import { Injectable } from '@angular/core';
import { Subject } from 'rxjs/internal/Subject';

export interface MessengerEvent {
  name: 'sidenav-toggle' | 'server-error' | 'clear-error';
  payload: string;
}

@Injectable({
  providedIn: 'root'
})
export class MessengerService {
  private eventSource = new Subject<MessengerEvent>();
  public event$ = this.eventSource.asObservable();
  
  constructor() { }

  public raise(event: MessengerEvent) {
    this.eventSource.next(event);
  }
}
