import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { Subject } from 'rxjs';
import { FileToUpload } from 'src/app/shared/models/fileToUpload';
import { UploadDownloadService } from '../upload-download.service';

// Maximum file size allowed to be uploaded = 3MB
const MAX_SIZE: number = 1048576*3;

@Component({
  selector: 'app-upload-download',
  templateUrl: './upload-download.component.html',
  styles: [
  ]
})
export class UploadDownloadComponent implements OnInit {
  @ViewChild('fileInput', {static: false})
  myFileInput: ElementRef;
  userId: number;
  
  theFiles: any[] = [];
  messages: string[] = [];


  constructor(private updownloadService: UploadDownloadService) { }

  ngOnInit(): void {

  }

  onFileChange(event) {
    this.theFiles = [];
    
    // Any file(s) selected from the input?
    if (event.target.files && event.target.files.length > 0) {
        for (let index = 0; index < event.target.files.length; index++) {
            let file = event.target.files[index];
            // Don't allow file sizes over 1MB
            if (file.size < MAX_SIZE) {
                // Add file to list of files
                this.theFiles.push(file);
            }
            else {
                this.messages.push("File: " + file.name + " is too large to upload.");
            }
        }
    }
  }


  private readAndUploadFile(theFile: any) {
      let file = new FileToUpload();
      
      // Set File Information
      file.fileName = theFile.name;
      file.fileSize = theFile.size;
      file.fileType = theFile.type;
      file.lastModifiedTime = theFile.lastModified;
      file.lastModifiedDate = theFile.lastModifiedDate;
      
      // Use FileReader() object to get file to upload
      // NOTE: FileReader only works with newer browsers
      let reader = new FileReader();
      
      // Setup onload event for reader
      reader.onload = () => {
          // Store base64 encoded representation of file
          file.fileAsBase64 = reader.result.toString();
          
          // POST to server
          this.updownloadService.uploadFile(file).subscribe(resp => { 
              this.messages.push("Upload complete"); });
      }
      
      // Read the file
      reader.readAsDataURL(theFile);
  }

  uploadFile(): void {
    for (let index = 0; index < this.theFiles.length; index++) {
        this.readAndUploadFile(this.theFiles[index]);
    }
}





}
