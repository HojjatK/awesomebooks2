import { Injectable } from '@angular/core';
import { Observable, Subject, throwError } from 'rxjs';
import { catchError, distinctUntilChanged, map, startWith, tap, delay } from 'rxjs/operators';
import { HttpEvent } from '@angular/common/http/src/response';
import { HttpErrorResponse, HttpRequest, HttpResponse } from '@angular/common/http';

export interface LoadingMessage {
    isLoading: boolean;
    url: string;
}

export class BlockingQueue<T> {
    constructor(public urls = []) {
    }
}

export class NonBlockingQueue<T> {
    constructor(public urls = []) {
    }
}

export type Queue = BlockingQueue<string> | NonBlockingQueue<string>;

@Injectable()
export class HttpLoadingService {
    private _nonBlockingQueue$: Subject<Queue> = new Subject<Queue>();
    private _nonBlockingQueue: Queue = new NonBlockingQueue();
    private _blockingQueue$: Subject<Queue> = new Subject<Queue>();
    private _blockingQueue: Queue = new BlockingQueue();

    public isLoading(queue: Queue): boolean {
        return queue.urls.length > 0;
    }

    public includes(queue: Queue, expectedUrlRegex: string): boolean {
        if (!this.isLoading(queue)) {
            return false;
        }
        if (!expectedUrlRegex) {
            return true;
        }
        return queue.urls.some(url => new RegExp(expectedUrlRegex).test(url));
    }

    public emitLoading(queue: Queue) {
        if (queue instanceof BlockingQueue) {
            this._blockingQueue$.next(queue);
        }
        if (queue instanceof NonBlockingQueue) {
            this._nonBlockingQueue$.next(queue);
        }
    }

    public intercept(obs: Observable<HttpEvent<any>>, { url }: HttpRequest<any>, isBlocking: boolean = false): any {
        const queue = isBlocking ? this._blockingQueue : this._nonBlockingQueue;
        return obs
            .pipe(
                delay(0),
                tap((event: HttpEvent<any>) => {
                    if (event instanceof HttpResponse) {
                        this.emitIsNotLoading(url, queue);
                    } else {
                        this.emitIsLoading(url, queue);
                    }
                    return event;
                }),
                catchError((error: HttpErrorResponse) => {
                    this.emitIsNotLoading(url, queue);
                    return throwError(error);
                })
            );
    }

    public get nonBlockingQueue$(): Observable<Queue> {
        return this._nonBlockingQueue$.pipe(startWith(this._nonBlockingQueue));
    }

    public get blockingQueue$(): Observable<Queue> {
        return this._blockingQueue$.pipe(startWith(this._blockingQueue));
    }

    public isLoading$(urlRegex?: string): Observable<boolean> {
        return this.nonBlockingQueue$.pipe(map(queue => this.includes(queue, urlRegex)), distinctUntilChanged());
    }

    public isBlocking$(urlRegex?: string): Observable<boolean> {
        return this.blockingQueue$.pipe(map(queue => this.includes(queue, urlRegex)), distinctUntilChanged());
    }

    public emitIsLoading(url: string, queue: Queue) {
        const updatedQueue = this.updateQueue({ url, isLoading: true }, queue);
        this.emitLoading(updatedQueue);
    }

    public emitIsNotLoading(url: string, queue: Queue) {
        const updatedQueue = this.updateQueue({ url, isLoading: false }, queue);
        this.emitLoading(updatedQueue);
    }

    public updateQueue({ url, isLoading }: LoadingMessage, queue: Queue) {

        const indexOf = queue.urls.indexOf(url);
        if (isLoading) {
            if (indexOf < 0) {
                queue.urls.push(url);
            }
        } else {
            if (indexOf > -1) {
                queue.urls.splice(indexOf, 1);
            }
        }
        return queue;
    }
}