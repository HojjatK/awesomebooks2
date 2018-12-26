import { Component, OnInit, Input, ElementRef, HostListener } from '@angular/core';

@Component({
  selector: 'app-notification',
  templateUrl: './notification.component.html',
  styleUrls: ['./notification.component.scss']
})
export class NotificationComponent implements OnInit {
  @Input('placement')
  placement: string = 'left';

  public visible: boolean = false;
  
  constructor(private eRef: ElementRef) { }

  ngOnInit() {
  }

  public onClick() {
    this.visible = !this.visible;
  }

  @HostListener('document:click', ['$event'])
  clickout(event) {
    if(!this.eRef.nativeElement.contains(event.target)) {
      this.visible = false;
    }
  }
}
