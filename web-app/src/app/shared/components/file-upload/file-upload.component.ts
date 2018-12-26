import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-file-upload',
  templateUrl: './file-upload.component.html',
  styleUrls: ['./file-upload.component.scss']
})
export class FileUploadComponent implements OnInit {

  constructor() { }
  @Input() uploadFile: File;
  @Output() uploadSelected: EventEmitter<File> = new EventEmitter<File>();
  ngOnInit() {
  }

  onFileUploadChange(files: Array<File>) {
    this.uploadFile = files[0]
  }

  onSubmitFileUpload() {  
    if (this.uploadFile == undefined || this.uploadFile.name == undefined) {
      return;
    }
    this.uploadSelected.emit(this.uploadFile);
  }
}
