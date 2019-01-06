import { Component, OnInit, Input, ViewChild } from '@angular/core';
import { MatDialog, MatPaginator, MatTableDataSource, MatSort } from '@angular/material';
import { CategoryGroupModel } from 'src/app/shared/models/index';
import { CategoryGroupService } from '../shared/services/category-group/category-group.service';
import { ConfirmDialogComponent, ConfirmDialogData } from '../shared/components/confirm-dialog/confirm-dialog.component';
import { FileService } from '../shared/services/file/file.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-category-group',
  templateUrl: './category-group.component.html',
  styleUrls: ['./category-group.component.scss']
})
export class CategoryGroupComponent implements OnInit {
  constructor(
    private dialog: MatDialog,
    private categoryGroupService: CategoryGroupService,
    private fileService: FileService) {
  }

  public columns: string[] = ['id', 'name', 'description', 'action'];

  public dataSource = new MatTableDataSource<CategoryGroupModel>([]);

  @ViewChild(MatPaginator) paginator: MatPaginator;

  @ViewChild(MatSort) sort: MatSort;

  categoryGroupFile: File;

  public pageSizes : Array<number> = [10, 20];

  public get columnsToDisplay(): string[] {
    return this.columns;
  }

  public set columnsToDisplay(value: string[]) {
    this.columns = value;
  }

  public get downloadCategoryGroupsUrl() : string {
    return [environment.apiUrl, 'file', 'download/category-groups'].join('/');
  } 

  ngOnInit() {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
    this.loadCategoryGroups();
  }

  private loadCategoryGroups(): void {
    this.categoryGroupService.getCategoryGroups().subscribe(res => {
      this.dataSource.data = res.data;
    });
  }

  applyFilter(filterValue: string) {
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }

  public delete(model: CategoryGroupModel) {
    let dialogData: ConfirmDialogData = {
      id: model.id,
      title: 'Delete "' + model.name + '" Category Group',
      confirmMessage: 'You cannot undo the delete, are you sure you want to delete "' + model.name + '" category group ?',
      confirmButton: 'Confirm'
    }
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      width: '550px',
      data: dialogData
    });

    dialogRef.afterClosed().subscribe(id => {
      if (id != undefined && +id > 0) {
        this.categoryGroupService.deleteCategoryGroup(+id)
          .subscribe(res => {
            this.loadCategoryGroups();
          });
      }
    });
  }

  onSubmitUpload = (fileToUpload: File) => {
    if (fileToUpload == undefined) {
      return;
    }
    this.fileService.uploadCategoryGroups(fileToUpload)
      .subscribe((data: any) => {
        this.loadCategoryGroups();
      });
  }
}
