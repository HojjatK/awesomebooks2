import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { McBreadcrumbsResolver, IBreadcrumb } from 'ngx-breadcrumbs';

@Injectable()
export class IdBreadcrumbsResolver extends  McBreadcrumbsResolver {
    
    resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {    
        const id = route.params.id;
        let parentPath = '';
        for(let i = 0; i < route.url.length - 1; i++) {
            if (parentPath == '') {
                parentPath += route.url[i];
            }
            else {
                parentPath += "/" + route.url[i];
            }            
        }

        let parentText = parentPath.replace('-', ' ');
        let words = parentText.split(/\s+/);
        for(let j = 0; j < words.length; j++) {
            words[j] = words[j].charAt(0).toUpperCase() + words[j].slice(1);
        }
        parentText = words.join(' ');
        let  text = id == 'new' ? parentText + ' (New)' : parentText + ' (#'+ id + ')';
        return [{
            text: text,
            path: parentPath + '/' + id
        }];        
      }
}