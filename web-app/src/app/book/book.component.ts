import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material';
import { map, switchMap } from 'rxjs/operators';
import { BookModel } from 'src/app/shared/models/index';
import { BookService } from 'src/app/shared/services/book/book.service';
import { environment } from 'src/environments/environment';
import { FileService } from '../shared/services/file/file.service';
import { ConfirmDialogComponent, ConfirmDialogData } from '../shared/components/confirm-dialog/confirm-dialog.component';
import { CategoryGroupModel, CategoryModel } from '../shared/models/index';
import { CategoryGroupService } from '../shared/services/category-group/category-group.service';
import { CategoryService } from '../shared/services/category/category.service';

@Component({
  selector: 'app-book',
  templateUrl: './book.component.html',
  styleUrls: ['./book.component.scss']
})
export class BookComponent implements OnInit {

  constructor(
    private dialog: MatDialog,
    private categoryGroupService: CategoryGroupService,
    private categoryService: CategoryService,
    private bookService: BookService,
    private fileService: FileService) { }

  private DEFAULT_GROUP : CategoryGroupModel = {
    id: null,
    name: '(All Category Groups)',
    description: null
  };

  private DEFAULT_CATEGORY : CategoryModel = {
    id: null,
    category_group_id: null,
    category_group_name: null,
    name: '(All Categories)',
    description: null
  };

  private _selectedGroup: CategoryGroupModel;
  public categoryGroups: CategoryGroupModel[] = [];

  private _selectedCategory: CategoryModel;
  public allCategories: CategoryModel[] = [];
  public categories: CategoryModel[] = [];

  public get selectedCategoryGroup(): CategoryGroupModel {
    return this._selectedGroup;
  }

  public set selectedCategoryGroup(val: CategoryGroupModel) {
    this._selectedGroup = val;
    if (val != undefined) {
      this.fillCategories(val.id);
    }
    else {
      this.fillCategories(null);
    }    
  }

  public filterValue : string = '';
  public get selectedCategory(): CategoryModel {
    return this._selectedCategory;
  }

  public set selectedCategory(val: CategoryModel) {
    this._selectedCategory = val;    
    this.applyFilter(this.filterValue);
  }

  bookFile: File;
  private _books : BookModel[] = [];
  private _bookRows: BookModel[][] = [];
  
  public get books() : BookModel[] {
    return this._books;
  }

  public set books(val: BookModel[]) {
    this._books = val;
  }

  public get bookRows() : BookModel[][] {
    return this._bookRows;
  }

  public set bookRows(val : BookModel[][]) {
    this._bookRows = val;
  }

  public get downloadBooksUrl() : string {
    return [environment.apiUrl, 'file', 'download/books'].join('/');
  }

  public filteredCount : string = '';

  ngOnInit() {
    this.loadBooks();
  }

  applyFilter(filterValue: string) {    
    let selectedCategoryGroupId = this.selectedCategoryGroup != null && this.selectedCategoryGroup.id != undefined ? this.selectedCategoryGroup.id : undefined;
    let selectedCategoryId = this.selectedCategory != null && this.selectedCategory.id != undefined ? this.selectedCategory.id : undefined;

    let filteredBooks = this.books.filter(b => 
      (selectedCategoryGroupId == undefined || b.category_group_id == selectedCategoryGroupId) &&
      (selectedCategoryId == undefined || b.category_id == selectedCategoryId));

    if (filterValue != undefined && filterValue.trim() != '') {
      filteredBooks = this.filterBooks(filterValue, filteredBooks);
    } 
    this.fillBookRows(filteredBooks);
  }

  public delete(model: BookModel) {
    let dialogData: ConfirmDialogData = {
      id: model.id,
      title: 'Delete "' + model.name + '" Category',
      confirmMessage: 'You cannot undo the delete, are you sure you want to delete "' + model.name + '" book ?',
      confirmButton: 'Confirm'
    }
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      width: '550px',
      data: dialogData
    });

    dialogRef.afterClosed().subscribe(id => {
      if (id != undefined && +id > 0) {
        this.bookService.deleteBook(+id)
          .subscribe(res => {
            this.loadBooks();
          });
      }
    });
  }

  onSubmitUpload = (fileToUpload: File) => {
    if (fileToUpload == undefined) {
      return;
    }
    this.fileService.uploadBooks(fileToUpload)
      .subscribe((data: any) => {
        this.loadBooks();
      });
  }

  private loadBooks() : void {
    this.categoryGroupService.getCategoryGroups().pipe(
      switchMap(res => {
        this.categoryGroups = [this.DEFAULT_GROUP, ...res.data];
        return this.categoryService.getCategories();
      }),
      switchMap(res => {
        this.allCategories = res.data;
        this.fillCategories(null);
        return this.bookService.getBooks()
      })
    ).subscribe(res => {
        this.books = res.data;
        for(let i = 0; i < this.books.length; i++) {
          let category = this.findCategory(this.books[i].category_id);
          if(category != null) {
            this.books[i].category_name = category.name;
            this.books[i].category_group_id = category.category_group_id;
            this.books[i].category_group_name = category.category_group_name;
          }
        }        
        this.selectedCategoryGroup = this.DEFAULT_GROUP;
        this.selectedCategory = this.DEFAULT_CATEGORY;
      });
  }

  private findCategory(categoryId: number){
    for(let j = 0; j < this.allCategories.length; j++) {
      if (this.allCategories[j].id == categoryId) {
        return this.allCategories[j];
      }
    }
    return null;
  }

  private filterBooks(text: string, source:  BookModel[]) : BookModel[] {
    let result = [];
    if (source != null && source.length > 0) {
      let texts = text.split(/\s+/).join('|').toLowerCase().split('|');
      result = source.filter(b => {
        let lower_name = b.name.toLowerCase();
        let lower_authors = b.authors.toLowerCase();
        let found = false;
        for(let i = 0; i < texts.length; i ++) {
          found = (lower_name.indexOf(texts[i]) != -1) || (lower_authors.indexOf(texts[i]) != -1);
          if (!found) {
            break;
          }
        }

        return found == true;
      });
    }
    return result;
  }

  private fillBookRows(source:  BookModel[]) {
    let allBookRows = [];
    let i = 0;
    let row = [];
    for(let book of source) {
      row.push(book);
      if ((i + 1)%3 == 0) {
        allBookRows.push(row);
        row = [];
      }
      i++;
    }
    if (row.length > 0) {
      allBookRows.push(row);
    }
    this.bookRows = allBookRows;
    this.filteredCount = '( ' + source.length + ' out of ' + this.books.length + ' )';
  }

  private fillCategories(categoryGroupId: number) {
    this.categories = [];
    this.allCategories.forEach(c => {
      if (categoryGroupId != undefined &&
        c.category_group_id == categoryGroupId) {
        this.categories.push(c);        
      }
    });
    this.categories = [this.DEFAULT_CATEGORY, ...this.categories];
    this.selectedCategory = this.DEFAULT_CATEGORY;
  }
}