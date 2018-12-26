import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { IResponse } from '../../models';

@Injectable()
export class FileService {
    private resoureUrl : string = '';
    
    constructor(private http: HttpClient) {
        this.resoureUrl = [environment.apiUrl, 'file/upload'].join('/')
    }

    public uploadCategoryGroups(fileToUpload: File) : Observable<IResponse<Object>> {
        const url = [this.resoureUrl, 'category-group'].join('/');
        return this.upload(url, fileToUpload);       
    }

    public uploadCategories(fileToUpload: File) : Observable<IResponse<Object>> {
        const url = [this.resoureUrl, 'category'].join('/');
        return this.upload(url, fileToUpload);       
    }

    public uploadBooks(fileToUpload: File) : Observable<IResponse<Object>> {
        const url = [this.resoureUrl, 'book'].join('/');
        return this.upload(url, fileToUpload);       
    }

    private upload(url: string, fileToUpload: File) : Observable<IResponse<Object>> {
        let formData = new FormData();
        formData.append("file", fileToUpload, fileToUpload.name);
        return this.http.post<IResponse<Object>>(url, formData);
    }
}