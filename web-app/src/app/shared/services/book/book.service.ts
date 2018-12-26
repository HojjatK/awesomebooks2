import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { BookModel, IResponse } from '../../models';

@Injectable()
export class BookService {
    private resourceUrl: string = '';

    constructor(private http: HttpClient) {
        this.resourceUrl = [environment.apiUrl, 'book'].join('/');
    }

    public getBooks(): Observable<IResponse<BookModel[]>> {
        const url = this.resourceUrl;
        return this.http.get<IResponse<BookModel[]>>(url);
    }

    public getBook(id: number): Observable<IResponse<BookModel>> {
        const url = [this.resourceUrl, '' + id].join('/');
        return this.http.get<IResponse<BookModel>>(url, {
            params: {
                'isBlocking': 'true'
            }
        });
    }

    public createBook(model: BookModel): Observable<IResponse<Object>> {
        const url = this.resourceUrl;
        return this.http.post<IResponse<Object>>(url, model, {
            params: {
                'isBlocking': 'true'
            }
        });
    }

    public updateBook(model: BookModel): Observable<IResponse<Object>> {
        const url = [this.resourceUrl, '' + model.id].join('/');
        return this.http.put<IResponse<Object>>(url, model, {
            params: {
                'isBlocking': 'true'
            }
        });
    }

    public deleteBook(id: number): Observable<IResponse<Object>> {
        const url = [this.resourceUrl, '' + id].join('/');
        return this.http.delete<IResponse<Object>>(url, {
            params: {
                'isBlocking': 'true'
            }
        });
    }
}