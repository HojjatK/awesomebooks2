import { Injectable } from '@angular/core';
import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest, HttpResponse } from '@angular/common/http';
import { Observable, of, throwError } from 'rxjs';
import { tap, catchError } from 'rxjs/operators';
import { LogService } from '../log/log.service';
import {MessengerService} from '../../services/messenger/messenger.service';

export interface IHeaders {
    [name: string]: string;
}

@Injectable()
export class HttpResponseInterceptor implements HttpInterceptor {
    constructor(
        private logService: LogService,
        private messengerService: MessengerService) {
    }

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        return next
            .handle(request)
            .pipe(tap((event: any) => {
                if (event instanceof HttpResponse) {
                    return this.handleResponse(event);
                }
                return event;
            }),
                catchError(error => of(this.handleResponse(error)))
            );
    }

    private handleResponse<T>(res: HttpResponse<T> | HttpErrorResponse): any {
        let msg = '';
        let errMsg = '';
        let response : any = res;
        if(response.error != undefined && response.error.Meta != undefined && 
           response.error.Meta.ErrorMessage != '') {
            errMsg = response.error.Meta.ErrorMessage;
        }
        switch (res.status) {
            case 200: // OK
            case 201: // Created
                this.logService.info('Http Success: ' + JSON.stringify(res));
                break;
            case 202: // RETRY
            case 400: // VALIDATION (Invalid Request)
                this.logService.warn('Invalid Request:' + JSON.stringify(res));
                msg = errMsg != '' ? errMsg : 'Validation Error';
                break;
            case 401: // FORBIDDEN
            case 403: // UNAUTHORIZED
            case 404: // NOT FOUND
                const errorMsg = `${res.status} Error:`;
                this.logService.error(errorMsg + JSON.stringify(res));
                return throwError(errorMsg);
            case 409: // CONCURRENCY
                this.logService.warn('Concurrency Error:' + JSON.stringify(res));
                msg = errMsg != '' ? errMsg : 'Concurrency error';
                break;
            case 500: // SERVER ERROR
                this.logService.error('500 Error: ' + JSON.stringify(res));
                return throwError('Internal Server Error');
            default:
                this.logService.error('Unknown status code.' + res.status);
                return throwError('Unknown Status Code: ' + res.status);
        }
        
        if (msg != '') {
            this.messengerService.raise({
                name: 'server-error',
                payload: msg
            });
        }
        else {
            this.messengerService.raise({
                name: 'clear-error',
                payload: ''
            });
        }
        
        return res;
    }
}