import { Component, OnInit, Input, ViewChild } from '@angular/core';
import { MatDialog, MatPaginator, MatTableDataSource, MatSort } from '@angular/material';
import { CategoryModel } from 'src/app/shared/models/index';
import { CategoryService } from '../shared/services/category/category.service';
import { ConfirmDialogComponent, ConfirmDialogData } from '../shared/components/confirm-dialog/confirm-dialog.component';
import { FileService } from '../shared/services/file/file.service';
import { environment } from 'src/environments/environment';

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
    private fileService: FileService) { }

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
    this.loadCategories();
  }

  private loadCategories(): void {
    this.categoryService.getCategories().subscribe(res => {
      this.dataSource.data = res.data;
    });
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