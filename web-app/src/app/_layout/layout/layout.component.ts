import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { Observable, of } from 'rxjs';
import { MessengerService } from 'src/app/shared/services/messenger/messenger.service';
import { HttpLoadingService } from 'src/app/shared/services/http-loading/http-loading.service';

@Component({
  selector: 'app-layout',
  templateUrl: './layout.component.html',
  styleUrls: ['./layout.component.scss']
})
export class LayoutComponent implements OnInit {

  constructor(
    private messengerService: MessengerService,
    private loadingService: HttpLoadingService) {
    messengerService.event$.subscribe(e => {
      if (e.name == 'sidenav-toggle') {
        let sidenav : any = this.sidenav; 
        sidenav.toggle();
      }
    });
   }

   public isBusy: boolean = false;

   @ViewChild('sidenav') sidenav: ElementRef;

   ngOnInit() {
     // this is for demo only
    this.isBusy = true;
    setTimeout(() => {
      this.isBusy = false;
    }, 1000);
   }

   private urlRegex = '.';
   public get isBlocking$(): Observable<boolean> {
    return this.loadingService.isBlocking$(this.urlRegex);
   }

   public get isLoading$(): Observable<boolean> {
     return this.loadingService.isLoading$(this.urlRegex);
   }
}