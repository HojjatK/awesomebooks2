import { Component, OnInit, Input } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { CategoryGroupModel, CategoryModel } from 'src/app/shared/models';
import { CategoryGroupService } from 'src/app/shared/services/category-group/category-group.service';
import { CategoryService } from 'src/app/shared/services/category/category.service';

@Component({
  selector: 'app-edit-category',
  templateUrl: './edit-category.component.html',
  styleUrls: ['./edit-category.component.scss']
})
export class EditCategoryComponent implements OnInit {
  private _action: 'new' | 'edit';
  private Id$: Observable<string>;
  private _selectedGroup : CategoryGroupModel = null;

  public categoryGroups: CategoryGroupModel[] = [];
  private _model: CategoryModel = {
    id: 0,
    name: '',
    category_group_id: 0,
    category_group_name: '',
    description: ''
  };

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private categoryGroupService: CategoryGroupService,
    private categoryService: CategoryService) {
  }

  @Input('action')
  public get action(): 'new' | 'edit' {
    return this._action;
  }

  public set action(value) {
    this._action = value;
  }

  public submitText: string = 'Save';

  public get model(): CategoryModel {
    return this._model;
  }

  public set model(val: CategoryModel) {
    this._model = val;
  }

  public get selectedGroup(): CategoryGroupModel {
    return this._selectedGroup;
  }

  public set selectedGroup(val: CategoryGroupModel) {
    this._selectedGroup = val;
    if (val != undefined) {
      this.model.category_group_id = val.id;
      this.model.category_group_name = val.name;
    }
    else {
      this.model.category_group_id = 0;
      this.model.category_group_name = null;
    }
  }

  ngOnInit() {
    this.Id$ = this.route.paramMap.pipe(
      map(params => params.get('id'))
    );
    this.Id$.subscribe(id => {
      this.action = id == 'new' ? 'new' : 'edit';
      this.submitText = id == 'new' ? 'Create' : 'Save';

      this.categoryGroupService.getCategoryGroups()
        .subscribe(res => {
          this.categoryGroups = res.data;

          if (this.action == 'edit' && +id > 0) {
            this.categoryService.getCategory(+id)
              .subscribe(res => {
                this.model = res.data;
                this.categoryGroups.forEach(group => {
                  if (this.model.category_group_id == group.id) {
                    this.selectedGroup = group;
                  }
                });
              })
          }
        });
    });
  }

  onSubmit() {
    if (this.action == 'new') {
      this.categoryService.createCategory(this.model)
        .subscribe(res => {
          this.router.navigate(['/category']);
        });
    }
    else if (this.action == 'edit') {
      this.categoryService.updateCategory(this.model)
        .subscribe(res => {
          this.router.navigate(['/category']);
        });
    }
  }
}