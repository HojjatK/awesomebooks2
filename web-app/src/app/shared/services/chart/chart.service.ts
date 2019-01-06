import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { ChartResult, ChartSerie, ChartSerieResult, IResponse } from '../../models';

@Injectable()
export class ChartService {
    private resourceUrl: string = '';

    constructor(private http: HttpClient) {
        this.resourceUrl = [environment.apiUrl, 'chart'].join('/');
    }

    public getCategoryGroupChart(): Observable<IResponse<ChartResult>> {
        const url = [this.resourceUrl, 'category-group'].join('/');
        return this.http.get<IResponse<ChartResult>>(url);
    }

    public getCategoryChart(): Observable<IResponse<ChartResult>> {
        const url = [this.resourceUrl, 'category'].join('/');
        return this.http.get<IResponse<ChartResult>>(url);
    }

    public getCategorySerieChart(): Observable<IResponse<ChartSerieResult>> {
        const url = [this.resourceUrl, 'category-serie'].join('/');
        return this.http.get<IResponse<ChartSerieResult>>(url);
    }

    public getBookPublishChart(): Observable<IResponse<ChartSerieResult>> {
        const url = [this.resourceUrl, 'book-publish'].join('/');
        return this.http.get<IResponse<ChartSerieResult>>(url);
    }
}