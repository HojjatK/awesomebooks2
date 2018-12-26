import { Component, OnInit, Input } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable, of } from 'rxjs';
import { map, switchMap } from 'rxjs/operators';
import { BookModel, CategoryModel, CategoryGroupModel } from 'src/app/shared/models';
import { CategoryGroupService } from 'src/app/shared/services/category-group/category-group.service';
import { CategoryService } from 'src/app/shared/services/category/category.service';
import { BookService } from 'src/app/shared/services/book/book.service';

@Component({
  selector: 'app-edit-book',
  templateUrl: './edit-book.component.html',
  styleUrls: ['./edit-book.component.scss']
})
export class EditBookComponent implements OnInit {
  private _action: 'new' | 'edit';
  private Id$: Observable<string>;
  private _selectedGroup: CategoryGroupModel;
  public categoryGroups: CategoryGroupModel[] = [];

  private _selectedCategory: CategoryModel;
  public allCategories: CategoryModel[] = [];
  public categories: CategoryModel[] = [];

  private _model: BookModel = {
    id: 0,
    name: '',
    publish_year: null,
    authors: '',
    rating: null,
    image_uri: '',
    amazon_uri: '',
    content_type: '',
    content_uri: '',
    reflection: '',
    category_id: 0,
    category_name: '',
  };

  public submitText: string = 'Save';

  public get model(): BookModel {
    return this._model;
  }

  public set model(val: BookModel) {
    this._model = val;
  }

  public get selectedCategoryGroup(): CategoryGroupModel {
    return this._selectedGroup;
  }

  public set selectedCategoryGroup(val: CategoryGroupModel) {
    this._selectedGroup = val;
    if (val != undefined) {
      this.fillCategories(val.id);
    }
    else {
      this.categories = [];
    }
  }

  public get selectedCategory(): CategoryModel {
    return this._selectedCategory;
  }

  public set selectedCategory(val: CategoryModel) {
    this._selectedCategory = val;
    if (val != undefined) {
      this.model.category_id = val.id;
      this.model.category_name = val.name;
    }
    else {
      this.model.category_id = 0;
      this.model.category_name = null;
    }
  }

  @Input('action')
  public get action(): 'new' | 'edit' {
    return this._action;
  }

  public set action(value) {
    this._action = value;
  }

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private categoryGroupService: CategoryGroupService,
    private categoryService: CategoryService,
    private bookService: BookService
  ) {
  }

  ngOnInit() {
    this.Id$ = this.route.paramMap.pipe(
      map(params => params.get('id'))
    );

    this.Id$.subscribe(id => {
      this.action = id == 'new' ? 'new' : 'edit';
      this.categoryGroupService.getCategoryGroups().pipe(
        switchMap(res => {
          this.categoryGroups = res.data;
          return this.categoryService.getCategories();
        }),
        switchMap(res => {
          this.allCategories = res.data;
          if (this.action == 'edit') {
            return this.bookService.getBook(+id);
          }
          return of({
            data: null
          });
        })).subscribe(res => {
          if (this.action == 'edit') {
            this.model = res.data;
            this.setCategroyId(this.model.category_id);
            this.setCategoryGroupId(this.selectedCategory.category_group_id);
          }
          else {
            if (this.allCategories.length > 0) {
              var categoryId = this.allCategories[0].id;
              this.setCategroyId(categoryId);
              this.setCategoryGroupId(this.selectedCategory.category_group_id);
            }
          }
        });
    });
  }

  onSubmit() {
    if (this.action == 'new') {
      this.bookService.createBook(this.model)
        .subscribe(res => {
          this.router.navigate(['/book']);
        });
    }
    else if (this.action == 'edit') {
      this.bookService.updateBook(this.model)
        .subscribe(res => {
          this.router.navigate(['/book']);
        });
    }
  }

  private setCategoryGroupId(categoryGroupId: number) {
    this.categoryGroups.forEach(cg => {
      if (cg.id == categoryGroupId) {
        this.selectedCategoryGroup = cg;
      }
    });
  }

  private setCategroyId(categoryId: number) {
    this.allCategories.forEach(c => {
      if (c.id == categoryId) {
        this.selectedCategory = c;
      }
    });
  }

  private fillCategories(categoryGroupId: number) {
    this.categories = [];
    this.allCategories.forEach(c => {
      if (this.selectedCategory != undefined &&
        c.category_group_id == categoryGroupId) {
        this.categories.push(c);
      }
    });
  }
}