import { ActivatedRoute, Router } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/shared/services/auth/auth.service';

@Component({
  selector: 'app-sign-in',
  templateUrl: './sign-in.component.html',
  styleUrls: ['./sign-in.component.scss']
})
export class SignInComponent implements OnInit {

  returnUrl : string;
  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private authService: AuthService) { }
  
  public model = {
    username : '',
    password: ''
  };

  ngOnInit() {
    // reset login status
    this.authService.logout();

    // get return url from route parameters or default to '/'
    this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
  }

  public login() {
    this.authService.login(this.model.username, this.model.password)
      .subscribe(user => {
        console.log('user is logged in.');
        this.router.navigateByUrl(this.returnUrl);
      });
  }
}
