import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  public showSignIn: boolean = true;

  constructor() { }

  singInClick() {
    this.showSignIn = true;
  }

  singUpClick() {
    this.showSignIn = false;
  }
  ngOnInit() {
  }

}
