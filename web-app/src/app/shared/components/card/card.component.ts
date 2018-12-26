import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-card',
  templateUrl: './card.component.html',
  styleUrls: ['./card.component.scss']
})
export class CardComponent implements OnInit {

  constructor() { }

  @Input()
  public title : string;

  @Input()
  public subTitle : string;

  @Input()
  public imageSrc : string;

  @Input()
  public description : string;

  ngOnInit() {
  }
}
