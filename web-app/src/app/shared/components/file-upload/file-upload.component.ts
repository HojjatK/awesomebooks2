import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-file-upload',
  templateUrl: './file-upload.component.html',
  styleUrls: ['./file-upload.component.scss']
})
export class FileUploadComponent implements OnInit {

  constructor() { }
  @Input() uploadFile: File;
  @Input() tootip: string = '';
  @Output() uploadSelected: EventEmitter<File> = new EventEmitter<File>();

  public fileSelected : boolean = false;
  public dragover : boolean = false;

  ngOnInit() {
  }

  onFileUploadChange(files: Array<File>) {    
    if (files != undefined && files.length > 0) {
      this.uploadFile = files[0]
      this.fileSelected = true;
    }
    else {
      this.uploadFile = null;
      this.fileSelected = false;
    }
  }

  clearFile() {
    if (this.uploadFile != undefined) {
      this.fileSelected = false;
      this.uploadFile = null;
    }
  }

  onDragOver(event) {    
    event.stopPropagation();
    event.preventDefault();    
    this.dragover = true;
  }

  onDragLeave(event) {           
    this.dragover = false;
  }

  onDrop(event) {    
    event.preventDefault();
    this.dragover = false;
    this.onFileUploadChange(event.dataTransfer.files);
  }

  onSubmitFileUpload() {  
    if (this.uploadFile == undefined || this.uploadFile.name == undefined) {
      this.fileSelected = false;
      return;
    }
    this.fileSelected = true;
    this.uploadSelected.emit(this.uploadFile);
  }
}
