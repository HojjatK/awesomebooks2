import { Component, OnInit, Input, ViewChild } from '@angular/core';
import { MatDialog, MatPaginator, MatTableDataSource, MatSort } from '@angular/material';
import { CategoryModel, CategoryGroupModel } from 'src/app/shared/models/index';
import { CategoryService } from '../shared/services/category/category.service';
import { ConfirmDialogComponent, ConfirmDialogData } from '../shared/components/confirm-dialog/confirm-dialog.component';
import { FileService } from '../shared/services/file/file.service';
import { environment } from 'src/environments/environment';
import { CategoryGroupService } from '../shared/services/category-group/category-group.service';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Component({
  selector: 'app-category',
  templateUrl: './category.component.html',
  styleUrls: ['./category.component.scss']
})
export class CategoryComponent implements OnInit {
  public columns : string[] = ['id', 'name', 'category_group_name', 'description', 'action'];
  dataSource = new MatTableDataSource<CategoryModel>([]);
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  categoryFile: File;

  constructor(
    private dialog: MatDialog,
    private categoryService: CategoryService,
    private categoryGroupService: CategoryGroupService,
    private fileService: FileService) { }

  public categoryGroups: CategoryGroupModel[] = [];
  public allCategories: CategoryModel[] = [];

  private DEFAULT_GROUP : CategoryGroupModel = {
    id: null,
    name: 'All Category Groups',
    description: null
  };

  private _selectedGroup : CategoryGroupModel = null;  
  public get selectedGroup(): CategoryGroupModel {
    return this._selectedGroup;
  }

  public set selectedGroup(val: CategoryGroupModel) {
    this._selectedGroup = val;    
    this.filterCategories();
  }

  public pageSizes : Array<number> = [10, 20, 30];

  public get columnsToDisplay() : string [] {
    return this.columns;
  }

  public set columnsToDisplay(value : string[])  {
    this.columns = value;
  }

  public get downloadCategoriesUrl() : string {
    return [environment.apiUrl, 'file', 'download/categories'].join('/');
  }

  ngOnInit() {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
    this.loadCategoryGroups().subscribe(groups => {
      this.categoryGroups = [this.DEFAULT_GROUP, ...groups];      
      this.loadCategories();      
    });
    
  }

  private loadCategoryGroups() : Observable<CategoryGroupModel[]> {
    return this.categoryGroupService.getCategoryGroups().pipe(
      map(res => res.data)
    );
  }

  private loadCategories(): void {
    this.categoryService.getCategories().subscribe(res => {
      this.allCategories = res.data;
      this.selectedGroup = this.DEFAULT_GROUP;
    });
  }

  private filterCategories() : void {
    if (this.selectedGroup != null && this.selectedGroup.id != undefined) {      
      this.dataSource.data = this.allCategories.filter(c => c.category_group_id == this.selectedGroup.id);
    }
    else {
      this.dataSource.data = this.allCategories;
    }

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }

  applyFilter(filterValue: string) {
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }

  public delete(model: CategoryModel) {
    let dialogData: ConfirmDialogData = {
      id: model.id,
      title: 'Delete "' + model.name + '" Category',
      confirmMessage: 'You cannot undo the delete, are you sure you want to delete "' + model.name + '" category ?',
      confirmButton: 'Confirm'
    }
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      width: '550px',
      data: dialogData
    });

    dialogRef.afterClosed().subscribe(id => {
      if (id != undefined && +id > 0) {
        this.categoryService.deleteCategory(+id)
          .subscribe(res => {
            this.loadCategories();
          });
      }
    });
  }

  onSubmitUpload = (fileToUpload: File) => {
    if (fileToUpload == undefined) {
      return;
    }
    this.fileService.uploadCategories(fileToUpload)
      .subscribe((data: any) => {
        this.loadCategories();
      });
  }
}