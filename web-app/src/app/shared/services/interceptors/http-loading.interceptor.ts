import { Injectable } from '@angular/core';
import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Observable } from 'rxjs';
import { HttpLoadingService } from '../http-loading/http-loading.service';

@Injectable()
export class HttpLoadingInterceptor implements HttpInterceptor {
  constructor(private loadingService: HttpLoadingService) {
  }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const isBlockingStr = request.params.get("isBlocking");
    let isBlocking = false;
    if (isBlockingStr != undefined && isBlockingStr != '') {
        isBlocking = isBlockingStr == 'true' || +isBlockingStr > 0 ? true : false;
    }
    return this.loadingService.intercept(next.handle(request), request, isBlocking);
  }
}
