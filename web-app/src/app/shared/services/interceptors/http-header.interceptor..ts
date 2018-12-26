import { Injectable } from '@angular/core';
import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthService } from '../auth/auth.service';
import { environment } from  'src/environments/environment';

export interface IHeaders {
    [name: string]: string;
}

@Injectable()
export class HttpHeadersInterceptor implements HttpInterceptor {
    apiUrl = environment.apiUrl;
    constructor(private authService : AuthService) {
    }

    intercept<T>(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<T>> {
        return next.handle(request.clone({ setHeaders: this.buildHeadersObject(request) }));
    }

    public get authHeader() {
        return ['Bearer', this.authService.getJWT()].join(' ');
    }

    protected buildHeadersObject(request: HttpRequest<any>): IHeaders {
        let url = [request.url, 'file'].join('/'); 
        if (url.startsWith(this.apiUrl)) {
            // content type is not application/json
            return {
                'Authorization': this.authHeader
            };
        }

        return {
            'Content-Type': 'application/json',
            'Authorization': this.authHeader
        };
    }
}