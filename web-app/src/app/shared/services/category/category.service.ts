import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { CategoryModel, IResponse } from '../../models';

@Injectable()
export class CategoryService {
    private resourceUrl : string = '';

    constructor(private http: HttpClient) {
        this.resourceUrl = [environment.apiUrl, 'category'].join('/');
    }

    public getCategories() : Observable<IResponse<CategoryModel[]>> {
        const url = this.resourceUrl;
        return this.http.get<IResponse<CategoryModel[]>>(url);
    }

    public getCategory(id: number) : Observable<IResponse<CategoryModel>> {
        const url = [this.resourceUrl, '' + id ].join('/');
        return this.http.get<IResponse<CategoryModel>>(url, {
            params: {
                'isBlocking': 'true'
            }
        });
    }

    public createCategory(model: CategoryModel) : Observable<IResponse<Object>> {
        const url = this.resourceUrl;
        return this.http.post<IResponse<Object>>(url, model, {
            params: {
                'isBlocking': 'true'
            }
        });
    }

    public updateCategory(model: CategoryModel) : Observable<IResponse<Object>> {
        const url = [this.resourceUrl, '' + model.id ].join('/');
        return this.http.put<IResponse<Object>>(url, model, {
            params: {
                'isBlocking': 'true'
            }
        });
    }

    public deleteCategory(id: number) : Observable<IResponse<Object>> {
        const url = [this.resourceUrl, '' + id ].join('/');
        return this.http.delete<IResponse<Object>>(url, {
            params: {
                'isBlocking': 'true'
            }
        });
    }
}