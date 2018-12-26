import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { of, pipe, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { UserSession } from '../../models/index';

@Injectable()
export class AuthService {
    constructor(private router: Router) {
    }

    public get isLoggedIn() {
        return !!localStorage.getItem('currentUser');
    }

    public get currentUser() : UserSession {
        if (this.isLoggedIn) {
            return JSON.parse(localStorage.getItem('currentUser'));
        }
        return null;
    }

    public getJWT() : string {
        var userSession = this.currentUser;
        if (userSession != null && userSession.token != undefined) {
            return userSession.token;
        }
        return null;
    }

    login(username: string, password: string) : Observable<UserSession> {
        if (username == undefined || username.trim() == '' ||
            password == undefined || password.trim() == '') {
            return of(null);
        }
        let userSession : UserSession = {
            'token': '34234234sdf45a65s7f67r6e878w789r9w9ef',
            'username': username
        };
        return of(userSession).pipe(
            map(userSession => {
                if (userSession && userSession.token) {
                    // store user details and jwt token 
                    // in local storage to keep user logged in between page refreshes
                    localStorage.setItem('currentUser', JSON.stringify(userSession));
                }
                return userSession;
            }));
    }

    logout() {
        localStorage.removeItem('currentUser');
        if (!this.router.url.includes('login')) {
            this.router.navigate(['login']);
        }
    }
}