import { Component, OnInit, Input } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CategoryGroupModel } from 'src/app/shared/models';
import { Observable } from 'rxjs';
import { map }  from 'rxjs/operators';
import { CategoryGroupService } from 'src/app/shared/services/category-group/category-group.service';

@Component({
  selector: 'app-edit-category-group',
  templateUrl: './edit-category-group.component.html',
  styleUrls: ['./edit-category-group.component.scss']
})
export class EditCategoryGroupComponent implements OnInit {
  constructor(
    private route : ActivatedRoute,
    private router: Router,
    private categoryGroupService : CategoryGroupService) {
  }

  private _action : 'new' | 'edit';
  private Id$: Observable<string>;

  private _model: CategoryGroupModel = {
    id: 0,
    name: '',
    description: ''
  };

  @Input('action')
  public get action() : 'new' | 'edit' {
    return this._action;
  }

  public set action(value) {
    this._action = value;
  }

  public submitText : string = 'Save'; 

  public get model() : CategoryGroupModel {
    return this._model;
  }

  public set model(val: CategoryGroupModel) {
    this._model = val;
  }

  ngOnInit() {
    this.Id$ = this.route.paramMap.pipe(
      map(params => params.get('id'))
    );
    this.Id$.subscribe(id => 
      {
        this.action = id == 'new' ? 'new' : 'edit';
        this.submitText = id == 'new' ? 'Create' : 'Save';
        if (this.action == 'edit' && +id > 0) {
          this.categoryGroupService.getCategoryGroup(+id)
              .subscribe(res => {
                this.model = res.data;
              })
        }
      });
  }

  onSubmit() {
    if (this.action == 'new') {
      this.categoryGroupService.createCategoryGroup(this.model)
          .subscribe(res => {
            this.router.navigate(['/category-group']);
          });
    }
    else if (this.action == 'edit') {
      this.categoryGroupService.updateCategoryGroup(this.model)
          .subscribe(res => {
            this.router.navigate(['/category-group']);
          });
    }
  }
}