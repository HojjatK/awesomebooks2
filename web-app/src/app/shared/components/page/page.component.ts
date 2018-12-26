import { Component, OnInit, Input } from '@angular/core';
import { MessengerService } from './../../services/messenger/messenger.service';
import { Observable, Subject, throwError } from 'rxjs';

@Component({
  selector: 'app-page',
  templateUrl: './page.component.html',
  styleUrls: ['./page.component.scss']
})
export class PageComponent implements OnInit {
  constructor(private messenger: MessengerService) {
    messenger.event$.subscribe(e => {
      if (e.name == 'clear-error') {
        this.showError = false;
        this.errorMessage = '';
      }
      else if (e.name == 'server-error') {
        this.showError = true;
        this.errorMessage = e.payload == undefined || e.payload == '' ? null : e.payload.trim();
      }
    });
  }

  public showError : boolean = false;

  private _errorMessage: string = '';
  public get errorMessage() : string {
    return this._errorMessage;
  } 

  public set errorMessage(err : string) {
    this._errorMessage = err;
  }

  @Input('title')
  public title: string;

  ngOnInit() {
  }
}
