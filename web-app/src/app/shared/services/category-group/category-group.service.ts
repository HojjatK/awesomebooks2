import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { CategoryGroupModel, IResponse } from '../../models';

@Injectable()
export class CategoryGroupService {
    private resourceUrl : string = '';

    constructor(private http: HttpClient) {
        this.resourceUrl = [environment.apiUrl, 'category-group'].join('/');
    }

    public getCategoryGroups() : Observable<IResponse<CategoryGroupModel[]>> {
        const url = this.resourceUrl;
        return this.http.get<IResponse<CategoryGroupModel[]>>(url);
    }

    public getCategoryGroup(id: number) : Observable<IResponse<CategoryGroupModel>> {
        const url = [this.resourceUrl, '' + id ].join('/');
        return this.http.get<IResponse<CategoryGroupModel>>(url, {
            params: {
                'isBlocking': 'true'
            }
        });
    }

    public createCategoryGroup(model: CategoryGroupModel) : Observable<IResponse<Object>> {
        const url = this.resourceUrl;
        return this.http.post<IResponse<Object>>(url, model, {
            params: {
                'isBlocking': 'true'
            }
        });
    }

    public updateCategoryGroup(model: CategoryGroupModel) : Observable<IResponse<Object>> {
        const url = [this.resourceUrl, '' + model.id ].join('/');
        return this.http.put<IResponse<Object>>(url, model, {
            params: {
                'isBlocking': 'true'
            }
        });
    }

    public deleteCategoryGroup(id: number) : Observable<IResponse<Object>> {
        const url = [this.resourceUrl, '' + id ].join('/');
        return this.http.delete<IResponse<Object>>(url, {
            params: {
                'isBlocking': 'true'
            }
        });
    }
}