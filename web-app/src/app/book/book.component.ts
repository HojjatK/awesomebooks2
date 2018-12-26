import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material';
import { BookModel } from 'src/app/shared/models/index';
import { BookService } from 'src/app/shared/services/book/book.service';
import { environment } from 'src/environments/environment';
import { FileService } from '../shared/services/file/file.service';
import { ConfirmDialogComponent, ConfirmDialogData } from '../shared/components/confirm-dialog/confirm-dialog.component';

@Component({
  selector: 'app-book',
  templateUrl: './book.component.html',
  styleUrls: ['./book.component.scss']
})
export class BookComponent implements OnInit {

  constructor(
    private dialog: MatDialog,
    private bookService: BookService,
    private fileService: FileService) { }

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

  ngOnInit() {
    this.loadBooks();
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
    this.bookService.getBooks()
      .subscribe(res => {
        this.books = res.data;
        let allBookRows = [];
        let i = 0;
        let row = [];
        for(let book of this.books) {
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
      });
  }
}