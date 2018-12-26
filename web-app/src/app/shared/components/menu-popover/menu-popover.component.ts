import { Component, OnInit, Input, ElementRef, HostListener } from '@angular/core';

@Component({
  selector: 'menu-popover',
  templateUrl: './menu-popover.component.html',
  styleUrls: ['./menu-popover.component.scss']
})
export class MenuPopoverComponent implements OnInit {
  @Input('title')
  buttonTitle: string = '';

  @Input('placement')
  placement: string = 'left';

  visible: boolean = false;
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
